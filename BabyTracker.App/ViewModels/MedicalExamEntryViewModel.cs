using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record ExamTypeOption(string Key, string DisplayName, bool IsBuiltIn);

public partial class MedicalExamEntryViewModel : ObservableObject
{
    private readonly EntryRepository<MedicalExamEntry> _repository;
    private readonly CurrentChildContext _childContext;
    private Guid? _entryId;
    private DateTime _createdAt;

    public List<ExamTypeOption> ExamTypeOptions { get; }

    [ObservableProperty] private ExamTypeOption _selectedExamType;
    [ObservableProperty] private string _customExamName = "";
    [ObservableProperty] private DateTime _entryDate = DateTime.Today;
    [ObservableProperty] private TimeSpan _entryTime = DateTime.Now.TimeOfDay;
    [ObservableProperty] private string _result = "";
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private bool _isSaving;

    public bool IsCustomType => SelectedExamType.Key == CustomKey;
    private const string CustomKey = "__custom__";

    public event Action? Completed;

    public MedicalExamEntryViewModel(EntryRepository<MedicalExamEntry> repository, CurrentChildContext childContext)
    {
        _repository = repository;
        _childContext = childContext;

        var loc = LocalizationResourceManager.Instance;
        ExamTypeOptions =
        [
            .. MedicalExamFormatter.BuiltInExamKeys.Select(k => new ExamTypeOption(k, loc[$"MedicalExam_{k}"], true)),
            new ExamTypeOption(CustomKey, loc["MedicalExam_Custom"], false)
        ];
        _selectedExamType = ExamTypeOptions[0];
    }

    partial void OnSelectedExamTypeChanged(ExamTypeOption value) => OnPropertyChanged(nameof(IsCustomType));

    public void LoadEntry(MedicalExamEntry? entry)
    {
        if (entry is null)
        {
            _entryId = null;
            IsEditing = false;
            SelectedExamType = ExamTypeOptions[0];
            CustomExamName = "";
            EntryDate = DateTime.Today;
            EntryTime = DateTime.Now.TimeOfDay;
            Result = "";
            Notes = "";
            return;
        }

        _entryId = entry.Id;
        _createdAt = entry.CreatedAt;
        IsEditing = true;
        if (entry.IsBuiltIn)
        {
            SelectedExamType = ExamTypeOptions.FirstOrDefault(o => o.Key == entry.ExamType) ?? ExamTypeOptions[0];
            CustomExamName = "";
        }
        else
        {
            SelectedExamType = ExamTypeOptions.Last(); // "__custom__"
            CustomExamName = entry.ExamType;
        }
        EntryDate = entry.OccurredAt.Date;
        EntryTime = entry.OccurredAt.TimeOfDay;
        Result = entry.Result ?? "";
        Notes = entry.Notes ?? "";
    }

    [RelayCommand]
    private async Task Save()
    {
        if (_childContext.ChildId is not { } childId) return;
        if (IsCustomType && string.IsNullOrWhiteSpace(CustomExamName)) return;

        IsSaving = true;
        try
        {
            var occurredAt = EntryDate.Date + EntryTime;
            var examType = IsCustomType ? CustomExamName.Trim() : SelectedExamType.Key;
            var result = string.IsNullOrWhiteSpace(Result) ? null : Result.Trim();
            var notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim();

            if (_entryId is { } id)
            {
                await _repository.UpdateAsync(new MedicalExamEntry
                {
                    Id = id,
                    ChildId = childId,
                    CreatedAt = _createdAt,
                    OccurredAt = occurredAt,
                    ExamType = examType,
                    IsBuiltIn = !IsCustomType,
                    Result = result,
                    Notes = notes
                });
            }
            else
            {
                await _repository.AddAsync(new MedicalExamEntry
                {
                    ChildId = childId,
                    OccurredAt = occurredAt,
                    ExamType = examType,
                    IsBuiltIn = !IsCustomType,
                    Result = result,
                    Notes = notes
                });
            }

            await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Saved"]);
            Completed?.Invoke();
        }
        finally { IsSaving = false; }
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (_entryId is { } id) await _repository.DeleteAsync(id);
        await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Deleted"]);
        Completed?.Invoke();
    }

    [RelayCommand]
    private void AddToDeviceCalendar()
    {
        var occurredAt = EntryDate.Date + EntryTime;
        var examName = IsCustomType ? CustomExamName : SelectedExamType.DisplayName;
        var title = $"{LocalizationResourceManager.Instance["MedicalExam_Title"]}: {examName}";
        CalendarIntentHelper.AddToDeviceCalendar(title, occurredAt, Notes);
    }
}
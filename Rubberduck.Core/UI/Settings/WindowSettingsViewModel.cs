﻿using NLog;
using Rubberduck.Settings;
using Rubberduck.SettingsProvider;
using Rubberduck.UI.Command;
using Rubberduck.Resources;

namespace Rubberduck.UI.Settings
{
    public class WindowSettingsViewModel : SettingsViewModelBase, ISettingsViewModel
    {
        public WindowSettingsViewModel(Configuration config)
        {
            CodeExplorerVisibleOnStartup = config.UserSettings.WindowSettings.CodeExplorerVisibleOnStartup;
            CodeInspectionsVisibleOnStartup = config.UserSettings.WindowSettings.CodeInspectionsVisibleOnStartup;
            TestExplorerVisibleOnStartup = config.UserSettings.WindowSettings.TestExplorerVisibleOnStartup;
            TodoExplorerVisibleOnStartup = config.UserSettings.WindowSettings.TodoExplorerVisibleOnStartup;

            ExportButtonCommand = new DelegateCommand(LogManager.GetCurrentClassLogger(), _ => ExportSettings());
            ImportButtonCommand = new DelegateCommand(LogManager.GetCurrentClassLogger(), _ => ImportSettings());
        }

        #region Properties

        private bool _codeExplorerVisibleOnStartup;
        public bool CodeExplorerVisibleOnStartup
        {
            get => _codeExplorerVisibleOnStartup;
            set
            {
                if (_codeExplorerVisibleOnStartup != value)
                {
                    _codeExplorerVisibleOnStartup = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _codeInspectionsVisibleOnStartup;
        public bool CodeInspectionsVisibleOnStartup
        {
            get => _codeInspectionsVisibleOnStartup;
            set
            {
                if (_codeInspectionsVisibleOnStartup != value)
                {
                    _codeInspectionsVisibleOnStartup = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private bool _testExplorerVisibleOnStartup;
        public bool TestExplorerVisibleOnStartup
        {
            get => _testExplorerVisibleOnStartup;
            set
            {
                if (_testExplorerVisibleOnStartup != value)
                {
                    _testExplorerVisibleOnStartup = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _todoExplorerVisibleOnStartup;
        public bool TodoExplorerVisibleOnStartup
        {
            get => _todoExplorerVisibleOnStartup;
            set
            {
                if (_todoExplorerVisibleOnStartup != value)
                {
                    _todoExplorerVisibleOnStartup = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public void UpdateConfig(Configuration config)
        {
            config.UserSettings.WindowSettings.CodeExplorerVisibleOnStartup = CodeExplorerVisibleOnStartup;
            config.UserSettings.WindowSettings.CodeInspectionsVisibleOnStartup = CodeInspectionsVisibleOnStartup;
            config.UserSettings.WindowSettings.TestExplorerVisibleOnStartup = TestExplorerVisibleOnStartup;
            config.UserSettings.WindowSettings.TodoExplorerVisibleOnStartup = TodoExplorerVisibleOnStartup;
        }

        public void SetToDefaults(Configuration config)
        {
            TransferSettingsToView(config.UserSettings.WindowSettings);
        }

        private void TransferSettingsToView(Rubberduck.Settings.WindowSettings toLoad)
        {
            CodeExplorerVisibleOnStartup = toLoad.CodeExplorerVisibleOnStartup;
            CodeInspectionsVisibleOnStartup = toLoad.CodeInspectionsVisibleOnStartup;
            TestExplorerVisibleOnStartup = toLoad.TestExplorerVisibleOnStartup;
            TodoExplorerVisibleOnStartup = toLoad.TodoExplorerVisibleOnStartup;
        }

        private void ImportSettings()
        {
            using (var dialog = new OpenFileDialog
            {
                Filter = RubberduckUI.DialogMask_XmlFilesOnly,
                Title = RubberduckUI.DialogCaption_LoadWindowSettings
            })
            {
                dialog.ShowDialog();
                if (string.IsNullOrEmpty(dialog.FileName)) return;
                var service = new XmlPersistanceService<Rubberduck.Settings.WindowSettings> { FilePath = dialog.FileName };
                var loaded = service.Load(new Rubberduck.Settings.WindowSettings());
                TransferSettingsToView(loaded);
            }
        }

        private void ExportSettings()
        {
            using (var dialog = new SaveFileDialog
            {
                Filter = RubberduckUI.DialogMask_XmlFilesOnly,
                Title = RubberduckUI.DialogCaption_SaveWindowSettings
            })
            {
                dialog.ShowDialog();
                if (string.IsNullOrEmpty(dialog.FileName)) return;
                var service = new XmlPersistanceService<Rubberduck.Settings.WindowSettings> { FilePath = dialog.FileName };
                service.Save(new Rubberduck.Settings.WindowSettings()
                {
                    CodeExplorerVisibleOnStartup = CodeExplorerVisibleOnStartup,
                    CodeInspectionsVisibleOnStartup = CodeInspectionsVisibleOnStartup,
                    TestExplorerVisibleOnStartup = TestExplorerVisibleOnStartup,
                    TodoExplorerVisibleOnStartup = TodoExplorerVisibleOnStartup
                });
            }
        }
    }
}

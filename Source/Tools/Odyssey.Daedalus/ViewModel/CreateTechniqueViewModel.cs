using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Organization;
using Odyssey.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ICommand = System.Windows.Input.ICommand;

namespace Odyssey.Daedalus.ViewModel
{
    public class CreateTechniqueViewModel : ViewModelBase
    {
        ShaderModel shaderModel;
        private PreferredRasterizerState rasterizerState;
        private PreferredBlendState blendState;
        private PreferredDepthStencilState depthStencilState;
        string searchFilter;
        readonly ObservableCollection<ShaderAttributeViewModel> vsAttributes;
        readonly ObservableCollection<ShaderAttributeViewModel> psAttributes;
        readonly ObservableCollection<ShaderAttributeViewModel> selectedFlags;

        public ObservableCollection<ShaderAttributeViewModel> VertexShaderFlags { get { return vsAttributes; } }
        public ObservableCollection<ShaderAttributeViewModel> PixelShaderFlags { get { return psAttributes; } }
        public ObservableCollection<ShaderAttributeViewModel> SelectedFlags { get { return selectedFlags; } }

        public CollectionViewSource VertexShaderView { get; set; }
        public CollectionViewSource PixelShaderView { get; set; }
        public TechniqueKey Key { get; private set; }
        public string Name { get; set; }

        public string SearchFilter
        {
            get { return searchFilter; }
            set
            {
                if (!string.IsNullOrEmpty(SearchFilter))
                    AddFilter();
                
                searchFilter = value;
                VertexShaderView.View.Refresh(); // important to refresh your View
            }
        }

        public ShaderModel SelectedShaderModel { get { return shaderModel; } set { shaderModel = value; RaisePropertyChanged("SelectedShaderModel"); } }
        public PreferredBlendState SelectedBlendState { get { return blendState; } set { blendState = value; RaisePropertyChanged("SelectedBlendState"); } }
        public PreferredRasterizerState SelectedRasterizerState { get { return rasterizerState; } set { rasterizerState = value; RaisePropertyChanged("SelectedRasterizerState"); } }
        public PreferredDepthStencilState SelectedDepthStencilState { get { return depthStencilState; } set { depthStencilState = value; RaisePropertyChanged("SelectedDepthStencilState"); } }

        public ICommand UpdateFlagCommand
        {
            get { return new RelayCommand<ShaderAttributeViewModel>(UpdateFlag); }
        }

        public ICommand CloseWindowCommand
        {
            get { return new RelayCommand<Window>((window) => window.Close()); }
        }

        public ICommand GenerateKeyCommand
        {
            get
            {
                return new RelayCommand<Window>((window) =>
                {
                    Key = GenerateKey(SelectedFlags, SelectedShaderModel, SelectedRasterizerState, SelectedBlendState, SelectedDepthStencilState);
                    window.DialogResult = true;
                    window.Close();
                }, (w) => selectedFlags.Any());
            }
        }

        public CreateTechniqueViewModel()
        {
            const string none = "None";
            vsAttributes = new ObservableCollection<ShaderAttributeViewModel>(from s in Enum.GetNames(typeof(VertexShaderFlags))
                                                                              where s != none
                                                                              select new ShaderAttributeViewModel
                                                                              {
                                                                                  Name = s,
                                                                                  Type = ShaderType.Vertex
                                                                              });
            psAttributes = new ObservableCollection<ShaderAttributeViewModel>(from s in Enum.GetNames(typeof(PixelShaderFlags))
                                                                              where s != none
                                                                              select new ShaderAttributeViewModel { Name = s, Type = ShaderType.Pixel });

            VertexShaderView = new CollectionViewSource() { Source = vsAttributes };
            PixelShaderView = new CollectionViewSource { Source = psAttributes };
            selectedFlags = new ObservableCollection<ShaderAttributeViewModel>();
        }

        private void UpdateFlag(ShaderAttributeViewModel vm)
        {
            if (SelectedFlags.Contains(vm))
                SelectedFlags.Remove(vm);
            else 
                SelectedFlags.Add(vm);
        }

        private void AddFilter()
        {
            AddFilter(VertexShaderView, Filter);
            AddFilter(PixelShaderView, Filter);
        }

        private void AddFilter(CollectionViewSource view, FilterEventHandler eventHandler)
        {
            view.Filter -= eventHandler;
            view.Filter += eventHandler;
        }

        private void Filter(object sender, FilterEventArgs e)
        {
            e.Accepted = ((ShaderAttributeViewModel)e.Item).Name.ToLowerInvariant().Contains(SearchFilter);
        }

        
        static TechniqueKey GenerateKey(IEnumerable<ShaderAttributeViewModel> data, ShaderModel selectedShaderModel, 
            PreferredRasterizerState rasterizerState, PreferredBlendState blendState, PreferredDepthStencilState depthStencilState)
        {
            Contract.Requires<ArgumentNullException>(data != null);
            VertexShaderFlags vsFlags = Graphics.Effects.VertexShaderFlags.None;
            PixelShaderFlags psFlags = Graphics.Effects.PixelShaderFlags.None;

            foreach (var flagVM in data)
            {
                switch (flagVM.Type)
                {
                    case ShaderType.Vertex:
                        vsFlags |= (VertexShaderFlags)Enum.Parse(typeof(VertexShaderFlags), flagVM.Name);
                        break;
                    case ShaderType.Pixel:
                        psFlags |= (PixelShaderFlags)Enum.Parse(typeof(PixelShaderFlags), flagVM.Name);
                        break;
                }
            }

            return new TechniqueKey(vsFlags, psFlags, selectedShaderModel, rasterizerState, blendState, depthStencilState);
        }
    }
}

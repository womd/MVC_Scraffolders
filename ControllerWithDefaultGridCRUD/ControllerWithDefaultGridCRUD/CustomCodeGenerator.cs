﻿using ControllerWithDefaultGridCRUD.UI;
using Microsoft.AspNet.Scaffolding;
using System.Collections.Generic;

namespace ControllerWithDefaultGridCRUD
{
    public class CustomCodeGenerator : CodeGenerator
    {
        CustomViewModel _viewModel;

        /// <summary>
        /// Constructor for the custom code generator
        /// </summary>
        /// <param name="context">Context of the current code generation operation based on how scaffolder was invoked(such as selected project/folder) </param>
        /// <param name="information">Code generation information that is defined in the factory class.</param>
        public CustomCodeGenerator(
            CodeGenerationContext context,
            CodeGeneratorInformation information)
            : base(context, information)
        {
            _viewModel = new CustomViewModel(Context);
        }


        /// <summary>
        /// Any UI to be displayed after the scaffolder has been selected from the Add Scaffold dialog.
        /// Any validation on the input for values in the UI should be completed before returning from this method.
        /// </summary>
        /// <returns></returns>
        public override bool ShowUIAndValidate()
        {
            // Bring up the selection dialog and allow user to select a model type
            SelectModelWindow window = new SelectModelWindow(_viewModel);
            bool? showDialog = window.ShowDialog();
            return showDialog ?? false;
        }

        /// <summary>
        /// This method is executed after the ShowUIAndValidate method, and this is where the actual code generation should occur.
        /// In this example, we are generating a new file from t4 template based on the ModelType selected in our UI.
        /// </summary>
        public override void GenerateCode()
        {
            // Get the selected code type
            var nameSpace = _viewModel.NameSpace;
            var controllerName = _viewModel.ControllerName;
            var viewModel = _viewModel.ViewModel;
            var gridName = _viewModel.GridName;

            // Setup the scaffolding item creation parameters to be passed into the T4 template.
            var parameters = new Dictionary<string, object>()
            {
                { "NameSpace",nameSpace },
                { "ControllerName", controllerName },
                { "ViewModel", viewModel },
                { "GridName", gridName },
                
                //You can pass more parameters after they are defined in the template
            };

            //add the Controller
            this.AddFileFromTemplate(Context.ActiveProject,
                string.Format("Controllers\\{0}Controller",controllerName),
                "AjaxCRUDControllerTemplate",
                parameters,
                skipIfExists: false);

            //create Views - Folder
            this.AddFolder(Context.ActiveProject, string.Format("Views\\{0}", controllerName));
            //add View
            this.AddFileFromTemplate(Context.ActiveProject,
                string.Format("Views\\{0}\\{1}", controllerName, gridName),
                "AjaxBatchGridViewTemplate",
                parameters,
                skipIfExists: false);

            //add default index with partialview embedded
            this.AddFileFromTemplate(Context.ActiveProject,
                string.Format("Views\\{0}\\Index", controllerName),
                "IndexWithGridPartial",
                parameters,
                skipIfExists: false);

        }


    }
}

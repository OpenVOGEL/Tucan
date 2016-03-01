using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DotNumerics_Samples.Runner;
using DotNumerics_Samples.Dumper;
using DotNumerics_Samples.Harness;
using DotNumerics_Samples.Utils;
namespace DotNumerics_Samples
{
    public partial class FormSampleRunner : Form
    {
        private Sample _currentSample;
        private SampleRunner _runner;

        public FormSampleRunner(SampleGroup rootSampleGroup)
        {
            InitializeComponent();

            this.samplesTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.samplesTreeView_AfterCollapse);
            this.samplesTreeView.DoubleClick += new System.EventHandler(this.samplesTreeView_DoubleClick);
            this.samplesTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.samplesTreeView_BeforeCollapse);
            this.samplesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.samplesTreeView_AfterSelect);
            this.samplesTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.samplesTreeView_AfterExpand);


            Text = rootSampleGroup.Title;

            ObjectDumper dumper = new ObjectDumper();
            dumper.OutputTextBox = outputTextBox;

            _runner = new SampleRunnerImpl(this, dumper);

            TreeNode rootNode = CreateTreeNode(rootSampleGroup, 0);

            rootNode.ImageKey = rootNode.SelectedImageKey = "Help";
            samplesTreeView.Nodes.Add(rootNode);
            rootNode.ExpandAll();
        }

        private TreeNode CreateTreeNode(SampleBase sample, int level)
        {
            TreeNode node = new TreeNode(sample.Title);
            node.Tag = sample;
            SampleGroup sg = sample as SampleGroup;
            if (sg != null)
            {
                foreach (SampleBase sb in sg.Children)
                {
                    node.Nodes.Add(CreateTreeNode(sb, level + 1));
                }
                node.ImageKey = node.SelectedImageKey = "BookClosed";
            }
            else
            {
                node.ImageKey = node.SelectedImageKey = "Item";
            }
            return node;
        }
        private void samplesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode currentNode = samplesTreeView.SelectedNode;
            _currentSample = currentNode.Tag as Sample;
            outputTextBox.Clear();
            if (_currentSample != null)
            {
                runButton.Enabled = true;
                descriptionTextBox.Text = _currentSample.Description;
                codeRichTextBox.Clear();
                codeRichTextBox.Text = _currentSample.SourceCode;
                SyntaxColorizer.ColorizeCode(codeRichTextBox);
            }
            else
            {
                runButton.Enabled = false;
                descriptionTextBox.Text = "Select a query from the tree to the left.";
                codeRichTextBox.Clear();
            }
        }

        private void samplesTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Level)
            {
                case 1:
                case 2:
                    e.Node.ImageKey = "BookOpen";
                    e.Node.SelectedImageKey = "BookOpen";
                    break;
            }
        }

        private void samplesTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            switch (e.Node.Level)
            {
                case 0:
                    e.Cancel = true;
                    break;
            }
        }

        private void samplesTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Level)
            {
                case 1:
                    e.Node.ImageKey = "BookStack";
                    e.Node.SelectedImageKey = "BookStack";
                    break;

                case 2:
                    e.Node.ImageKey = "BookClosed";
                    e.Node.SelectedImageKey = "BookClosed";
                    break;
            }
        }

        private void samplesTreeView_DoubleClick(object sender, EventArgs e)
        {
            if (this._currentSample != null)
            {
                RunCurrentSample();
            }
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            RunCurrentSample();
        }

        private void RunCurrentSample()
        {
            this.outputTextBox.Text = "";
            _runner.Run(_currentSample);
        }


        class SampleRunnerImpl : SampleRunner
        {
            private FormSampleRunner _form;

            internal SampleRunnerImpl(FormSampleRunner form, IObjectDumper dumper)
                : base(dumper)
            {
                _form = form;
            }

            public override void OnStarting(Sample sample)
            {

            }

            public override void OnSuccess(Sample sample)
            {
            }

            public override void OnFailure(Sample sample, Exception ex)
            {
                //this.ObjectDumper.Write(ex);

            }

            public override void OnStartingGroup(SampleGroup group)
            {
            }

            public override void OnFinishedGroup(SampleGroup group)
            {
            }
        }

    }
}

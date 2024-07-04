﻿﻿// This file was auto-generated by ML.NET Model Builder. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Trainers;
using Microsoft.ML;

namespace TestDataMaker
{
    public partial class Y_Value_1
    {
        /// <summary>
        /// Retrains model using the pipeline generated as part of the training process. For more information on how to load data, see aka.ms/loaddata.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="trainData"></param>
        /// <returns></returns>
        public static ITransformer RetrainPipeline(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.ReplaceMissingValues(new []{new InputOutputColumnPair(@"B1", @"B1"),new InputOutputColumnPair(@"B2", @"B2"),new InputOutputColumnPair(@"B3", @"B3"),new InputOutputColumnPair(@"B4", @"B4"),new InputOutputColumnPair(@"B5", @"B5"),new InputOutputColumnPair(@"B6", @"B6"),new InputOutputColumnPair(@"B7", @"B7"),new InputOutputColumnPair(@"B8", @"B8"),new InputOutputColumnPair(@"B9", @"B9"),new InputOutputColumnPair(@"B10", @"B10"),new InputOutputColumnPair(@"B11", @"B11"),new InputOutputColumnPair(@"B12", @"B12"),new InputOutputColumnPair(@"B13", @"B13"),new InputOutputColumnPair(@"B14", @"B14"),new InputOutputColumnPair(@"B15", @"B15"),new InputOutputColumnPair(@"B16", @"B16"),new InputOutputColumnPair(@"B17", @"B17"),new InputOutputColumnPair(@"B18", @"B18"),new InputOutputColumnPair(@"B19", @"B19")})      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"B1",@"B2",@"B3",@"B4",@"B5",@"B6",@"B7",@"B8",@"B9",@"B10",@"B11",@"B12",@"B13",@"B14",@"B15",@"B16",@"B17",@"B18",@"B19"}))      
                                    .Append(mlContext.Regression.Trainers.FastTree(new FastTreeRegressionTrainer.Options(){NumberOfLeaves=84,MinimumExampleCountPerLeaf=9,NumberOfTrees=439,MaximumBinCountPerFeature=196,FeatureFraction=0.99999999,LearningRate=0.0413915385053563,LabelColumnName=@"LocationY",FeatureColumnName=@"Features"}));

            return pipeline;
        }
    }
}

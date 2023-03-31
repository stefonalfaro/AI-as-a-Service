# OpenAI.cs SDK Documentation

This documentation describes how to use the OpenAI.cs SDK to interact with OpenAI's API for language model fine-tuning and other operations.

# Getting Started

To get started, first install the OpenAI.cs SDK in your project using the package manager or by downloading the source code.

Next, initialize the OpenAI client with your API key:

```
var openAI = new OpenAI("your_api_key");
```
Controllers
FileController
Upload File

# Upload a file for fine-tuning.

```
[HttpPost("upload")]
public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest request)
{
    var uploadedFile = await _openAI.UploadFileAsync(request.File, request.Purpose);
    return Ok(uploadedFile);
}
```

# FineTuningController
Create Fine-tune

Create a fine-tuning job using the specified parameters.
```
[HttpPost("create")]
public async Task<IActionResult> CreateFineTune([FromBody] CreateFineTuneRequest request)
{
    var fineTune = await _openAI.CreateFineTuneAsync(request.TrainingFile, request.ValidationFile, request.Model, request.NEpochs, request.BatchSize, request.LearningRateMultiplier, request.PromptLossWeight, request.ComputeClassificationMetrics, request.ClassificationNClasses, request.ClassificationPositiveClass, request.ClassificationBetas, request.Suffix);
    return Ok(fineTune);
}
```

# AboutController
Get API Version

Return an object containing the API version.
```
public class AboutResponse
{
    public string APIVersion { get; set; }
}
```

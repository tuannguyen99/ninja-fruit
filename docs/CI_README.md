## CI / Local Test Instructions

This document explains how to run Unity EditMode and PlayMode tests locally and how to re-enable CI if desired.

Running tests locally (Windows PowerShell):

```powershell
$unity = "C:\Program Files\Unity\Hub\Editor\6000.0.62f1\Editor\Unity.exe"
& $unity -projectPath "$PWD\ninja-fruit" -runTests -testPlatform EditMode -testResults "ninja-fruit/logs/editmode-test-results.xml" -batchmode -nographics -quit
& $unity -projectPath "$PWD\ninja-fruit" -runTests -testPlatform PlayMode -testResults "ninja-fruit/logs/playmode-test-results.xml" -batchmode -nographics -quit

Get-Content .\ninja-fruit\logs\editmode-test-results.xml
Get-Content .\ninja-fruit\logs\playmode-test-results.xml
```

Where to find test logs:

- `ninja-fruit/logs/editmode-test-results.xml`
- `ninja-fruit/logs/playmode-test-results.xml`

Re-enabling CI on GitHub-hosted runners:

GitHub-hosted runners require a valid Unity license. Options:

- Provide a paid Unity seat serial using the `UNITY_SERIAL` repository secret.
- Use a self-hosted runner (Windows) with Unity Editor installed and activated; change the workflow `runs-on` to `self-hosted` and provide appropriate labels.

If you choose to store test artifacts in the repo, consider adding `ninja-fruit/logs/` to `.gitignore` to avoid accidental commits.

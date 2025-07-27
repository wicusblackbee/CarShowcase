# 🚀 Deployment Guide

## GitHub Actions CI/CD for Blazor Application

The project includes automated deployment to GitHub Pages with the Blazor application:

### 1. Continuous Integration
- **Builds the Blazor Server application** using `dotnet publish`
- **Runs all 161 tests** with coverage collection
- **Generates HTML coverage reports** using ReportGenerator
- **Validates 70%+ coverage threshold** as a quality gate
- **Publishes the Blazor app** for deployment

### 2. Continuous Deployment
- **Deploys the Blazor application** to GitHub Pages on main/master branch
- **Creates a professional showcase page** with project metrics and features
- **Includes interactive coverage reports** accessible via web interface
- **Uses `PERSONAL_TOKEN` secret** for secure authentication

## Setting up GitHub Pages Deployment

### Step 1: Create a Personal Access Token
1. Go to GitHub Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Click "Generate new token (classic)"
3. Give it a descriptive name (e.g., "Car Showcase Deployment")
4. Select the following scopes:
   - `repo` (Full control of private repositories)
   - `workflow` (Update GitHub Action workflows)
5. Set expiration as needed (recommend 90 days or no expiration for demos)
6. Click "Generate token"
7. **Copy the generated token immediately** (you won't see it again)

### Step 2: Add the Token as a Repository Secret
1. Go to your repository Settings → Secrets and variables → Actions
2. Click "New repository secret"
3. **Name**: `PERSONAL_TOKEN` (exactly as specified)
4. **Value**: Paste your personal access token from Step 1
5. Click "Add secret"

### Step 3: Enable GitHub Pages
1. Go to repository Settings → Pages
2. **Source**: "Deploy from a branch"
3. **Branch**: `gh-pages` (will be created automatically by the workflow)
4. **Folder**: `/ (root)`
5. Click "Save"

### Step 4: Deploy
1. **Push to main/master branch** - the workflow will automatically trigger
2. **Monitor the workflow** in the Actions tab
3. **Wait for deployment** (usually takes 2-3 minutes)
4. **Access your site** at `https://yourusername.github.io/repository-name/`

## Workflow Features

### Quality Gates
- ✅ **Build Validation**: Ensures the Blazor app compiles successfully
- ✅ **Test Validation**: All 161 tests must pass
- ✅ **Coverage Threshold**: Minimum 70% code coverage required
- ✅ **Deployment Safety**: Only deploys on successful builds

### Deployment Package
The workflow creates a comprehensive deployment package including:
- **Professional Landing Page**: Showcases the application features and metrics
- **Coverage Reports**: Interactive HTML reports accessible at `/coverage/`
- **Project Documentation**: README and technical details
- **Quality Metrics**: Real-time display of test results and coverage

### Automatic Updates
- **On every push** to main/master branch
- **Coverage reports** are regenerated with latest test results
- **Metrics are updated** automatically in the showcase page
- **No manual intervention** required

## Manual Deployment (Alternative)

If you prefer manual deployment:

```bash
# Build the Blazor application
dotnet publish CarShowcase/CarShowcase.csproj -c Release -o publish

# Run tests and generate coverage
dotnet test CarShowcase.Tests --collect:"XPlat Code Coverage" --results-directory:TestResults
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"TestResults/*/coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:Html

# Deploy to your hosting provider
# (Copy contents of publish/wwwroot/ directory to your web server)
```

## Troubleshooting

### Common Issues

1. **"PERSONAL_TOKEN not found"**
   - Ensure the secret is named exactly `PERSONAL_TOKEN`
   - Verify the token has `repo` and `workflow` scopes

2. **"Permission denied"**
   - Check that the personal access token hasn't expired
   - Ensure the token has sufficient permissions

3. **"gh-pages branch not found"**
   - The workflow creates this branch automatically
   - If it fails, check the workflow logs for errors

4. **"Coverage threshold not met"**
   - The workflow requires 70%+ code coverage
   - Run tests locally to check coverage: `dotnet test --collect:"XPlat Code Coverage"`

### Workflow Logs
- Go to repository → Actions tab
- Click on the latest workflow run
- Expand each step to see detailed logs
- Look for error messages in red

## Live Demo

Once deployed, your application will be available at:
- **Main Site**: `https://yourusername.github.io/repository-name/`
- **Coverage Report**: `https://yourusername.github.io/repository-name/coverage/`

The deployment showcases:
- 🚗 **Car Management Features** with advanced search and filtering
- 🧪 **Testing Excellence** with 161 passing tests and 75%+ coverage
- 🎨 **Modern UI Components** with 8 reusable Blazor components
- 🏗️ **Professional Architecture** with clean separation of concerns

## Security Notes

- The `PERSONAL_TOKEN` is stored securely as a GitHub secret
- The token is only used during the deployment workflow
- No sensitive information is exposed in the deployed application
- The workflow only has access to the specific repository

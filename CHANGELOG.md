# Changelog
## [2.1.1] (05/02/2024)
- ### Changed
- - Updated dependency `com.cobilas.unity.utility` to version `2.10.3`.
- - Updated dependency `com.cobilas.unity.management.runtime` to version `2.2.1`.
- - This update includes bug fixes and new features that do not directly impact this package.
- - The sub-dependency `com.cobilas.unity.core.net4x@1.4.1` was made explicit in the package dependencies.
## [2.1.0] 25/01/2024
### Changed
A change in package dependencies.
## [2.0.1] - 06/09/2023
###Fixed
- Correction in package dependencies.
## [2.0.0] - 05/09/2023
### Changed
- The functions of the `CobilasAssetManagement` class were incorporated into the `ResourceManager` class.
- The `CRC` structure has been moved to `CRC.cs` and has also become read-only.
### Removed
```c#
     public static class CobilasAssetManagementWin;
     public class AssetsItemRefDraw;
     public class AssetsItemRef;
     public static class CobilasAssetManagement;
```
## [2.0.0-rc1] - 03/09/2023
### Added
- The `ResourceManager` class has been added.
### Deprecate
- The `CobilasResources` class has been replaced by the `ResourceManager` class.
## [1.14.0] - 29/08/2023
### Changed
- Package dependencies have been changed.
## [1.13.0-ch1] - 28/08/2023
### Changed
- The package author was changed from `Cobilas CTB` to `BélicusBr`.
## [1.0.12] - 30/01/2023
### Changed
#### CobilasAssetManagementWin
- Removal of unnecessary assignments.
#### CobilasResources
- The `T:GetComponentInGameObject<T>(string)` method received the `where T : Component` restriction.
- Transforming possible fields into `readonly`.
## [1.0.11] 17/11/2021
#### Change 1
`CobilasResources` and `CobilasAssetManagement` are using the new `StartMethodOnRun` for initialization.
## [1.0.7] 13/08/2022
- Change Editor\Cobilas.Unity.Editor.Management.Resource.asmdef
- Change Runtime\Cobilas.Unity.Management.Resource.asmdef
- Change Runtime\CobilasResources\CobilasResources.cs
## [1.0.7] 11/08/2022
- Change Runtime\Cobilas.Unity.Management.Resource.asmdef
## [1.0.7] 09/08/2022
- Change CHANGELOG.md
- Change Runtime\CobilasResources\CobilasResources.cs
## [1.0.7] 08/08/2022
- Fix CHANGELOG.md
- Fix package.json
- Remove CRItem.cs
- Add CobilasResourcesInspector.cs
- Change CobilasResources.cs
## [1.0.6] 31/07/2021
- Fix CHANGELOG.md
- Fix package.json
- Add Cobilas MG Resources.asset
- Remove Runtime\DependencyWarning.cs
- Remove Editor\DependencyWarning.cs
## [1.0.5] 27/07/2022
- Fix CHANGELOG.md
- Fix package.json
- Fix CobilasResources.cs
-> The instructions `void UnloadActive(Prefabs); void Init();` the `private static void InitEditor();` method was added.
## [1.0.4] 23/07/2022
- Fix Runtime/DependencyWarning.cs
- Add CHANGELOG.md
- Fix package.json
## [1.0.3] 22/07/2022
- Add Editor/DependencyWarning.cs
- Add Runtime/DependencyWarning.cs
- Fix LICENSE.md
- Fix Cobilas.Unity.Management.Resource.asmdef
- Fix Cobilas.Unity.Editor.Management.Resource.asmdef
## [1.0.2] 17/07/2022
- Delete main.yml
- Delete README.md
- Fix package.json
## [1.0.0] 15/07/2022
- Add package.json
- Add LICENSE.md
- Add folder:Editor
- Add folder:Runtime
## [0.0.1] 15/07/2022
### Repository com.cobilas.unity.management.resource started
- Released to GitHub
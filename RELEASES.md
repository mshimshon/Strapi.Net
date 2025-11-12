## v1.0.0
- Fixed Filter Issues

## 🔖 Versioning Policy

### 🚧 Pre-1.0.0 (`0.x.x`)

- The project is considered **Work In Progress**.
- **Breaking changes can occur at any time** without notice.
- No guarantees are made about stability or upgrade paths.

### ✅ Post-1.0.0 (`1.x.x` and beyond)

Follows a common-sense semantic versioning pattern:

- **Major (`X.0.0`)**  
  
  - Introduces major features or architectural changes  
  - May include well documented **breaking changes**

- **Minor (`1.X.0`)**  
  
  - Adds new features or enhancements  
  - May include significant bug fixes  
  - **No breaking changes**

- **Patch (`1.0.X`)**  
  
  - Hotfixes or urgent bug fixes  
  - Safe to upgrade  
  - **No breaking changes**


## v0.9.54
- Bobo Hotfix.

## v0.95 (Initial Release)
- Fluent query building with `IStrapiBuilder` (filters, sorting, pagination, populate)
- Strongly typed response mapping with `StrapiResponse<T>`
- Support for both single and collection endpoints
- Standardized error model mapping from Strapi error format
- Abstract-friendly design for custom `IStrapiClient` implementations
- Utilities for decoding Strapi responses with case-insensitive property resolution
- Compatible with ASP.NET Core, Blazor, and Clean Architecture patterns
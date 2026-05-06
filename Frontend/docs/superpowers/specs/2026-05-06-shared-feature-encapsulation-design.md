# Shared Feature Encapsulation Design

## Goal

Review every file in `Frontend/src/shared` manually and keep only code that is truly shared by multiple feature or app areas. Code used by a single feature should live inside that feature so each feature owns the code it needs and can be separated or extended independently later.

## Classification rule

A file stays in `src/shared` only when it has direct imports from at least two areas outside `src/shared`.

An area is the first meaningful owner outside shared, such as `src/features/<feature>`, `src/app`, `src/proxy.ts`, or another top-level runtime entry. If a helper is directly used by only one feature or app area, move it into that owner even if it looks like infrastructure code.

## Internal shared files

Some files under `src/shared` are internal parts of a component or module and are only imported by another file in `src/shared`. For these files, classify the public entrypoint or parent module first:

- If the entrypoint has direct imports from two or more outside areas, keep the entrypoint and its internal files together in `src/shared`.
- If the entrypoint is used by one outside area, move the whole relevant module group into that area.
- If no outside consumer is clear, mark the file or group as `review-unused` and inspect manually before changing it.

## Workflow

1. Build an inventory for all files under `src/shared` with direct importers outside shared, owning areas, and whether the file is internal to a shared entrypoint.
2. Manually review each file or module group, especially files with zero or one direct outside area.
3. Apply one of three outcomes:
   - `keep`: direct imports from at least two outside areas.
   - `move:<area>`: direct imports from exactly one outside area, or internal to an entrypoint used by exactly one outside area.
   - `review-unused`: no clear outside consumer after checking internal imports.
4. Move files or module groups into the owning feature/app area and update imports.
5. Do not change runtime behavior, component behavior, naming, or formatting beyond what the move requires.

## Verification

Run the relevant TypeScript, lint, and test checks after import updates. If app-shell or UI-facing shared modules move, run the frontend app and verify the affected golden paths in browser before claiming completion.

## Non-goals

- No broad refactor of feature APIs.
- No new abstractions or compatibility re-export shims.
- No deletion of unclear files without manual review.
- No exception for platform/helper code that is only used by one feature.

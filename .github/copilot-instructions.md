# GitHub Copilot Instructions for No Vanilla Apparel Mod

## Mod Overview and Purpose

The "No Vanilla Apparel" mod is designed for RimWorld to enhance gameplay by removing all vanilla apparel from the game environment. The primary purpose of this mod is to ensure that only apparel from other mods is available in the game, providing players with a unique and tailored experience.

The mod achieves this by disabling the spawning and usage of vanilla apparel and their recipes. This ensures that new vanilla apparel does not appear during gameplay, although existing apparel from saved games and scenarios will remain unaffected. It's important to note that the base classes of apparel are left intact to maintain mod compatibility.

**Potential Issues:** The mod may cause errors if no other apparel-mods are loaded, as it is designed to work alongside additional apparel modifications.

## Key Features and Systems

- **Vanilla Apparel Removal:** The mod iterates through all apparel items in the game, setting their properties to prevent them from spawning and being usable.
- **Layer-Specific Toggles:** Headgear, upper body, lower body, utility (belt layer), and armor removal are controlled independently so utility items (such as shield belts) can be kept.
- **Recipe Inhibition:** Vanilla apparel recipes are disabled to ensure factions cannot produce them.
- **Compatibility with Other Mods:** The mod is designed to be used with other apparel mods, leaving the base classes untouched for versatility.

## Coding Patterns and Conventions

- The mod is developed using C# and targets the .NET Framework versions 4.7.2, 4.8, and 4.8.1.
- Classes are defined with an emphasis on internal visibility to encapsulate logic and maintain focus on mod-specific operations.

### Key Classes & Files:
- **NoVanillaApparel.cs:** Contains static methods to iterate through and modify apparel items.
- **NoVanillaApparelMod.cs:** Acts as the main entry point, inheriting from `Mod`, which initializes the mod.
- **NoVanillaApparelSettings.cs:** Handles any settings-related functionalities, inheriting from `ModSettings`.

## XML Integration

While this mod predominantly focuses on C# coding, XML files may be used for configuration and settings within the game. The mod is not heavily reliant on XML; however, any extensions or future enhancements might involve XML for defining mod settings or additional integration points.

## Harmony Patching

The mod employs Harmony for targeted patching, ensuring that modifications to apparel properties do not interfere with the core mechanics of the game and other mods. This allows for dynamic changes during gameplay without altering the core game files permanently. 

- Utilize `HarmonyPatch` to apply changes selectively and revert them when necessary.
- Ensure patches are well-documented and limited to avoid performance issues.

## Suggestions for Copilot

When using GitHub Copilot to assist in writing or enhancing this mod, consider the following:

- **Iterate Efficiently:** Use Copilot to automate repetitive tasks such as iterating through lists of apparel objects.
- **Harmony Patching:** Assist with generating boilerplate code for new Harmony patches.
- **Robust Error Handling:** Implement error-checking code, especially where user-generated content (from other mods) might conflict.
- **Testing & Debugging Aids:** Generate unit tests or debug utilities to rapidly validate assumptions about gameplay behavior.
- **User Feedback Integration:** Allow Copilot to suggest enhancements based on user feedback data and common usage patterns.

Feel free to reach out with suggestions or further customizations to enhance this mod's functionality!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.

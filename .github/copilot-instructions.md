# GitHub Copilot Instructions for Immersive-ish Research (Continued)

## Mod Overview and Purpose

Immersive-ish Research (Continued) significantly modifies the way research is conducted in RimWorld. This mod introduces a more immersive research system, where players engage in experiments to unlock new research projects. It brings realism to the research process and challenges players to think strategically about managing their intellectual colonists and research resources.

## Key Features and Systems

- **Experiment System**: Intellectual colonists can perform research experiments at a new Experiment Bench. These experiments unlock research projects based on scientific fields.
- **'Brain Drain' - Education System**: Colonists who study completed experiments become researchers. Losing these researchers can lead to significant setbacks in research progress.
- **Experiment Filing Cabinet**: Allows storage and retrieval of completed experiments, easing research management.
- **Ancient Datadisks**: Obtain and decode datadisks for new research data, scattered throughout the world or from traders.
- **Datadisk Analyzer**: Decodes locked datadisks when connected to a powered research bench.
- **Research Progression Changes**: Research options are hidden until unlocked through experiments or datadisks.

## Coding Patterns and Conventions

- **Static Classes for Defs**: Utilize static classes like `AddExperimentJobDefOf` and `LoreComputerDefOf` to organize and define job and item definitions.
- **Inheritance**: Use inheritance to extend RimWorld classes, for instance, `Building_ExperimentBench : Building_WorkTable`.
- **Structured Projects**: Classes are organized logically, with `Experiment`, `Bill_Experiment`, and associated classes handling research experiments and related activities.

## XML Integration

- The mod extensively uses XML integration for defining game objects, research projects, and their properties. Ensure XML files align with RimWorld's expected schema and namespace.

## Harmony Patching

- **Harmony Patches**: Apply conditional Harmony patches for compatibility with other DLCs like Royalty and Ideology.
- **Use of Postfix**: Prefer Postfix methods over destructive Prefix patches to ensure compatibility with other mods, such as the Tech Advancing mod.

## Suggestions for Copilot

- **XML Definitions**: Assist in generating and modifying XML files that define items, buildings, experiments, and jobs.
- **Experiment System**: In creating new experiment types or adding experiments to the existing structure, suggest improvements in class methods related to `ExperimentStack` and `Bill_Experiment`.
- **UI Elements**: Guide in developing UI elements, such as experiment configuration dialogs and research listing interfaces.
- **Harmony Patches**: Recommend safe and effective usage of Harmony patches, ensuring compatibility with the base game and other mods.

## Compatibility and Future Enhancements

- **Incompatibilities**: The mod is currently incompatible with ResearchPal and other mods that drastically change the vanilla Research window.
- **Compatibility Goal**: Future updates aim to address save compatibility and resolve mod conflicts.
- **In Progress Features**: Datadisks from quests, additional flavor text, mod settings menu, improved art, and bug fixes.

For a smoother development experience, it is recommended to test changes with a minimal set of active mods, report any issues to the GitHub repository or the Discord channel, and utilize the RimSort tool for mod sorting to ensure optimal compatibility with existing mods.

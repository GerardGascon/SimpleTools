# Simple Tools

This package contains simple tools to use in your project.

This package will be updated once I find another useful tool or someone suggest me one.

## Features

- **AudioManager** with Play, Pause and most of the other basic things, as well as some effects like FadeIn or FadeOut.
- Some Cinemachine tools for making a **camera trigger** and an easy way for creating a **screen shake camera.**
- Basic **dialogue system** that works with TextMeshPro.
- Basic menu with **music and SFX sliders** as well as **resolution and quality dropdowns.**
- An **object pooler** with the ability to create pools with an undetermined size.
- A basic **scene manager** with a loading screen with progress bar.

All of that comes with some editor menu items for creating all of that as fast as possible.

## How to install

First install the TextMeshPro and Cinemachine into your Unity project

### Git Installation (Best way to get latest version)

If you have git in your computer, you can open Package Manager inside Unity, select "Add package from Git url...", and paste link [https://github.com/GerardGascon/SimpleTools.git](https://github.com/GerardGascon/SimpleTools.git)

or

Open the manifest.json file of your Unity project. Add "com.geri.simpletools": "[https://github.com/GerardGascon/SimpleTools.git](https://github.com/GerardGascon/SimpleTools.git)"

### Manual Installation

Download latest package from the Release section Import SimpleTools.unitypackage to your Unity Project

## Usage

### AudioManager

```csharp
using SimpleTools.AudioManager;

AudioManager.instance.Play("Name"); //Plays the sound with that name
AudioManager.instance.Play("Name", 1f); //Starts playing the sound "Name" in 1 second
AudioManager.instance.PlayOneShot("Name"); //Plays one shot of that sound (Useful for repeated sounds)
AudioManager.instance.PlayWithIntro("Intro", "Loop"); //Plays the intro and then the loop

AudioManager.instance.Pause("Name"); //Pauses the sound
AudioManager.instance.UnPause("Name"); //Unpauses the sound

AudioManager.instance.Stop("Name"); //Stops the sound
AudioManager.instance.StopAll(); //Stops all the sounds that are being played

AudioManager.instance.GetSource("Name"); //Gets the AudioSource with that name

AudioManager.instance.FadeIn("Name", 1f); //Fade In the source with a specific duration
AudioManager.instance.FadeOut("Name", 1f); //Fade Out the source with a specific duration

AudioManager.instance.PlayMuted("Name"); //Play a sound muted
AudioManager.instance.FadeMutedIn("Name", 1f); //Fade In a muted sound with a specific duration
AudioManager.instance.FadeMutedOut("Name", 1f); //Fade Out a sound without stopping it
```

### ObjectPooler

The SpawnFromPool function always return a GameObject

```csharp
using SimpleTools.ObjectPooler;

Pool pool; //The pool scriptable object goes here
Pooler.CreatePools(pool); //Create the pool, without creating it you cannot spawn it
Pool[] pools;
Pooler.CreatePools(pools); //Create multiple pools
Pooler.Destroy(gameObject); //Destroys a GameObject and returns it into the pool scene

Pooler.SpawnFromPool("Name", Vector3.zero); //Spawns an object into a specific position
Pooler.SpawnFromPool("Name", Vector3.zero, Quaternion.identity); //Spawn into a specific position and rotation
Pooler.SpawnFromPool("Name", Vector3.zero, transform); //Spawn into a specific position and parent
Pooler.SpawnFromPool("Name", Vector3.zero, Quaternion.identity, transform); //Spawn into a specific position, rotation and parent
Pooler.SpawnFromPool("Name", Vector3.zero, transform, true); //Spawn into a specific position, parent and instantiate in worldSpace or not
Pooler.SpawnFromPool("Name", Vector3.zero, Quaternion.identity, transform, true); //Spawn into a specific position, rotation, parent and instantiate in worldSpace or not
```

### Dialogue System

The Dialogue function returns a bool (true if it's talking, false if it has ended)

```csharp
using SimpleTools.DialogueSystem;

Dialogue dialogue; //The dialogue scriptable object goes here
DialogueSystem.instance.Dialogue(dialogue); //Start/Continue the dialogue
DialogueSystem.instance.Dialogue(dialogue, "Sound1", "Sound2"); //Start/Continue the dialogue with a random set of sounds for the text reveal
```

Text commands:

```html
<color=color></color> --> Sets font color within tags
<size=percentage></size> --> Sets font size within tags
<sprite=index> --> Draws a sprite from the TextMeshPro
<p:[tiny,short,normal,long,read]> --> Pauses during a period of time
<anim:[wobble,wave,rainbow,shake]></anim> --> Reproduces an animation
<sp:number></sp> --> Changes reveal speed
<snd:soundname> --> Plays a sound from the Audio Manager
```

### SceneManager

```csharp
using SimpleTools.SceneManagement;

Loader.Load(0); //Loads a scene with a specific build index
Loader.Load("Scene"); //Loads a scene with a specific name
```

### ScreenShake

```csharp
using SimpleTools.Cinemachine;

ScreenShake.Shake(1f, .25f); //Shakes the camera with an intensity and duration
```

### Timer

```csharp
using SimpleTools.Timer;

//Setup a stopwatch that updates at an unscaled time
Timer timer = textMeshProText.SetupTimer(TimerType.Stopwatch, TimerUpdate.UnscaledTime);
//Setup a clock
Timer timer = textMeshProText.SetupTimer(TimerType.Clock, TimerUpdate.UnscaledTime);
//Setup a countdown with the default time of 60 seconds
Timer timer = textMeshProText.SetupTimer(TimerType.Countdown, TimerUpdate.UnscaledTime, 60f);

timer.Play(); //Play or resume the timer
timer.Stop(); //Pause the timer
timer.ResetTimer(); //Pause and sets the time to the default one
timer.Restart(); //Restarts the timer
```

### Editor

You can easily set up some things by right clicking in your Project Tab and then selecting Tools and clicking on the one you want to create.

Also you can right click in the Hierarchy for easily creating some GameObjects with the Tools in it.
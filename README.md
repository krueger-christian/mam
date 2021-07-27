# Morph A Mood (Unity Asset)
**Morph A Mood (MAM) is a pictographic scale for rapid assessment of affective states.**

1. Introduction
2. Some further information
3. Installation
    1. Desktop environment
    2. Oculus VR
4. Computational Models
5. Acknowledgement


## Introduction

Morph A Mood (MAM) is a continuous pictographic scale for rapid assessment of affective states. It was developed to be used in virtual reality, but it can be also used in other environments.
MAM consists basically of a figure whose facial expression the user can adjust, when asked about his or her feeling state, so that it represents the state. This can be operated with a controller gesture using a VR toolkit or, in desktop environments, with a mouse. The use of touch screen devices would also be possible; this is currently being researched at the TU Berlin and will probably be extended to this asset in the future, but is not yet considered in the current state of this Unity package.  
Adjusting the facial expression of the character leads to a computation of a valence and an arousal value. These are reported to a csv file, stored in the application data path.

</br><img src="img/mam.gif" width="300px"></br></br>


## Installation

Import the *Morph_A_Mood* Unity package into your project. From the top menu, go to *Assets* &rarr; *Import Package* &rarr; *Custom Package*. The *Morph A Mood* asset uses the *TextMesh Pro* package. If it has not already been imported via the Package Manager, you will be automatically prompted to do so (as checked on Unity version 2019.3.3f1), at the latest when you load one of the example scenes or place the *MorphAMood* prefab in your custom scene. So drag the *MorphAMood* prefab into the hierarchy window of your scene (or load one of the example scenes from the *Scenes* directory). When you are asked to load the *TextMesh Pro* package, which you should confirm, it may happen, that the text of the *MorphAMood* interface may be displayed much too large. Just reload the scene or drag the prefab into your scene again and the text should be displayed in normal size.

<img src="img/000.jpg" width="300px"></br></br>

Attached to the prefab *MorphAMood* you will find the script *MorphAMoodCtrl*. The following options are available:  

**Playground Mode** allows you to play around with the character and get familiar with the tool without saving data or quitting by pressing the confirmation button.  

**Event On End** allows to define what happens after the user has confirmed the adjusted expression. You can choose between no event (NONE), defining a callback routine (CALLBACK) or a scene to be loaded afterwards (LOAD_SCENE). Depending on what you choose you must fill in the corresponding field below. This is an exclusive option. It is not possible to load a scene and execute a callback.  

With **Cursor Speed** you can set how sensitive the dot cursor laid over the character reacts on movements of the control device.  

**Report ID** allows to define an identifier of what has been rated. It will be associated with all reported values. This ID may need to be updated automatically when different ratings are performed. You can change this public variable by scripting.

For **saving reports** the script *Report* is included. By default, the filepath is the [application data path](https://docs.unity3d.com/ScriptReference/Application-dataPath.html). On desktop environments you can define your own filepath. Defining a filename is optional. The default filename is the prefix "mam_report_" followed by a timestamp.  

<img src="img/001.jpg" width="300px"></br></br>


#### Desktop environment

On desktop environments you should be able to start after checking the previously mentioned setup. By default, left mouse click is used to enable editing of the character's facial expression. Moving the mouse or stroking the touchpad while holding down the left button will cause changes. Right-clicking (two finger click on macOS) confirms the change and ends the rating.


#### Oculus VR

Import the *Oculus Integration* asset from the Unity Asset Store, go to the top menu: *Window* &rarr; *Asset Store*. It is assumed that you are familiar with the Oculus SDK. You will have to add the player controller by yourself.

To use MAM on Oculus VR kits, you should also consider the previously mentioned setup. By default the primary trigger finger button is used to enable editing of the character's facial expression. Moving the primary controller while keeping trigger button pressed will result in changes. Clicking the touchpad button confirms the change and ends the rating.

By default the scripts are compiled without regard to the Oculus SDK. **To apply the Oculus controller to MAM**, go to the top menu: *Edit* &rarr; *MorphAMood* &rarr; *Enable Oculus SDK*. This will set the preprocessor directive *#define OCULUS* in some scripts. It may take some seconds until the Unity editor detects the changes in the scripts. To speed up the update, simply switch to another application and go back to the Unity editor.


## Some further information

You might ask: what is a *pictographic scale*? What is an *affective state*? And why assess affective states with pictographic scales?  

**Pictographic Scales**  
Well known are scales with alphanumeric text labels – either the ticks labeled with pure numbers as on a temperature scale or with words like "agree" and "disagree". With pictographic scales, the symbols that mark the scale divisions are pictograms, sometimes additionally labeled with numbers or words, but a scale can also be represented by a selection of pictograms only.  

**Affective state**  
is a generic term for both positive and negative feelings [1, p. 216], this covers both emotions and moods. These differ in duration and in the attribution to causes of the feeling. In theory, an emotion is a short-term, directed feeling, whereas a mood is a longterm, undirected feeling. Affective states can be described with dimensional models. Commonly used dimensions are the valence and arousal measure [2-4]. An affective state can be measured by assessing the degree of pleasure (valence) and the degree of activation (arousal).  

To **assess an affective state with a pictographic scale** the user is asked to choose a pictographic symbol, that represents his/her feeling. The pictograms can be translated into numeric values for the purpose of statistical analysis. An example of a pictographic tool is [Pick-A-Mood](https://diopd.org/pick-a-mood/) (PAM) [2], that asks you to choose a representative cartoon out of a set of nine different expressions. Each cartoon is encoded with a valence and an arousal value that have been obtained as average values during a validation experiment, where participants assessed a valence and an arousal value on a 5 point scale for each cartoon. These associated values place each cartoon in a two-dimensional valence-arousal space. The limitation is the discreteness of this scale. The special aspect of MAM is its continuous pictographic scale. It is achieved through interpolation of also nine basic expressions, the same one used in PAM: *calm, relaxed, cheerful, excited, irritated, tense, sad, bored and neutral*. Interpolation allows to define states between these basic expressions as well as between the valence and arousal values of these. The computation is described in the section *Computational Models*.
Furthermore the 3D character allows a new degree of experience for graphical questionnaires in virtual environments (VEs). VEs displayed on head-mounted displays (HMD) differ from "Window-on-the-world" systems such as desktop computers or mobile phones by tracking the user's position and orientation and by a stereoscopic view. This allows immersive experiences that are distinctive for VEs. The use of a 3D cartoon in MAM approaches these specific graphical characteristics of HMD based VEs.


In order to **assess an affective state with a pictographic scale**, the user is asked to choose a pictographic symbol that represents his or her feeling. The pictograms can be translated into numerical values for the purpose of statistical analysis. An example of a pictographic tool is [Pick-A-Mood](https://diopd.org/pick-a-mood/) (PAM) [2], where the user is asked to choose a representative cartoon from a set of nine different expressions. Each cartoon is coded with a valence and an excitation value obtained as averages during a validation experiment in which the participants rated each cartoon with a valence and an arousal value on a 5-point scale. These associated values place each cartoon in a two-dimensional valence and arousal space. The limitation lies in the discreteness of this scale. The special feature of MAM is the continuous pictographic scale. It is achieved by interpolating nine basic expressions, also used in PAM: calm, relaxed, happy, excited, irritated, tense, sad, bored and neutral. The interpolation allows to define states between these basic expressions and between their valence and excitation values. The calculation is described in the section Computational models. Furthermore, the 3D character allows a new level of experience for graphical questionnaires in virtual environments (VEs). VEs displayed on Head Mounted Displays (HMD) differ from "window-on-the-world" systems [5] such as desktop computers or mobile phones by tracking the position and orientation of the user and by providing a stereoscopic view. This allows immersive experiences that are distinctive for VEs. The use of a 3D cartoon in MAM is an approach to take into account these specific graphical features of HMD-based VEs.

The validation of the tool is described in [6], the data of the validation experiment is available on [mam.christiankruger.de](http://mam.christiankruger.de).

<sub>
[1] Eder, A. and Brosch, T. (2017). Emotion. In Müsseler, J. and Rieger,M. (eds.). Allgemeine Psychologie. Berlin, Heidelberg: Springer, pp. 185–222.
</sub>
</br></br>
<sub>
[2] Desmet, P.M. A., Vastenburg,M. H. and Romero, N. (2016a). Mood measurement with Pick-A-Mood: review of current methods and design of a pictorial self-report scale. J. Design Research, 14 (3), pp. 241–279.  
</sub>
</br></br>
<sub>
[3] Bradley, M. M. and Lang, P. J. ( 1994). Measuring emotion:The self-assessment manikin and the semantic differential. Journal of Behavior Therapy and Experimental Psychiatry, 25 (1), pp. 49–59.  
</sub>
</br></br>
<sub>
[4] Russell, J. A. (2003). Core affect and the psychological construction of emotion. Psychological Review, 110 (1), pp. 145–172.</sub>
</sub>
</br></br>
<sub>
[5] Milgram, P., Takemura, H., Utsumi, A. and Kishino, F. (1995). Augmented reality: a class of displays on the reality-virtuality continuum. in Das, H. (ed.), Telemanipulator and Telepresence Technologies, SPIE, pp. 282–292.
</sub>
</br></br>
<sub>
[6] Krüger, C., Kojic, T., Maier, L., Möller, S. and Voigt-Antons, J.-N. (2020). Development and validation of pictographic scales for rapid assessment of affective states in virtual reality. 2020 Twelfth International Conference on Quality of Multimedia Experience (QoMEX), Athlone, Ireland, IEEE, pp. 1-6, DOI: 10.1109/QoMEX48832.2020.9123100
</sub>


## Computational Models

As you can see in the drop down menu *Mapping* of the *MorphAMoodCtrl* script, there are two optional models, *POLAR* and *CARTESIAN*, which are used to interpolate the custom expression based on the defined key expressions. By default, *POLAR* is selected because it was used during the validation experiment of this tool. A general introduction into the interpolation process and a description of the two models is given in the following subsections.

#### The basic idea of expression interpolation

The facial model is defined by partitions like mouth, eyelids, eyebrows. In general, an expression is characterized e.g. by a certain angle of the mouth or a certain position of the eyebrows. This means that every partition can transform its geometry according to the specific facial expression. In the polygon mesh used to model the face, the vertices define the geometry of the partitions. Therefore each defined basic expression is accompanied by a specific constellation of the vertices. Interpolating between two expressions means interpolating the vertex positions of two specific constellations of vertices.  
Each basic expression has a feature vector containing parameters for controlling the vertex groups of the facial partitions (mouth, eyelids, …). Furthermore, it contains a valence and an arousal value. The Interpolation of a new expression is not only the computation of positions between the vertices, but also of valence and arousal values between them. The feature vector contains 24 geometry parameters. So how to interpolate in a 24-dimensional space? This is done by a mapping a two-dimensional space to the 24-dimensional space (or 26-dimensional if valence and arousal parameters are included). The valence-arousal space is a two-dimensional map in which the expressions can be placed based on their associated valence and arousal values. To select a user-defined expression, you can place a cursor in this two-dimensional map. The cursor has (variable) coordinates, just as each basic expression has (constant) coordinates. Based on the distances between the cursor position to the positions of the neighbouring basic expressions, ratios can be computed, to generate a custom feature vector for the cursor position by summing the feature vectors of the neighbouring expressions weighted by their distance ratios. The geometry parameters of the mixed (interpolated) feature vector can be used to render the face model, and the valence and arousal values contained in it provide a reportable measure.

#### Cartesian Computation Model

One way to map the basic expressions on a two-dimensional space is to use the existing two-dimensional valence-arousal space. The expressions are per se located on this map by their predefined valence and arousal values. Since interpolation within triangles is relatively simple, a triangle mesh is created between the location vectors of the basic expressions.  In this computational model, feature vector interpolation takes into account the triangle in which the cursor is located. Each vertex of a triangle represents one expression. The interpolation of the feature vector of the cursor is simply the sum of the weighted feature vectors associated with the constant vertices of the triangles (which represent the basic expressions). The weights are simply the ratios of the areas subdivided by the cursor point to the area of the total triangle. A valence and arousal value need not be interpolated in this model because the cursor coordinates are identical to them. This is an advantage.
A disadvantage is that if basic expressions in the valence-arousal space are very close to each other, the interpolation between them causes changes in the facial expression to appear faster and more abrupt. The polar computational model approaches a more fluid morphing behaviour.


<img src="img/cartesian_map.svg" width="350px"></br></br>

#### Polar Computation Model
Whereas the Cartesian computation model described in the above section works with the dimensions *valence* and *arousal*, the Polar computation model uses a transformation of these dimensions, with the range per axis being defined between -1 and 1. In the Cartesian model, a triangle mesh works as a grid system. In the Polar model, a circular grid with two rings is placed around the coordinate origin with a radius of 0.5 and 1. The ring is partitioned with equal angular segments. When visually inspecting the Cartesian map and comparing the basic expressions, a ring-like structure can already be seen. There is a group of four expressions within another group of also four expressions. In some cases, an expression inside the inner ring seems to be a mild version of the closest expression outside the ring, such as *cheerful* and *excited* or *calm* and *relaxed*. This observation has led to the simplification of the polar map, which associates equal angles (in polar coordinates) with related expressions located in the same quadrant of the valence-arousal space.
In the polar map, the vertices of the expressions are approximately equidistant, which leads to a more fluid morphing behaviour, since the gradual changes during the interpolation of the basic expressions are similar. Also the recognition of the cell in which the cursor is located is much easier, because the indices of the vertices can be calculated directly by the angle, whereas in the triangle mesh the recognition process has to iterate over the triangles.  
Since the polar map works on dimensions that are not equal to the valence-arousal space, the valence and arousal values of the considered basic expressions are also interpolated.

<img src="img/polar_map.svg" width="400px"></br></br>

## Acknowledgement

Many thanks to Dr.-Ing. Jan-Niklas Voigt-Antons, Tanja Kojic, Martin Josef Burghardt, Alina
Dorsch, Katharina Erben, Christof Grumpelt, Christopher Hyna, Lena Manger, Urszula Przybylska, Tina Schüler and Peter Schwartz for the discussions. In addition, Natalie Romero and her team for providing the PAM cartoons as well as the DEAP research team for granting access to their database, which both, cartoons and database, were essential for the development and the validation experiment.
Finally, many thanks to all the volunteers who took part in the online study and the experiment, and to all the author's friends and family who helped to recruit test subjects.

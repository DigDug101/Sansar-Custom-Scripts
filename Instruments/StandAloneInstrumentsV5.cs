﻿//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

#define SansarBuild
#define InstrumentBuild

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;

public class StandAloneInstV5 : SceneObjectScript

{
    #region ConstantsVariables
    [DefaultValue("Click Me!")]
    public Interaction InstrumentInteraction;
    private int Clicks = 0;
    private float[] NoteHitX1Array = new float[10];
    private float[] NoteHitY1Array = new float[10];
    private float[] NoteHitX2Array = new float[10];
    private float[] NoteHitY2Array = new float[10];
    private float[] ControlSurfaceXRelative = new float[10];
    private float[] ControlSurfaceYRelative = new float[10];
    private float[] ControlSurfaceRadiusArray = new float[10];
    private float[] ControlSurfaceZMinimum = new float[10];
    private float[] ControlSurfaceZMaximum = new float[10];
    private float[] ControlSurfaceXRelativeAfterRotation = new float[10];
    private float[] ControlSurfaceYRelativeAfterRotation = new float[10];

    private float[] NoteHitZ1Array = new float[10];
    private float[] NoteHitZ2Array = new float[10];
    private string[] InstrumentNotes = new string[10];
    private int NumOfNotes = 0;
    bool loopNote = true;
    private string[] NoteArray = new string[100];
    private string[] TimingArray = new string[100];
    private List<int> intNoteArray = new List<int>();
    private List<float> fltTimingArray = new List<float>();
    private int NumOfEntries = 0;
    private int CheckNote = 0;
    private int CheckNoteSend = 0;
    public string ModalMessage;
    private AgentPrivate Hitman;
    private SendNoteMatches sendNoteMatches = new SendNoteMatches();
    private SendAttaBoys sendAttaBoys = new SendAttaBoys();
    private string preview = "Y";
    private bool BPMLoaded = false;
    private bool SamplesLoaded = false;
    private bool InstrumentLoaded = false;
    private bool NotePositionsLoaded = false;
    private bool MelodyLoaded = false;
    public struct Point
    {
        public float X, Y;
    }
    private Point[] Points = new Point[4];
    private Point[,] PointsArray = new Point[10,4]; 

    public string startNote = null;
    public string scaleIn = null;
    public string StandAloneInstrument = "piano";
    public string WelcomeMessage = "Welcome to the Interactive Grand Piano";
    public Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);
    public double ZRotation = new double();
    private double[] NotePositionRadius = new double[10];
    private int NumNotePositions = 0;

    private const int c0 = 12; const int db0 = 13; const int d0 = 14; const int eb0 = 15; const int e0 = 16; const int f0 = 17; const int gb0 = 18; const int g0 = 19; const int ab0 = 20; const int a0 = 21; const int bb0 = 22; const int b0 = 23;
    private const int c1 = 24; const int db1 = 25; const int d1 = 26; const int eb1 = 27; const int e1 = 28; const int f1 = 29; const int gb1 = 30; const int g1 = 31; const int ab1 = 32; const int a1 = 33; const int bb1 = 34; const int b1 = 35;
    private const int c2 = 36; const int db2 = 37; const int d2 = 38; const int eb2 = 39; const int e2 = 40; const int f2 = 41; const int gb2 = 52; const int g2 = 43; const int ab2 = 44; const int a2 = 45; const int bb2 = 46; const int b2 = 47;
    private const int c3 = 48; const int db3 = 49; const int d3 = 50; const int eb3 = 51; const int e3 = 52; const int f3 = 53; const int gb3 = 54; const int g3 = 55; const int ab3 = 56; const int a3 = 57; const int bb3 = 58; const int b3 = 59;
    private const int c4 = 60; const int db4 = 61; const int d4 = 62; const int eb4 = 63; const int e4 = 64; const int f4 = 65; const int gb4 = 66; const int g4 = 67; const int ab4 = 68; const int a4 = 69; const int bb4 = 70; const int b4 = 71;
    private const int c5 = 72; const int db5 = 73; const int d5 = 74; const int eb5 = 75; const int e5 = 76; const int f5 = 77; const int gb5 = 78; const int g5 = 79; const int ab5 = 80; const int a5 = 81; const int bb5 = 82; const int b5 = 83;
    private const int c6 = 84; const int db6 = 85; const int d6 = 86; const int eb6 = 87; const int e6 = 88; const int f6 = 89; const int gb6 = 90; const int g6 = 91; const int ab6 = 92; const int a6 = 93; const int bb6 = 94; const int b6 = 95;
    private const int c7 = 96; const int db7 = 97; const int d7 = 98; const int eb7 = 99; const int e7 = 100; const int f7 = 101; const int gb7 = 102; const int g7 = 103; const int ab7 = 104; const int a7 = 105; const int bb7 = 106; const int b7 = 107;
    private const int c8 = 108; const int db8 = 109; const int d8 = 110; const int eb8 = 111; const int e8 = 112; const int f8 = 113; const int gb8 = 114; const int g8 = 115; const int ab8 = 116; const int a8 = 117; const int bb8 = 118; const int b8 = 119;
    private const int c9 = 120; const int db9 = 121; const int d9 = 122; const int eb9 = 123; const int e9 = 124; const int f9 = 125; const int gb9 = 126; const int g9 = 127;

    private List<string> MidiNote = new List<string>();

    //scales
    private static int[] major = { 2, 2, 1, 2, 2, 2, 1 };
    private static int[] dorian = { 1, 2, 2, 1, 2, 2, 2 };
    private static int[] phrygian = { 2, 1, 2, 2, 1, 2, 2 };
    private static int[] lydian = { 2, 2, 1, 2, 2, 1, 2 };
    private static int[] mixolydian = { 2, 2, 2, 1, 2, 2, 1 };
    private static int[] aelian = { 1, 2, 2, 2, 1, 2, 2 };
    private static int[] minor = { 2, 1, 2, 2, 2, 1, 2 };
    private static int[] minor_pentatonic = { 3, 2, 2, 3, 2 };
    private static int[] major_pentatonic = { 2, 3, 2, 2, 3 };
    private static int[] egyptian = { 3, 2, 3, 2, 2 };
    private static int[] jiao = { 2, 3, 2, 3, 2 };
    private static int[] zhi = { 2, 2, 3, 2, 3 };
    private static int[] whole_tone = { 2, 2, 2, 2, 2, 2 };
    private static int[] whole = { 2, 2, 2, 2, 2, 2 };
    private static int[] chromatic = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    private static int[] harmonic_minor = { 2, 1, 2, 2, 1, 3, 1 };
    private static int[] melodic_minor_asc = { 2, 1, 2, 2, 2, 2, 1 };
    private static int[] hungarian_minor = { 2, 1, 3, 1, 1, 3, 1 };
    private static int[] octatonic = { 2, 1, 2, 1, 2, 1, 2, 1 };
    private static int[] messiaen1 = { 2, 2, 2, 2, 2, 2 };
    private static int[] messiaen2 = { 1, 2, 1, 2, 1, 2, 1, 2 };
    private static int[] messiaen3 = { 2, 1, 1, 2, 1, 1, 2, 1, 1 };
    private static int[] messiaen4 = { 1, 1, 3, 1, 1, 1, 3, 1 };
    private static int[] messiaen5 = { 1, 4, 1, 1, 4, 1 };
    private static int[] messiaen6 = { 2, 2, 1, 1, 2, 2, 1, 1 };
    private static int[] messiaen7 = { 1, 1, 1, 2, 1, 1, 1, 1, 2, 1 };
    private static int[] super_locrian = { 1, 2, 1, 2, 2, 2, 2 };
    private static int[] hirajoshi = { 2, 1, 4, 1, 4 };
    private static int[] kumoi = { 2, 1, 4, 2, 3 };
    private static int[] neapolitan_major = { 1, 2, 2, 2, 2, 2, 1 };
    private static int[] bartok = { 2, 2, 1, 2, 1, 2, 2 };
    private static int[] bhairav = { 1, 3, 1, 2, 1, 3, 1 };
    private static int[] locrian_major = { 2, 2, 1, 1, 2, 2, 2 };
    private static int[] ahirbhairav = { 1, 3, 1, 2, 2, 1, 2 };
    private static int[] enigmatic = { 1, 3, 2, 2, 2, 1, 1 };
    private static int[] neapolitan_minor = { 1, 2, 2, 2, 1, 3, 1 };
    private static int[] pelog = { 1, 2, 4, 1, 4 };
    private static int[] augmented2 = { 1, 3, 1, 3, 1, 3 };
    private static int[] scriabin = { 1, 3, 3, 2, 3 };
    private static int[] harmonic_major = { 2, 2, 1, 2, 1, 3, 1 };
    private static int[] melodic_minor_desc = { 2, 1, 2, 2, 1, 2, 2 };
    private static int[] romanian_minor = { 2, 1, 3, 1, 2, 1, 2 };
    private static int[] hindu = { 2, 2, 1, 2, 1, 2, 2 };
    private static int[] iwato = { 1, 4, 1, 4, 2 };
    private static int[] melodic_minor = { 2, 1, 2, 2, 2, 2, 1 };
    private static int[] diminished2 = { 2, 1, 2, 1, 2, 1, 2, 1 };
    private static int[] marva = { 1, 3, 2, 1, 2, 2, 1 };
    private static int[] melodic_major = { 2, 2, 1, 2, 1, 2, 2 };
    private static int[] indian = { 4, 1, 2, 3, 2 };
    private static int[] spanish = { 1, 3, 1, 2, 1, 2, 2 };
    private static int[] prometheus = { 2, 2, 2, 5, 1 };
    private static int[] diminished = { 1, 2, 1, 2, 1, 2, 1, 2 };
    private static int[] todi = { 1, 2, 3, 1, 1, 3, 1 };
    private static int[] leading_whole = { 2, 2, 2, 2, 2, 1, 1 };
    private static int[] augmented = { 3, 1, 3, 1, 3, 1 };
    private static int[] purvi = { 1, 3, 2, 1, 1, 3, 1 };
    private static int[] chinese = { 4, 2, 1, 4, 1 };
    //chords
    private static int[] chdmajor = { 0, 4, 7 };
    private static int[] chdminor = { 0, 3, 7 };
    private static int[] chdmajor7 = { 0, 4, 7, 11 };
    private static int[] chddom7 = { 0, 4, 7, 10 };
    private static int[] chdminor7 = { 0, 3, 7, 10 };
    private static int[] chdaug = { 0, 4, 8 };
    private static int[] chddim = { 0, 3, 6 };
    private static int[] chddim7 = { 0, 3, 6, 9 };
    private static int[] chdhalfdim = { 0, 3, 6, 10 };
    private static int[] chd1 = { 0 };
    private static int[] chd5 = { 0, 7 };
    private static int[] chdmaug5 = { 0, 3, 8 };
    private static int[] chdsus2 = { 0, 2, 7 };
    private static int[] chdsus4 = { 0, 5, 7 };
    private static int[] chd6 = { 0, 4, 7, 9 };
    private static int[] chdm6 = { 0, 3, 7, 9 };
    private static int[] chd7sus2 = { 0, 2, 7, 10 };
    private static int[] chd7sus4 = { 0, 5, 7, 10 };
    private static int[] chd7dim5 = { 0, 4, 6, 10 };
    private static int[] chd7aug5 = { 0, 4, 8, 10 };
    private static int[] chdm7aug5 = { 0, 3, 8, 10 };
    private static int[] chd9 = { 0, 4, 7, 10, 14 };
    private static int[] chdm9 = { 0, 3, 7, 10, 14 };
    private static int[] chdm7aug9 = { 0, 3, 7, 10, 14 };
    private static int[] chdmaj9 = { 0, 4, 7, 11, 14 };
    private static int[] chd9sus4 = { 0, 5, 7, 10, 14 };
    private static int[] chd6sus9 = { 0, 4, 7, 9, 14 };
    private static int[] chdm6sus9 = { 0, 3, 9, 7, 14 };
    private static int[] chd7dim9 = { 0, 4, 7, 10, 13 };
    private static int[] chdm7dim9 = { 0, 3, 7, 10, 13 };
    private static int[] chd7dim10 = { 0, 4, 7, 10, 15 };
    private static int[] chd7dim11 = { 0, 4, 7, 10, 16 };
    private static int[] chd7dim13 = { 0, 4, 7, 10, 20 };
    private static int[] chd9dim5 = { 0, 10, 13 };
    private static int[] chdm9dim5 = { 0, 10, 14 };
    private static int[] chd7aug5dim9 = { 0, 4, 8, 10, 13 };
    private static int[] chdm7aug5dim9 = { 0, 3, 8, 10, 13 };
    private static int[] chd11 = { 0, 4, 7, 10, 14, 17 };
    private static int[] chdm11 = { 0, 3, 7, 10, 14, 17 };
    private static int[] chdmaj11 = { 0, 4, 7, 11, 14, 17 };
    private static int[] chd11aug = { 0, 4, 7, 10, 14, 18 };
    private static int[] chdm11aug = { 0, 3, 7, 10, 14, 18 };
    private static int[] chd13 = { 0, 4, 7, 10, 14, 17, 21 };
    private static int[] chdm13 = { 0, 3, 7, 10, 14, 17, 21 };
    private static int[] chdadd2 = { 0, 2, 4, 7 };
    private static int[] chdadd4 = { 0, 4, 5, 7 };
    private static int[] chdadd9 = { 0, 4, 7, 14 };
    private static int[] chdadd11 = { 0, 4, 7, 17 };
    private static int[] add13 = { 0, 4, 7, 21 };
    private static int[] madd2 = { 0, 2, 3, 7 };
    private static int[] madd4 = { 0, 3, 5, 7 };
    private static int[] madd9 = { 0, 3, 7, 14 };
    private static int[] madd11 = { 0, 3, 7, 17 };
    private static int[] madd13 = { 0, 3, 7, 21 };

    private static string[] validNotes = { "c0", "db0", "d0", "eb0", "e0", "f0", "gb0", "g0", "ab0", "a0", "bb0", "b0", "c1", "db1", "d1", "eb1", "e1", "f1", "gb1", "g1", "ab1", "a1", "bb1", "b1", "c2", "db2", "d2", "eb2", "e2", "f2", "gb2", "g2", "ab2", "a2", "bb2", "b2", "c3", "db3", "d3", "eb3", "e3", "f3", "gb3", "g3", "ab3", "a3", "bb3", "b3", "c4", "db4", "d4", "eb4", "e4", "f4", "gb4", "g4", "ab4", "a4", "bb4", "b4", "c5", "db5", "d5", "eb5", "e5", "f5", "gb5", "g5", "ab5", "a5", "bb5", "b5", "c6", "db6", "d6", "eb6", "e6", "f6", "gb6", "g6", "ab6", "a6", "bb6", "b6", "c7", "db7", "d7", "eb7", "e7", "f7", "gb7", "g7", "ab7", "a7", "bb7", "b7", "c8", "db8", "d8", "eb8", "e8", "f8", "gb8", "g8", "ab8", "a8", "bb8", "b8", "c9", "db9", "d9", "eb9", "e9", "f9", "gb9", "g9" };
    private string Errors = "Errors: ";
    private string Errormsg = "No Errors";
    private bool strErrors = false;

    private List<Vector> SoundPos = new List<Vector>();
    private int instrumentcntr = 0;
    private List<string> InstrumentName = new List<string>();
    private const char s = 's';  //sample
    private const char b = 'b';  //beat
    private const char m = 'm';  //multibeat
    private const char i = 'i';  //instrument

    private int loopNum = 0;
    private const int numTracks = 9;
    private List<SoundResource>[] TrackSamples = new List<SoundResource>[numTracks];
    private List<int>[] TrackOffsets = new List<int>[numTracks];
    private List<float>[] TrackMilliSeconds = new List<float>[numTracks];
    private List<char>[] TrackSequence = new List<char>[numTracks];
    private List<int>[] TrackNotes = new List<int>[numTracks];
    private string[] TrackArrayAccess = new string[numTracks]; //"ring";
    private bool[] TrackRunning = new bool[numTracks];
    private bool[] TrackStop = new bool[numTracks];
    private float[] TrackVolume = new float[numTracks];  //0f;
    private float[] TrackPitchShift = new float[numTracks];  //0f;
    private bool[] TrackPlay_Once = new bool[numTracks];  //true;
    private bool[] TrackDont_Sync = new bool[numTracks];  //true;
    private PlayHandle[] playHandle = new PlayHandle[127];
    private PlaySettings[] playSettings = new PlaySettings[127];

    private SessionId Jammer = new SessionId();

    AgentPrivate TheUser = null;
    List<IEventSubscription> ButtonSubscriptions = new List<IEventSubscription>();
    private bool Shifted = false;
    int bpm = 100;

    #endregion

    public override void Init()
    {
        Script.UnhandledException += UnhandledException; // Catch errors and keep running unless fatal
        RigidBodyComponent rigidBody;
        sendNoteMatches.NoteMatch = new List<string>();
        sendAttaBoys.AttaBoy = new List<string>();
        string eventString = null;
        int samplepackcntr = 1;
        do
        {
            eventString = "Samples" + samplepackcntr;
            SubscribeToScriptEvent(eventString, getSamples);
            eventString = "Instrument" + samplepackcntr;
            SubscribeToScriptEvent(eventString, getInstrument);
            samplepackcntr++;
        } while (samplepackcntr < 64);

        SubscribeToScriptEvent("NotePositions", getNotePositions);

        SubscribeToScriptEvent("CollisionData", getCollisionData);

        SubscribeToScriptEvent("Melody", getMelodys);

        if (ObjectPrivate.TryGetFirstComponent(out rigidBody))
        // && rigidBody.IsTriggerVolume())
        {
            //CurPos = rigidBody.GetPosition();
        }
        else
        {
        }
    }

    private void UnhandledException(object Sender, Exception Ex)
    {
        Log.Write(LogLevel.Error, GetType().Name, Ex.Message + "\n" + Ex.StackTrace + "\n" + Ex.Source);
        return;
    }//UnhandledException

    #region Communication

    public interface SendsCollisionData
    {
        CollisionData SentCollisionData { get; }
    }

    private void getCollisionData(ScriptEventData gotCollisionData)
    {
        Log.Write("In getCollisions");
        if (gotCollisionData.Data == null)
        {
            return;
        }
        SendsCollisionData sendsCollisionData = gotCollisionData.Data.AsInterface<SendsCollisionData>();
        if (sendsCollisionData == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }
        CollisionData Data = sendsCollisionData.SentCollisionData;
        AgentPrivate hit = ScenePrivate.FindAgent(Data.HitComponentId.ObjectId);
        Hitman = hit;
        if (Data.Phase == CollisionEventPhase.TriggerEnter)
        {
            ScenePrivate.Chat.MessageAllUsers(WelcomeMessage);

            Log.Write("user: " + hit.ToString());
            //Log.Write("client: " + hit.Client);
            //Log.Write("After Hit");
            ScenePrivate.Chat.Subscribe(0, "user", hit.AgentInfo.SessionId, GetChatCommand);
            //Log.Write("After chat subscribe");
            //SubscribeKeyPressed(hit, "sub");
            SubscribeKeyPressed(hit.Client, "sub");
            //Log.Write("after keypressed subscribe");
            SubscribeToScriptEvent("BPMBlock", getBPM);  // to automatically change pitch
            CheckNote = 0;
            InitalizeDefaults();
            GameReset();
        }
        else
        {
            Log.Write("has left my volume!");
            SubscribeKeyPressed(hit.Client, "unsub");
        }

        while (!SamplesLoaded)
        {
            Wait(TimeSpan.FromMilliseconds(10));
        }
        while (!InstrumentLoaded)
        {
            Wait(TimeSpan.FromMilliseconds(10));
        }
        while (!NotePositionsLoaded)
        {
            Wait(TimeSpan.FromMilliseconds(10));
        }
        while (!MelodyLoaded)
        {
            Wait(TimeSpan.FromMilliseconds(10));
        }
        PlayMelody();

    }

    private void Test()
    {
        Log.Write("Test");
    }

    public List<string> Melodys = new List<string>();

    public interface SendMelodys
    {
        List<string> SendMelody { get; }
    }

    private void getMelodys(ScriptEventData gotMelodys)
    {
        Log.Write("In getMelody");
        if (gotMelodys.Data == null)
        {
            Log.Write(LogLevel.Warning, Script.ID.ToString(), "Expected non-null event data");
            return;
        }
        SendMelodys sendMelodys = gotMelodys.Data.AsInterface<SendMelodys>();
        if (sendMelodys == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        //Log.Write("BPM: " + sendMelodys.SendMelody.ElementAt(0));
        //Log.Write("Notes: " + sendMelodys.SendMelody.ElementAt(1));
        //Log.Write("Timing: " + sendMelodys.SendMelody.ElementAt(2));
        string BPMString = sendMelodys.SendMelody.ElementAt(0);
        bpm = Int32.Parse(BPMString);
        string NotesString = sendMelodys.SendMelody.ElementAt(1);
        NoteArray = NotesString.Split(',');
        string TimingString = sendMelodys.SendMelody.ElementAt(2);
        TimingArray = TimingString.Split(',');
        int cntr = 0;
        NumOfEntries = NotesString.Split(',').Length;
        Log.Write("NumOfEntries: " + NumOfEntries);
        intNoteArray.Clear();
        fltTimingArray.Clear();
        if (NoteArray.Count() > 0)
        {
            do
            {
                intNoteArray.Add(Int32.Parse(NoteArray[cntr]));
                fltTimingArray.Add(float.Parse(TimingArray[cntr]) / bpm * 60 * 1000);
                cntr++;
            } while (cntr < NumOfEntries);
        }
        //Log.Write("Calling PlayMelody from getMelody");
        MelodyLoaded = true;

    }

    public List<string> NotePositions = new List<string>();

    public interface SendNotePositions
    {
        List<string> SendNotePosition { get; }
    }

    private void getNotePositions(ScriptEventData gotNotePositions)
    {
        Log.Write("In getNotePositions");
        if (gotNotePositions.Data == null)
        {
            Log.Write(LogLevel.Warning, Script.ID.ToString(), "Expected non-null event data");
            return;
        }
        SendNotePositions sendNotePositions = gotNotePositions.Data.AsInterface<SendNotePositions>();
        if (sendNotePositions == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        Log.Write("CurPos: " + CurPos);
        float CurPosX = CurPos.X;
        float CurPosY = CurPos.Y;
        Log.Write("ZRotation: " + ZRotation);
        double ZRad = ZRotation * 0.0174533;
        string tempNotePosition;
        string[] values = new string[100];
        int cntr = 0;
        //Log.Write("sendSamples.SendSampleLibrary.Count(): " + sendSamples.SendSampleLibrary.Count());
        //Log.Write("sendNotePositions.SendNotePosition.Count(): " + sendNotePositions.SendNotePosition.Count());
        NumOfNotes = sendNotePositions.SendNotePosition.Count();
        if (sendNotePositions.SendNotePosition.Count() > 0)
        {
            do
            {
                tempNotePosition = sendNotePositions.SendNotePosition.ElementAt(cntr);
                Log.Write("tempNotePositions: " + tempNotePosition);
                LoadControlSurfaces(tempNotePosition, cntr);
                cntr++;
            } while (cntr < sendNotePositions.SendNotePosition.Count());
            NumNotePositions = cntr;
            //Log.Write("NumNotePositions: " + NumNotePositions);
            //FindRotatedPositions(NumNotePositions, NoteHitX1Array, NoteHitX2Array, NoteHitY1Array, NoteHitY2Array);
        }
        NotePositionsLoaded = true;
        InteractiveInstrument();
    }

    public interface SendBPM
    {
        List<string> SendBPMArray { get; }
    }

    private void getBPM(ScriptEventData gotBPM)
    {
        //Log.Write("Raver: In gotBPM");
        if (gotBPM.Data == null)
        {
            return;
        }
        SendBPM sendBPM = gotBPM.Data.AsInterface<SendBPM>();
        if (sendBPM == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        if (sendBPM.SendBPMArray.Count() > 0)
        {
            //Log.Write("Raver: Processing BPM Parameters");
            bpm = Int32.Parse(sendBPM.SendBPMArray[2]);
            //Log.Write("Raver: BPM in BeatRaver: " + bpm);
        }
    }

    public List<SoundResource> SampleLibrary = new List<SoundResource>();
    public List<string> SampleNames = new List<string>();

    public interface SendSamples
    {
        List<object> SendSampleLibrary { get; }
    }

    private void getSamples(ScriptEventData gotSamples)
    {
        //Log.Write("In getSamples");
        if (gotSamples.Data == null)
        {
            Log.Write(LogLevel.Warning, Script.ID.ToString(), "Expected non-null event data");
            return;
        }
        SendSamples sendSamples = gotSamples.Data.AsInterface<SendSamples>();
        if (sendSamples == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        SoundResource tempSample;
        string tempSampleName;
        int cntr = 0;
        //Log.Write("sendSamples.SendSampleLibrary.Count(): " + sendSamples.SendSampleLibrary.Count());
        if (sendSamples.SendSampleLibrary.Count() > 0)
        {
            do
            {
                //Log.Write("cntr: " + cntr);
                if ((cntr == 1) || (cntr == 3) || (cntr == 5) || (cntr == 7) || (cntr == 9))
                {
                    tempSample = sendSamples.SendSampleLibrary.ElementAt(cntr) as SoundResource;
                    SampleLibrary.Add(tempSample);
                    //Log.Write("sample added: " + tempSample.GetName());
                }
                else
                {
                    tempSampleName = sendSamples.SendSampleLibrary.ElementAt(cntr) as string;
                    SampleNames.Add(tempSampleName);
                    //Log.Write("Sample Name Added: " + tempSampleName);
                }
                //Errors = Errors + ", " + tempSample.GetName();
                //Log.Write("Sample Loaded: " + tempSample.GetName());
                cntr++;
            } while (cntr < sendSamples.SendSampleLibrary.Count());
        }
        SamplesLoaded = true;
}

    public List<string>[] InstrumentArray = new List<string>[128];

    public interface SendInstrument
    {
        List<string> SendInstrumentArray { get; }
    }

    private void getInstrument(ScriptEventData gotInstrument)
    {
        if (gotInstrument.Data == null)
        {
            Log.Write(LogLevel.Warning, Script.ID.ToString(), "Expected non-null instrument event data");
            return;
        }
        SendInstrument sendInstrument = gotInstrument.Data.AsInterface<SendInstrument>();
        if (sendInstrument == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }
        if (sendInstrument.SendInstrumentArray.Count() > 0)
        {
            Log.Write("loading instrument: " + sendInstrument.SendInstrumentArray[0]);
            InstrumentArray[instrumentcntr] = new List<string>();
            int notecntr = 0;
            do
            {
                if (notecntr > 0)
                {
                    InstrumentArray[instrumentcntr].Add(sendInstrument.SendInstrumentArray[notecntr]);
                }
                else
                {
                    InstrumentName.Add(sendInstrument.SendInstrumentArray[0]);  //first entry of SendInstrumentArray is the name of the instrument
                }
                notecntr++;
            } while (notecntr < sendInstrument.SendInstrumentArray.Count());
            instrumentcntr++;
        }
#if (InstrumentBuild)
        if (sendInstrument.SendInstrumentArray[0] == StandAloneInstrument)
        {
            if (String.IsNullOrEmpty(startNote))
            {
                startNote = "c2";
            }
            if (String.IsNullOrEmpty(scaleIn))
            {
                scaleIn = "chromatic";
            }
            string DefaultInstrument = "/jam inst(" + StandAloneInstrument + ") scale(" + startNote + ", " + scaleIn + ") octaves(4)";
            ParseCommands(DefaultInstrument);
            Log.Write("INSTRUMENT READY");
            
        }
#endif

        InstrumentLoaded = true;

}

    public class SendNoteMatches : Reflective
    {
        public ScriptId SourceScriptId { get; internal set; }

        public List<string> NoteMatch { get; internal set; }

        public List<string> SendNoteMatch
        {
            get
            {
                return NoteMatch;
            }
        }
    }

    public class SendAttaBoys : Reflective
    {
        public ScriptId SourceScriptId { get; internal set; }

        public List<string> AttaBoy { get; internal set; }

        public List<string> SendAttaBoy
        {
            get
            {
                return AttaBoy;
            }
        }
    }

    private List<SoundResource> BuildSamples(List<string> Tempcmds)
    {
        string strtest;
        string cmdline;
        int cntr = 0;
        bool SampleFound = false;
        List<SoundResource> TempSamples = new List<SoundResource>();
        //Log.Write("In Build Samples");
        //Log.Write("tempcmds.count: " + Tempcmds.Count);
        do
        {
            cmdline = Tempcmds[cntr];
            //Log.Write("cmdline: " + cmdline);
            if (cmdline.Contains("sample") || cmdline.Contains("inst") || cmdline.Contains("midi"))
            {
                SampleFound = false;
                int namecntr = 0;
                do
                {
                    //Log.Write("namecntr: " + namecntr);
                    strtest = SampleNames[namecntr];
                    //Log.Write("strtest: " + strtest);
                    if (cmdline.Contains(strtest))
                    {
                        SampleFound = true;
                        //Log.Write("match");
                        //Log.Write("Name of Sample Chosen: " + SampleLibrary[namecntr].GetName());
                        TempSamples.Add(SampleLibrary[namecntr]);
                    }
                    namecntr++;
                } while (namecntr < SampleNames.Count); 
                if (!SampleFound)
                {
                    ScenePrivate.Chat.MessageAllUsers("Sample - " + cmdline + " - not found");
                }
            }
            cntr++;
        } while (cntr < Tempcmds.Count);

        return TempSamples;
    }

    private void GetChatCommand(ChatData Data)
    {
        string DataCmd = Data.Message;
        Log.Write("DataCmd: " + DataCmd);
        string InstrumentToChange = StandAloneInstrument;
        string StartNoteToChange = startNote;
        string ScaleToChange = scaleIn;
        if (DataCmd.Contains("/inst("))
        {
            //fix up cmd
            int from = DataCmd.IndexOf("inst(", StringComparison.CurrentCulture);
            string test = DataCmd.Substring(from, DataCmd.Length - from);
            int to = test.IndexOf(")", StringComparison.CurrentCulture);
            InstrumentToChange = test.Substring(5, to - 5);
        }
        if (DataCmd.Contains("/scale("))
        {
            //fix up cmd
            int from = DataCmd.IndexOf("(", StringComparison.CurrentCulture);
            //Log.Write("from: " + from);
            int to = DataCmd.IndexOf(",", StringComparison.CurrentCulture);
            //Log.Write("to: " + to);
            string test = DataCmd.Substring(from + 1, to - from - 1);
            //Log.Write("test: " + test);
            StartNoteToChange = test;
            //Log.Write("New Start Note: " + StartNoteToChange);
            ScaleToChange = DataCmd.Substring(to + 2, DataCmd.Length - to -3);
            //Log.Write("New Scale: " + ScaleToChange);
        }
        string DefaultInstrument = "/jam inst(" + InstrumentToChange + ") scale(" + StartNoteToChange + ", " + ScaleToChange + ") octaves(4)";
        DataCmd = DefaultInstrument;
        ParseCommands(DataCmd);
    }

    void SubscribeClientToButton(Client client, string Button)
    {
        ButtonSubscriptions.Add(client.SubscribeToCommand(Button, CommandAction.Pressed, CommandReceived, CommandCanceled));
        ButtonSubscriptions.Add(client.SubscribeToCommand(Button, CommandAction.Released, CommandReceived, CommandCanceled));
    }

    void UnsubscribeAllButtons()
    {
        foreach (IEventSubscription sub in ButtonSubscriptions) sub.Unsubscribe();
        ButtonSubscriptions.Clear();
    }

    //void SubscribeKeyPressed(AgentPrivate agent, string Message)
    void SubscribeKeyPressed(Client client, string Message)
    {
        Log.Write("in SubscribeKeyPressed");
        Log.Write("client: " + client);
        Log.Write("message: " + Message);
        if (client == null) return;

        if (Message == "sub" && TheUser == null)
        {
            //TheUser = agent;
            //Client client = agent.Client;
            SubscribeClientToButton(client, "Keypad0");
            SubscribeClientToButton(client, "Keypad1");
            SubscribeClientToButton(client, "Keypad2");
            SubscribeClientToButton(client, "Keypad3");
            SubscribeClientToButton(client, "Keypad4");
            SubscribeClientToButton(client, "Keypad5");
            SubscribeClientToButton(client, "Keypad6");
            SubscribeClientToButton(client, "Keypad7");
            SubscribeClientToButton(client, "Keypad8");
            SubscribeClientToButton(client, "Keypad9");
            SubscribeClientToButton(client, "Action1");
            SubscribeClientToButton(client, "Action2");
            SubscribeClientToButton(client, "Action3");
            SubscribeClientToButton(client, "Action4");
            SubscribeClientToButton(client, "Action5");
            SubscribeClientToButton(client, "Action6");
            SubscribeClientToButton(client, "Action7");
            SubscribeClientToButton(client, "Action8");
            SubscribeClientToButton(client, "Action9");
            SubscribeClientToButton(client, "Action0");
            SubscribeClientToButton(client, "Modifier");
        }

        if (Message == "unsub")
        {
            UnsubscribeAllButtons();
            TheUser = null;
            return;
        }
    }

    void CommandReceived(CommandData Button)
    {
        loopNote = true;
        if ((Button.Command == "Modifier") && (Button.Action == CommandAction.Pressed))  //shift acts a toggle
        {
            if (Shifted == true) Shifted = false;
            else Shifted = true;
        }

        if ((Button.Command == "Action1") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(1, 0.0f);
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(2, 0.0f);
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(3, 0.0f);
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(4, 0.0f);
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(5, 0.0f);
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(6, 0.0f);
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(7, 0.0f);
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(8, 0.0f);
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(9, 0.0f);
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(10, 0.0f);
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(11, 0.0f);
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(12, 0.0f);
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(13, 0.0f);
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(14, 0.0f);
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(15, 0.0f);
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(16, 0.0f);
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(17, 0.0f);
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(18, 0.0f);
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(19, 0.0f);
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) PlayJam(20, 0.0f);
        else if ((Button.Command == "Action1") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(21, 0.0f);
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(22, 0.0f);
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(23, 0.0f);
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(24, 0.0f);
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(25, 0.0f);
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(26, 0.0f);
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(27, 0.0f);
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(28, 0.0f);
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(29, 0.0f);
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(30, 0.0f);
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(31, 0.0f);
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(32, 0.0f);
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(33, 0.0f);
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(34, 0.0f);
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(35, 0.0f);
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(36, 0.0f);
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(37, 0.0f);
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(38, 0.0f);
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(39, 0.0f);
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Pressed) && (Shifted)) PlayJam(40, 0.0f);

        if ((Button.Command == "Action1") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[0].Stop();
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[1].Stop();
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[2].Stop();
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[3].Stop();
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[4].Stop();
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[5].Stop();
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[6].Stop();
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[7].Stop();
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[8].Stop();
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[9].Stop();
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[10].Stop();
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[11].Stop();
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[12].Stop();
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[13].Stop();
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[14].Stop();
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[15].Stop();
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[16].Stop();
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[17].Stop();
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[18].Stop();
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandle[19].Stop();
        else if ((Button.Command == "Action1") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[20].Stop();
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[21].Stop();
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[22].Stop();
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[23].Stop();
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[24].Stop();
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[25].Stop();
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[26].Stop();
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[27].Stop();
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[28].Stop();
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[29].Stop();
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[30].Stop();
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[31].Stop();
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[32].Stop();
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[33].Stop();
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[34].Stop();
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[35].Stop();
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[36].Stop();
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[37].Stop();
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[38].Stop();
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Released) && (Shifted)) playHandle[39].Stop();
    }

    void CommandCanceled(CancelData data)
    {
        Log.Write(GetType().Name, "Subscription canceled: " + data.Message);
    }

    #endregion

    #region Interaction

    private void LoadControlSurfaces(string tempNotePosition, int cntr)
    {
        //Takes Relative Values read in from configuration and converts them to realworld position 
        string[] values = new string[100];
        //Log.Write("sendSamples.SendSampleLibrary.Count(): " + sendSamples.SendSampleLibrary.Count());
        //Log.Write("sendNotePositions.SendNotePosition.Count(): " + sendNotePositions.SendNotePosition.Count());

        Log.Write("ZRotation: " + ZRotation);
        values = tempNotePosition.Split(',');
        InstrumentNotes[cntr] = values[0];
        ControlSurfaceXRelative[cntr] = float.Parse(values[1]);
        Log.Write("ControlSurfaceXRelative[" + cntr + "]: " + ControlSurfaceXRelative[cntr]);
        ControlSurfaceYRelative[cntr] = float.Parse(values[2]);
        Log.Write("ControlSurfaceYRelative[" + cntr + "]: " + ControlSurfaceYRelative[cntr]);
        ControlSurfaceRadiusArray[cntr] = float.Parse(values[3]);
        Log.Write("ControlSurfaceRadiusArray[" + cntr + "]: " + ControlSurfaceRadiusArray[cntr]);
        ControlSurfaceZMinimum[cntr] = float.Parse(values[4]);
        Log.Write("ControlSurfaceZMinimum[" + cntr + "]: " + ControlSurfaceZMinimum[cntr]);
        ControlSurfaceZMaximum[cntr] = float.Parse(values[5]);
        Log.Write("ControlSurfaceZMaximum[" + cntr + "]: " + ControlSurfaceZMaximum[cntr]);
        float CosAngle = (float)Math.Cos(ZRotation * 0.0174533);
        float SinAngle = (float)Math.Sin(ZRotation * 0.0174533);

        ControlSurfaceXRelativeAfterRotation[cntr] = (ControlSurfaceXRelative[cntr] * CosAngle) - (ControlSurfaceYRelative[cntr] * SinAngle);
        Log.Write("ControlSurfaceXRelativeAfterRotation[" + cntr + "]: " + ControlSurfaceXRelativeAfterRotation[cntr]);
        ControlSurfaceYRelativeAfterRotation[cntr] = (ControlSurfaceYRelative[cntr] * CosAngle) + (ControlSurfaceXRelative[cntr] * SinAngle);
        Log.Write("ControlSurfaceYRelativeAfterRotation[" + cntr + "]: " + ControlSurfaceYRelativeAfterRotation[cntr]);
    }

    private void InteractiveInstrument()
    {
        InstrumentInteraction.Subscribe((InteractionData idata) =>
        {
            //InstrumentInteraction.SetPrompt("Clicks: " + (++Clicks)
            //    + "\nHit:" + idata.HitPosition.ToString()
            //    + "\nFrom:" + idata.Origin.ToString()
            //    + "\nNormal:" + idata.HitNormal.ToString()
            //    + "\nBy:" + ScenePrivate.FindAgent(idata.AgentId).AgentInfo.Name);
            //Vector hitPosition = idata.HitPosition;
            PlayInteractionNote(idata.HitPosition);
            //Log.Write("idata.HitPosition.ToString()" + idata.HitPosition.ToString());
        });
    }

    private void PlayInteractionNote(Vector hitPosition)
    {
        loopNote = false;
        float hitXRelative = 0;
        float hitYRelative = 0;
        float hitRadius = 0;
        Log.Write("CurPosX: " + CurPos.X);
        Log.Write("CurPosY: " + CurPos.Y);
        Log.Write("hitPosition.X: " + hitPosition.X);
        Log.Write("hitPosition.Y: " + hitPosition.Y);
        //normalize to origin 0,0
        if (CurPos.X > 0.0)
        {
            hitXRelative = (hitPosition.X - CurPos.X) * 100;
        }
        else
        {
            hitXRelative = (hitPosition.X + CurPos.X) *100;
        }
        if (CurPos.Y > 0.0)
        {
            hitYRelative = (hitPosition.Y - CurPos.Y) *100;
        }
        else
        {
            hitYRelative = (hitPosition.Y + CurPos.Y) *100;
        }
        Log.Write("hitXRelative: " + hitXRelative);
        Log.Write("hitYRelative: " + hitYRelative);


        //Check to See if the Relative Hit Radius falls within a Control Surface
        int cntr = 0;
        int matchingNote = 0;
        float XminTest = 0;
        float XmaxTest = 0;
        float YminTest = 0;
        float YmaxTest = 0;

        do
        {
            XminTest = ControlSurfaceXRelativeAfterRotation[cntr] - ControlSurfaceRadiusArray[cntr];
            Log.Write("XminTest: " + XminTest);
            XmaxTest = ControlSurfaceXRelativeAfterRotation[cntr] + ControlSurfaceRadiusArray[cntr];
            Log.Write("XmaxTest: " + XmaxTest);
            YminTest = ControlSurfaceYRelativeAfterRotation[cntr] - ControlSurfaceRadiusArray[cntr];
            Log.Write("YminTest: " + YminTest);
            YmaxTest = ControlSurfaceYRelativeAfterRotation[cntr] + ControlSurfaceRadiusArray[cntr];
            Log.Write("YmaxTest: " + YmaxTest);

            if ((hitXRelative >= XminTest) && (hitXRelative <= XmaxTest))
            {
                Log.Write("Within X Range");
                if ((hitYRelative >= YminTest) && (hitYRelative <= YmaxTest))
                {
                    Log.Write("Within Y Range");
                    hitRadius = (float)Math.Sqrt(((hitXRelative - ControlSurfaceXRelativeAfterRotation[cntr]) * (hitXRelative - ControlSurfaceXRelativeAfterRotation[cntr])) + ((hitYRelative - ControlSurfaceYRelativeAfterRotation[cntr]) * (hitYRelative - ControlSurfaceYRelativeAfterRotation[cntr])));
                    Log.Write("hitRadius: " + hitRadius);
                    Log.Write("ControlSurfaceRadiusArray[" + cntr + "]: " + ControlSurfaceRadiusArray[cntr]);
                    if (hitRadius <= ControlSurfaceRadiusArray[cntr])
                    {
                        matchingNote = Int32.Parse(InstrumentNotes[cntr]);
                        Log.Write("Hit Note: " + matchingNote);
                    }
                }
            }
            cntr++;
        } while (cntr < NumNotePositions);
        Log.Write("NoteToPlay: " + matchingNote);
        PlayJam(matchingNote, 0);
    }

    #endregion

    #region SimonGame

    private void PlayMelody()
    {
        Log.Write("In Play Melody");
        Log.Write("Notes: " + intNoteArray.Count());
        Log.Write("Timing: " + fltTimingArray.Count());
        int cntr = 0;
        loopNote = false;
        int TempNote = 0;
        float TempTime = 0.0f;
        //Wait(TimeSpan.FromMilliseconds(200));
        preview = "Y";
        do
        {
            Log.Write("In PlayMelody Loop");
            Log.Write("NumOfEntries: " + NumOfEntries);
            Log.Write("cntr: " + cntr);
            Log.Write("intNoteArray: " + intNoteArray[cntr]);
            Log.Write("fltTimingArray: " + fltTimingArray[cntr]);
            TempNote = intNoteArray[cntr];
            TempTime = fltTimingArray[cntr];
            cntr++;
            PlayJam(TempNote, TempTime);

        } while (cntr < NumOfEntries);
        preview = "N";
        CheckNote = 0;
    }

    private void CheckMelody(int  notePlayed)
    {
        if (CheckNote < NoteArray.Count())
        {
            Log.Write("CheckNote: " + CheckNote);
            Log.Write("notePlayed: " + notePlayed);
            Log.Write("NoteArray[CheckNote]: " + NoteArray[CheckNote]);
            Log.Write("NoteArray.Count: " + NoteArray.Count());
            if (notePlayed == Int32.Parse(NoteArray[CheckNote]))
            {
                Log.Write("Note " + CheckNote + " matched");
                CheckNoteSend = CheckNote + 1;
                sendNoteMatches.NoteMatch.Clear();
                sendNoteMatches.NoteMatch.Add(preview);
                sendNoteMatches.NoteMatch.Add(CheckNoteSend.ToString());
                sendNoteMatches.NoteMatch.Add("match");
                sendNoteMatches.NoteMatch.Add(NoteArray[CheckNote]);
                sendNoteMatches.NoteMatch.Add(fltTimingArray[CheckNote].ToString());
                PostScriptEvent(ScriptId.AllScripts, "NoteMatch", sendNoteMatches);
                if ((CheckNote == NoteArray.Count()-1) && (preview == "N"))
                {
                    Log.Write("SendingAttaBoy");
                    sendAttaBoys.AttaBoy.Clear();
                    sendAttaBoys.AttaBoy.Add("Display");     
                    PostScriptEvent(ScriptId.AllScripts, "AttaBoy", sendAttaBoys);
                }
                CheckNote++;
            }
            else
            {
                Log.Write("Note " + CheckNote + " missed");
                CheckNoteSend = CheckNote + 1;
                sendNoteMatches.NoteMatch.Clear();
                sendNoteMatches.NoteMatch.Add(preview);
                sendNoteMatches.NoteMatch.Add(CheckNoteSend.ToString());
                sendNoteMatches.NoteMatch.Add("fail");
                sendNoteMatches.NoteMatch.Add(NoteArray[CheckNote]);
                sendNoteMatches.NoteMatch.Add(fltTimingArray[CheckNote].ToString());
                PostScriptEvent(ScriptId.AllScripts, "NoteMatch", sendNoteMatches);
                Wait(TimeSpan.FromMilliseconds(100));
                MelodyNotMatched();
            } 
        }
        else
        {
            //Log.Write("Notes being played after the Melody is at end");
            sendNoteMatches.NoteMatch.Clear();
            sendNoteMatches.NoteMatch.Add(preview);
            sendNoteMatches.NoteMatch.Add("99");
            sendNoteMatches.NoteMatch.Add("none");
            sendNoteMatches.NoteMatch.Add("99");
            sendNoteMatches.NoteMatch.Add("99");
            PostScriptEvent(ScriptId.AllScripts, "NoteMatch", sendNoteMatches);
        }    
    }

    private void MelodyNotMatched()
    {
        SceneInfo info = ScenePrivate.SceneInfo;
        ModalDialog Dlg;
        //AgentPrivate agent = ScenePrivate.FindAgent(Hitman);
        Log.Write("In MelodyNotMatched");
        if (Hitman == null)
            return;

        Dlg = Hitman.Client.UI.ModalDialog;
        WaitFor(Dlg.Show, ModalMessage, "Try Again", "Exit");
        Log.Write("Dlg.Response: " + Dlg.Response);
        if (Dlg.Response == "Try Again")
        {
            CheckNote = 0;
            Log.Write("StandAloneInstrument In Try Again, CheckNote: " + CheckNote);
            sendNoteMatches.NoteMatch.Clear();
            preview = "N";
            sendNoteMatches.NoteMatch.Add(preview);
            sendNoteMatches.NoteMatch.Add("99");
            sendNoteMatches.NoteMatch.Add("none");
            sendNoteMatches.NoteMatch.Add("99");
            sendNoteMatches.NoteMatch.Add("99");
            PostScriptEvent(ScriptId.AllScripts, "NoteMatch", sendNoteMatches);
            sendAttaBoys.AttaBoy.Clear();
            sendAttaBoys.AttaBoy.Add("Hide");
            PostScriptEvent(ScriptId.AllScripts, "AttaBoy", sendAttaBoys);
            PlayMelody();
        }
        if (Dlg.Response == "Exit")
        {
            GameReset();
        }
    }

    private void GameReset()
    {
        Log.Write("In Game Reset");
        CheckNote = 0;
        //Send Messages to Cubes to Reset
        sendNoteMatches.NoteMatch.Clear();
        preview = "Y";
        sendNoteMatches.NoteMatch.Add(preview);
        sendNoteMatches.NoteMatch.Add("99");
        sendNoteMatches.NoteMatch.Add("none");
        sendNoteMatches.NoteMatch.Add("99");
        sendNoteMatches.NoteMatch.Add("99");
        PostScriptEvent(ScriptId.AllScripts, "NoteMatch", sendNoteMatches);

        sendAttaBoys.AttaBoy.Clear();
        sendAttaBoys.AttaBoy.Add("Hide");
        PostScriptEvent(ScriptId.AllScripts, "AttaBoy", sendAttaBoys);

    }

    #endregion

    #region PlayMusic

    private void PlaySansar(float LoudnessIn, float PitchShiftIn, int PlayIndexIn, int LoopIn2, bool loopNote)
    {
        //PlaySettings playSettings = TrackPlay_Once[LoopIn2] ? PlaySettings.PlayOnce : PlaySettings.Looped;
        PlaySettings playSettings = !loopNote ? PlaySettings.PlayOnce : PlaySettings.Looped;
        playSettings.Loudness = TrackVolume[LoopIn2];
        playSettings.DontSync = TrackDont_Sync[LoopIn2];
        Log.Write("LoopIn2: " + LoopIn2);
        if (LoopIn2 == 8)
        {
            if ((bpm < 100) || (bpm > 100))
            {
                float tempRate = 0.0f;
                tempRate = float.Parse(bpm.ToString()) / 100.0f;
                playSettings.PitchShift = tempRate;
            }
            else
            {
                playSettings.PitchShift = TrackPitchShift[LoopIn2];
            }
        }
        else
        {
            playSettings.PitchShift = TrackPitchShift[LoopIn2];
        }
        //playHandle[LoopIn2] = ScenePrivate.PlaySoundAtPosition(TrackSamples[LoopIn2][PlayIndexIn], SoundPos[LoopIn2], playSettings);
        playHandle[LoopIn2] = ScenePrivate.PlaySound(TrackSamples[LoopIn2][PlayIndexIn], playSettings);
    }

    private void PlayNote(float LoudnessIn, float PitchShiftIn, int PlayIndexIn, int LoopIn2, bool loopNote)
    {
        //PlaySettings playSettings = TrackPlay_Once[LoopIn2] ? PlaySettings.PlayOnce : PlaySettings.Looped;
        //PlaySettings playSettings = !loopNote ? PlaySettings.PlayOnce : PlaySettings.Looped;
        playSettings[PlayIndexIn] = !loopNote ? PlaySettings.PlayOnce : PlaySettings.Looped;
        playSettings[PlayIndexIn].Loudness = TrackVolume[LoopIn2];
        playSettings[PlayIndexIn].DontSync = TrackDont_Sync[LoopIn2];
        //Log.Write("LoopIn2: " + LoopIn2);
        //Log.Write("PlayIndexIn: " + PlayIndexIn);
        if (LoopIn2 == 8)
        {
            if ((bpm < 100) || (bpm > 100))
            {
                float tempRate = 0.0f;
                tempRate = float.Parse(bpm.ToString()) / 100.0f;
                playSettings[PlayIndexIn].PitchShift = tempRate;
            }
            else
            {
                playSettings[PlayIndexIn].PitchShift = TrackPitchShift[LoopIn2];
            }
        }
        else
        {
            playSettings[PlayIndexIn].PitchShift = TrackPitchShift[LoopIn2];
        }
        //playHandle[LoopIn2] = ScenePrivate.PlaySoundAtPosition(TrackSamples[LoopIn2][PlayIndexIn], SoundPos[LoopIn2], playSettings);
        playHandle[PlayIndexIn] = ScenePrivate.PlaySound(TrackSamples[LoopIn2][PlayIndexIn], playSettings[PlayIndexIn]);
    }

    private void PlayJam(int playIndex, float timing)
    {
        //Log.Write("in PlayJam");
        //Log.Write("loopNum: " + loopNum);
        int loopIn = loopNum;
        
        //Log.Write("playIndex: " + playIndex);
        //Log.Write("TrackOffsets[loopIn][playIndex-1]: " + TrackOffsets[loopIn][playIndex - 1]);   
        TrackPitchShift[loopIn] = TrackOffsets[loopIn][playIndex-1];
        //PlaySansar(TrackVolume[loopIn], TrackPitchShift[loopIn], playIndex-1, loopIn, loopNote);
        //Log.Write("TrackVolume[loopIn]: " + TrackVolume[loopIn]);

        PlayNote(TrackVolume[loopIn], TrackPitchShift[loopIn], playIndex - 1, loopIn, loopNote);
        if (NumOfEntries > 0)
        {
            CheckMelody(playIndex);
        }
        Wait(TimeSpan.FromMilliseconds(timing));
    }

    #endregion

    #region BuildLoopParameters
    private void InitalizeDefaults()
    {
        BuildMidiNotes();
        Vector InitPos = new Vector(-2.0f, 0.0f, 0.0f);
        Vector IncPos = new Vector(0.25f, 0.0f, 0.0f);
        Vector LastPos = CurPos + InitPos;
        int cntr = 0;
        do
        {
            SoundPos.Add(LastPos + IncPos);
            LastPos = SoundPos[cntr];
            TrackArrayAccess[cntr] = "ring";
            TrackVolume[cntr] = 0f;
            TrackPitchShift[cntr] = 0f;
            TrackPlay_Once[cntr] = true;
            TrackDont_Sync[cntr] = true;
            cntr++;
        } while (cntr < numTracks);
    }

    private void BuildMidiNotes()
    {
        MidiNote.Add("c-"); MidiNote.Add("db-"); MidiNote.Add("d-"); MidiNote.Add("eb-"); MidiNote.Add("e-"); MidiNote.Add("f-"); MidiNote.Add("gb-"); MidiNote.Add("g-"); MidiNote.Add("ab-"); MidiNote.Add("a-"); MidiNote.Add("bb-"); MidiNote.Add("b-");
        MidiNote.Add("c0"); MidiNote.Add("db0"); MidiNote.Add("d0"); MidiNote.Add("eb0"); MidiNote.Add("e0"); MidiNote.Add("f0"); MidiNote.Add("gb0"); MidiNote.Add("g0"); MidiNote.Add("ab0"); MidiNote.Add("a0"); MidiNote.Add("bb0"); MidiNote.Add("b0");
        MidiNote.Add("c1"); MidiNote.Add("db1"); MidiNote.Add("d1"); MidiNote.Add("eb1"); MidiNote.Add("e1"); MidiNote.Add("f1"); MidiNote.Add("gb1"); MidiNote.Add("g1"); MidiNote.Add("ab1"); MidiNote.Add("a1"); MidiNote.Add("bb1"); MidiNote.Add("b1");
        MidiNote.Add("c2"); MidiNote.Add("db2"); MidiNote.Add("d2"); MidiNote.Add("eb2"); MidiNote.Add("e2"); MidiNote.Add("f2"); MidiNote.Add("gb2"); MidiNote.Add("g2"); MidiNote.Add("ab2"); MidiNote.Add("a2"); MidiNote.Add("bb2"); MidiNote.Add("b2");
        MidiNote.Add("c3"); MidiNote.Add("db3"); MidiNote.Add("d3"); MidiNote.Add("eb3"); MidiNote.Add("e3"); MidiNote.Add("f3"); MidiNote.Add("gb3"); MidiNote.Add("g3"); MidiNote.Add("ab3"); MidiNote.Add("a3"); MidiNote.Add("bb3"); MidiNote.Add("b3");
        MidiNote.Add("c4"); MidiNote.Add("db4"); MidiNote.Add("d4"); MidiNote.Add("eb4"); MidiNote.Add("e4"); MidiNote.Add("f4"); MidiNote.Add("gb4"); MidiNote.Add("g4"); MidiNote.Add("ab4"); MidiNote.Add("a4"); MidiNote.Add("bb4"); MidiNote.Add("b4");
        MidiNote.Add("c5"); MidiNote.Add("db5"); MidiNote.Add("d5"); MidiNote.Add("eb5"); MidiNote.Add("e5"); MidiNote.Add("f5"); MidiNote.Add("gb5"); MidiNote.Add("g5"); MidiNote.Add("ab5"); MidiNote.Add("a5"); MidiNote.Add("bb5"); MidiNote.Add("b5");
        MidiNote.Add("c6"); MidiNote.Add("db6"); MidiNote.Add("d6"); MidiNote.Add("eb6"); MidiNote.Add("e6"); MidiNote.Add("f6"); MidiNote.Add("gb6"); MidiNote.Add("g6"); MidiNote.Add("ab6"); MidiNote.Add("a6"); MidiNote.Add("bb6"); MidiNote.Add("b6");
        MidiNote.Add("c7"); MidiNote.Add("db7"); MidiNote.Add("d7"); MidiNote.Add("eb7"); MidiNote.Add("e7"); MidiNote.Add("f7"); MidiNote.Add("gb7"); MidiNote.Add("g7"); MidiNote.Add("ab7"); MidiNote.Add("a7"); MidiNote.Add("bb7"); MidiNote.Add("b7");
        MidiNote.Add("c8"); MidiNote.Add("db8"); MidiNote.Add("d8"); MidiNote.Add("eb8"); MidiNote.Add("e8"); MidiNote.Add("f8"); MidiNote.Add("gb8"); MidiNote.Add("g8"); MidiNote.Add("ab8"); MidiNote.Add("a8"); MidiNote.Add("bb8"); MidiNote.Add("b8");
        MidiNote.Add("c9"); MidiNote.Add("db8"); MidiNote.Add("d9"); MidiNote.Add("eb9"); MidiNote.Add("e9"); MidiNote.Add("f9"); MidiNote.Add("gb9"); MidiNote.Add("g9");
    }

    private List<string> ParseIt(string InString)
    {
        List<string> cmds = new List<string>();
        int strlen = 0;
        char semi = ')';
        string test = InString;
        string cmd;
        int beg = 0;
        int endcmd = 0;

        do
        {
            endcmd = test.IndexOf(semi);
            cmd = test.Substring(beg, endcmd);
            cmd = cmd.Replace('(', ' ');
            cmds.Add(cmd);
            test = test.Remove(beg, endcmd + 1);
            strlen = test.Length;
        } while (test.Length > 1);
        return cmds;
    }

    private float BuildVolume(string InString)
    {
        string substr = "vol(";
        int from = InString.IndexOf(substr, StringComparison.CurrentCulture);
        int length = InString.LastIndexOf(")", StringComparison.CurrentCulture);
        int last = length - from;
        string chunk = InString.Substring(from, last + 1);
        int next = chunk.IndexOf(")", StringComparison.CurrentCulture);
        string strVolume = chunk.Substring(4, next - 4);
        float fltVolume = float.Parse(strVolume);
        return fltVolume;
    }

    private Vector BuildPan(string InString)
    {
        string substr = "pan(";
        int from = InString.IndexOf(substr, StringComparison.CurrentCulture);
        int length = InString.LastIndexOf(")", StringComparison.CurrentCulture);
        int last = length - from;
        string chunk = InString.Substring(from, last + 1);
        int next = chunk.IndexOf(")", StringComparison.CurrentCulture);
        string strPan = chunk.Substring(4, next - 4);;
        float fltPan = float.Parse(strPan);
        Vector PanPos = new Vector(fltPan, 0.0f, 0.0f);
        PanPos = CurPos + PanPos;
        return PanPos;
    }
#endregion

    #region NotesScalesChords
    private List<string> BuildScale(string[] ScaleIn)
    {
        int[] TempScaleNotes = null;
        List<string> ReturnNotes = new List<string>();
        int notecntr = 0;
        int basenote = 0;
        //find index of base note in MidiNoteArray
        do
        {
            if (MidiNote[notecntr] == ScaleIn[0])
            {
                basenote = notecntr;
                break;
            }
            notecntr++;
        } while (notecntr < MidiNote.Count());
        ScaleIn[1] = ScaleIn[1].Substring(1, ScaleIn[1].Length - 1);
        switch (ScaleIn[1])
        {
            case "major":
                TempScaleNotes = major;
                break;
            case "dorian":
                TempScaleNotes = dorian;
                break;
            case "phrygian":
                TempScaleNotes = phrygian;
                break;
            case "lydian":
                TempScaleNotes = lydian;
                break;
            case "mixolydian":
                TempScaleNotes = mixolydian;
                break;
            case "aelian":
                TempScaleNotes = aelian;
                break;
            case "minor":
                TempScaleNotes = minor;
                break;
            case "minor_pentatonic":
                TempScaleNotes = minor_pentatonic;
                break;
            case "major_pentatonic":
                TempScaleNotes = major_pentatonic;
                break;
            case "egyptian":
                TempScaleNotes = egyptian;
                break;
            case "jiao":
                TempScaleNotes = jiao;
                break;
            case "zhi":
                TempScaleNotes = zhi;
                break;
            case "whole_tone":
                TempScaleNotes = whole_tone;
                break;
            case "whole":
                TempScaleNotes = whole;
                break;
            case "chromatic":
                TempScaleNotes = chromatic;
                break;
            case "harmonic_minor":
                TempScaleNotes = harmonic_minor;
                break;
            case "melodic_minor_asc":
                TempScaleNotes = melodic_minor_asc;
                break;
            case "hungarian_minor":
                TempScaleNotes = hungarian_minor;
                break;
            case "octatonic":
                TempScaleNotes = octatonic;
                break;
            case "messiaen1":
                TempScaleNotes = messiaen1;
                break;
            case "messiaen2":
                TempScaleNotes = messiaen2;
                break;
            case "messiaen3":
                TempScaleNotes = messiaen3;
                break;
            case "messiaen4":
                TempScaleNotes = messiaen4;
                break;
            case "messiaen5":
                TempScaleNotes = messiaen5;
                break;
            case "messiaen6":
                TempScaleNotes = messiaen6;
                break;
            case "messiaen7":
                TempScaleNotes = messiaen7;
                break;
            case "super_locrian":
                TempScaleNotes = super_locrian;
                break;
            case "hirajoshi":
                TempScaleNotes = hirajoshi;
                break;
            case "kumoi":
                TempScaleNotes = kumoi;
                break;
            case "neapolitan_major":
                TempScaleNotes = neapolitan_major;
                break;
            case "bartok":
                TempScaleNotes = bartok;
                break;
            case "bhairav":
                TempScaleNotes = bhairav;
                break;
            case "locrian_major":
                TempScaleNotes = locrian_major;
                break;
            case "ahirbhairav":
                TempScaleNotes = ahirbhairav;
                break;
            case "enigmatic":
                TempScaleNotes = enigmatic;
                break;
            case "neapolitan_minor":
                TempScaleNotes = neapolitan_minor;
                break;
            case "pelog":
                TempScaleNotes = pelog;
                break;
            case "augmented2":
                TempScaleNotes = augmented2;
                break;
            case "scriabin":
                TempScaleNotes = scriabin;
                break;
            case "harmonic_major":
                TempScaleNotes = harmonic_major;
                break;
            case "melodic_minor_desc":
                TempScaleNotes = melodic_minor_desc;
                break;
            case "romanian_minor":
                TempScaleNotes = romanian_minor;
                break;
            case "hindu":
                TempScaleNotes = hindu;
                break;
            case "iwato":
                TempScaleNotes = iwato;
                break;
            case "melodic_minor":
                TempScaleNotes = melodic_minor;
                break;
            case "diminished":
                TempScaleNotes = diminished;
                break;
            case "marva":
                TempScaleNotes = marva;
                break;
            case "melodic_major":
                TempScaleNotes = melodic_major;
                break;
            case "indian":
                TempScaleNotes = indian;
                break;
            case "spanish":
                TempScaleNotes = spanish;
                break;
            case "prometheus":
                TempScaleNotes = prometheus;
                break;
            case "diminished2":
                TempScaleNotes = diminished2;
                break;
            case "todi":
                TempScaleNotes = todi;
                break;
            case "leading_whole":
                TempScaleNotes = leading_whole;
                break;
            case "augmented":
                TempScaleNotes = augmented;
                break;
            case "purvi":
                TempScaleNotes = purvi;
                break;
            case "chinese":
                TempScaleNotes = chinese;
                break;
            case "chdmajor":
                TempScaleNotes = chdmajor;
                break;
            case "chdminor":
                TempScaleNotes = chdminor;
                break;
            case "chdmajor7":
                TempScaleNotes = chdmajor7;
                break;
            case "chddom7":
                TempScaleNotes = chddom7;
                break;
            case "chdminor7":
                TempScaleNotes = chdminor7;
                break;
            case "chdaug":
                TempScaleNotes = chdaug;
                break;
            case "chddim":
                TempScaleNotes = chddim;
                break;
            case "chddim7":
                TempScaleNotes = chddim7;
                break;
            case "chdhalfdim":
                TempScaleNotes = chdhalfdim;
                break;
            case "chd1":
                TempScaleNotes = chd1;
                break;
            case "chd5":
                TempScaleNotes = chd5;
                break;
            case "chdmaug5":
                TempScaleNotes = chdmaug5;
                break;
            case "chdsus2":
                TempScaleNotes = chdsus2;
                break;
            case "chdsus4":
                TempScaleNotes = chdsus4;
                break;
            case "chd6":
                TempScaleNotes = chd6;
                break;
            case "chdm6":
                TempScaleNotes = chdm6;
                break;
            case "chd7sus2":
                TempScaleNotes = chd7sus2;
                break;
            case "chd7sus4":
                TempScaleNotes = chd7sus4;
                break;
            case "chd7dim5":
                TempScaleNotes = chd7dim5;
                break;
            case "chd7aug5":
                TempScaleNotes = chd7aug5;
                break;
            case "chdm7aug5":
                TempScaleNotes = chdm7aug5;
                break;
            case "chd9":
                TempScaleNotes = chd9;
                break;
            case "chdm9":
                TempScaleNotes = chdm9;
                break;
            case "chdm7aug9":
                TempScaleNotes = chdm7aug9;
                break;
            case "chdmaj9":
                TempScaleNotes = chdmaj9;
                break;
            case "chd9sus4":
                TempScaleNotes = chd9sus4;
                break;
            case "chd6sus9":
                TempScaleNotes = chd6sus9;
                break;
            case "chdm6sus9":
                TempScaleNotes = chdm6sus9;
                break;
            case "chd7dim9":
                TempScaleNotes = chd7dim9;
                break;
            case "chdm7dim9":
                TempScaleNotes = chdm7dim9;
                break;
            case "chd7dim10":
                TempScaleNotes = chd7dim10;
                break;
            case "chd7dim11":
                TempScaleNotes = chd7dim11;
                break;
            case "chd7dim13":
                TempScaleNotes = chd7dim13;
                break;
            case "chd9dim5":
                TempScaleNotes = chd9dim5;
                break;
            case "chdm9dim5":
                TempScaleNotes = chdm9dim5;
                break;
            case "chd7aug5dim9":
                TempScaleNotes = chd7aug5dim9;
                break;
            case "chdm7aug5dim9":
                TempScaleNotes = chdm7aug5dim9;
                break;
            case "chd11":
                TempScaleNotes = chd11;
                break;
            case "chdm11":
                TempScaleNotes = chdm11;
                break;
            case "chdmaj11":
                TempScaleNotes = chdmaj11;
                break;
            case "chd11aug":
                TempScaleNotes = chd11aug;
                break;
            case "chdm11aug":
                TempScaleNotes = chdm11aug;
                break;
            case "chd13":
                TempScaleNotes = chd13;
                break;
            case "chdm13":
                TempScaleNotes = chdm13;
                break;
            case "chdadd2":
                TempScaleNotes = chdadd2;
                break;
            case "chdadd4":
                TempScaleNotes = chdadd4;
                break;
            case "chdadd9":
                TempScaleNotes = chdadd9;
                break;
            case "chdadd11":
                TempScaleNotes = chdadd11;
                break;
            case "add13":
                TempScaleNotes = add13;
                break;
            case "madd2":
                TempScaleNotes = madd2;
                break;
            case "madd4":
                TempScaleNotes = madd4;
                break;
            case "madd9":
                TempScaleNotes = madd9;
                break;
            case "madd11":
                TempScaleNotes = madd11;
                break;
            case "madd13":
                TempScaleNotes = madd13;
                break;
            default:
                Errormsg = "Scale or Chord Not Found";
                break;
        }

        ReturnNotes.Add(ScaleIn[0]); //first note is the base note

        if (!(Errormsg == "Scale or Chord Not Found"))
        {
            //Get the Rest of the notes of the scale
            notecntr = 0;
            do
            {
                ReturnNotes.Add(MidiNote[basenote + TempScaleNotes[notecntr]]);
                basenote = basenote + TempScaleNotes[notecntr];
                notecntr++;
            } while (notecntr < TempScaleNotes.Count() - 1);
        }

        return ReturnNotes;
    }

    private int FindMidiNote(string MidiNoteIn)
    {
        int x = 0;
        do
        {
            if (MidiNote[x] == MidiNoteIn) break;
            x++;
        } while (x < MidiNote.Count());
        return x;
    }

    private bool FindValidNote(string NoteCheck)
    {
        int x = 0;
        bool noteTest = false;
        do
        {
            if (validNotes[x] == NoteCheck)
            {
                noteTest = true;
                break;
            }
            x++;
        } while (x < validNotes.Count());
        return noteTest;
    }

    private List<int> BuildNotes(List<string> Tempcmds)
    {
        string cmdline;
        string strNotes = "";
        char comma = ',';
        int octaves = 0;
        int cntr = 0;
        int notecntr = 0;
        bool noteFound = false;
        List<string> strTempNotes = new List<string>();
        List<int> intTempNotes = new List<int>();

        do
        {
            cmdline = Tempcmds[cntr];
            if (cmdline.Contains("notes"))
            {
                //parse note arrays
                // notes[e3,g3,a3]
                if (cmdline.Contains("["))
                {
                    string[] NoteArray = cmdline.Split(comma);

                    //fix up first entry
                    int from = NoteArray[0].IndexOf("[", StringComparison.CurrentCulture);
                    if (NoteArray[0].Length == 11) strNotes = NoteArray[0].Substring(from + 1, 3);
                    if (NoteArray[0].Length == 10) strNotes = NoteArray[0].Substring(from + 1, 2);
                    strNotes = strNotes.Trim();
                    noteFound = FindValidNote(strNotes);
                    if (!noteFound) Errormsg = "Invalid Note Name";
                    else
                    {
                        strTempNotes.Add(strNotes);
                        NoteArray[0] = strNotes;
                        //fix up last entry
                        int lastentry = NoteArray.Count();
                        strNotes = NoteArray[lastentry - 1];
                        if (NoteArray[lastentry - 1].Length == 4) strNotes = NoteArray[lastentry - 1].Substring(0, 3);
                        if (NoteArray[lastentry - 1].Length == 3) strNotes = NoteArray[lastentry - 1].Substring(0, 2);
                        strNotes = strNotes.Trim();
                        noteFound = FindValidNote(strNotes);
                        if (!noteFound) Errormsg = "Invalid Note Name";
                        else
                        {
                            NoteArray[lastentry - 1] = strNotes;
                            notecntr = 0;
                            do
                            {
                                NoteArray[notecntr] = NoteArray[notecntr].Trim();
                                noteFound = FindValidNote(NoteArray[notecntr]);
                                if (!noteFound)
                                {
                                    Errormsg = "Invalid Note Name";
                                    notecntr = NoteArray.Count();
                                }
                                else
                                {
                                    if (notecntr > 0) strTempNotes.Add(NoteArray[notecntr]);
                                    notecntr++;
                                }
                            } while (notecntr < NoteArray.Count());
                        }
                    }
                }
                //parse single note
                else
                {
                    if (cmdline.Length == 10) strNotes = cmdline.Substring(cmdline.Length - 3, 3);
                    if (cmdline.Length == 9) strNotes = cmdline.Substring(cmdline.Length - 2, 2);
                    noteFound = FindValidNote(strNotes);
                    if (noteFound) strTempNotes.Add(strNotes);
                    else Errormsg = "Invalid Note Name";
                }
            }
            //parse scales
            if (cmdline.Contains("scale") || cmdline.Contains("chord"))
            {
                string[] NoteArray = cmdline.Split(comma);
                NoteArray[0] = NoteArray[0].Substring(7, NoteArray[0].Length - 7);
                if (FindMidiNote(NoteArray[0]) == 128)
                {
                    Errormsg = "Base Note of Scale or Chord is Not Correct";
                }
                else
                {
                    strTempNotes = BuildScale(NoteArray);
                }
            }

            //octaves processing
            if (!(Errormsg == "Base Note of Scale or Chord is Not Correct"))
            {
                if (cmdline.Contains("octaves"))
                {
                    string strOctaves = cmdline.Substring(9, 1);
                    octaves = Int32.Parse(cmdline.Substring(9, 1));
                }
            }

            cntr++;
        } while (cntr < Tempcmds.Count);

        if (strTempNotes.Count() > 0)
        {
            notecntr = 0;
            do
            {
                intTempNotes.Add(FindMidiNote(strTempNotes[notecntr]));
                notecntr++;
            } while (notecntr < strTempNotes.Count());
        }

        if (octaves > 0)
        {
            int octcntr = 0;
            int arraylength = intTempNotes.Count();
            do
            {
                notecntr = 0;
                do
                {
                    intTempNotes.Add(intTempNotes[notecntr] + (12 * (octcntr + 1)));
                    notecntr++;
                } while (notecntr < arraylength);
                octcntr++;
            } while (octcntr < octaves - 1);
        }

        return intTempNotes;
    }

    private int FindInstrument(string instrumentToFind)
    {
        int instcntr = 0;
        int instindex = 99;
        do  //Find Instrument in Instrument Array
        {
            if (instrumentToFind == InstrumentName[instcntr])
            {
                instindex = instcntr;
            }
            instcntr++;
        } while (instcntr < InstrumentName.Count());
        return instindex;
    }
#endregion

    #region dump
    private void dump()
    {
        Log.Write("In dump");
        if (!(TrackSequence[loopNum] == null))
        {
            int cntr = 0;
            do
            {
                Log.Write("TrackSequence " + loopNum + ": " + TrackSequence[loopNum][cntr]);
                cntr++;
            } while (cntr < TrackSequence[loopNum].Count());
        }
        else Log.Write("Track Sequence is null");

        Log.Write("TrackSamplesCount: " + TrackSamples[loopNum].Count());

        int xcntr = 0;
        do
        {
            //Log.Write("TrackSample" + loopNum + ": " + TrackSamples[loopNum][xcntr].GetName());
            xcntr++;
        } while (xcntr < TrackSamples[loopNum].Count());

        Log.Write("TrackOffsetsCount: " + TrackOffsets[loopNum].Count());
        if (!(TrackOffsets[loopNum] == null))
        {
            int cntr = 0;
            do
            {
                Log.Write("TrackOffset" + loopNum + ": " + TrackOffsets[loopNum][cntr]);
                cntr++;
            } while (cntr < TrackOffsets[loopNum].Count());
        }
        else Log.Write("TrackOffset is null");

        Log.Write("TrackMilliSecondsCount: " + TrackMilliSeconds[loopNum].Count());
        if (!(TrackMilliSeconds[loopNum] == null))
        {
            int cntr = 0;
            do
            {
                Log.Write("TrackBeats: " + TrackMilliSeconds[loopNum][cntr]);
                cntr++;
            } while (cntr < TrackMilliSeconds[loopNum].Count());
        }
        else Log.Write("Track MilliSeconds is null");
    }
#endregion

    #region Tracks
    private void StopTrack()
    {
        int loopIn = loopNum;
        TrackStop[loopIn] = true;
        while (TrackRunning[loopIn])
        {
            Wait(TimeSpan.FromMilliseconds(10));
        }
        TrackStop[loopIn] = false;
    }

    private void StopAll()
    {
        int loopNum = 0;
        int loopcntr = 0;
        do
        {
            if (TrackRunning[loopNum])
            {
                loopNum = loopcntr;
                StartCoroutine(StopTrack);
                loopcntr++;
            }
        } while (loopcntr < TrackRunning.Count());
    }
    #endregion

    #region CommandParser
    private void ParseCommands(string DataCmdIn)
    {
        loopNum = 0;
        Errormsg = "No Errors";
#if (InstrumentBuild)
        if (DataCmdIn.Contains("/loop"))
        {
            DataCmdIn = "/jam inst(" + StandAloneInstrument + ") scale(" + startNote + ", " + scaleIn + ") octaves(4)";
            ScenePrivate.Chat.MessageAllUsers("Loop command is invalid for standalone Instrument ... get AlgoRaver");
        }
#endif
        Log.Write(DataCmdIn);
        if (DataCmdIn.Contains("/"))
        {
            strErrors = false;
            if (DataCmdIn.Contains("/loop") || (DataCmdIn.Contains("/jam")))
            {
                if (DataCmdIn.Contains("/jam")) loopNum = 8;
                if (DataCmdIn.Contains("inst("))
                {
                    List<string> TempCmdsIn;
                    List<string> TempSamplesIn = new List<string>();
                    List<int> TempOffsetsIn = new List<int>();
                    TempCmdsIn = ParseIt(DataCmdIn);
                    if (TrackRunning[loopNum]) StartCoroutine(StopTrack);
                    if (DataCmdIn.Contains("vol(")) TrackVolume[loopNum] = BuildVolume(DataCmdIn);
                    if (DataCmdIn.Contains("pan(")) SoundPos[loopNum] = BuildPan(DataCmdIn);
                    TrackNotes[loopNum] = BuildNotes(TempCmdsIn);
                    if (!(Errormsg == "No Errors"))
                    {
                        strErrors = true;
                        Log.Write("Error: " + Errormsg);
                        ScenePrivate.Chat.MessageAllUsers("Error: " + Errormsg);
                    }
                    else strErrors = false;
                    if (!strErrors)
                    {
                        if (!(TrackSequence[loopNum] == null)) TrackSequence[loopNum].Clear();
                        if (!(TrackSamples[loopNum] == null)) TrackSamples[loopNum].Clear();
                        if (!(TrackMilliSeconds[loopNum] == null)) TrackMilliSeconds[loopNum].Clear();
                        if (!(TrackOffsets[loopNum] == null)) TrackOffsets[loopNum].Clear();
                        int notecntr = 0;
                        string strOffset = "";
                        int intOffset = 0;
                        do
                        {
                            int from = DataCmdIn.IndexOf("inst(", StringComparison.CurrentCulture);
                            string test = DataCmdIn.Substring(from, DataCmdIn.Length - from);
                            int to = test.IndexOf(")", StringComparison.CurrentCulture);
                            string InstrumentToFind = test.Substring(5, to - 5);
                            //int instcntr = 0;
                            int instindex = FindInstrument(InstrumentToFind);
                            if (instindex == 99)
                            {
                                strErrors = true;
                                ScenePrivate.Chat.MessageAllUsers("Instrument Not Found");
                            }
                            else
                            {
                                TempSamplesIn.Add("sample " + InstrumentArray[instindex][TrackNotes[loopNum][notecntr] * 2]);  // add string of sample to be used in temp array                                                                                       
                                strOffset = InstrumentArray[instindex][TrackNotes[loopNum][notecntr] * 2 + 1];
                                intOffset = Int32.Parse(strOffset);
                                TempOffsetsIn.Add(intOffset);  // add int of offset to be used in temp array
                            }
                            notecntr++;
                        } while (notecntr < TrackNotes[loopNum].Count());

                        TrackSamples[loopNum] = BuildSamples(TempSamplesIn);
                        if (TrackSamples[loopNum].Count == 0)
                        {
                            strErrors = true;
                            ScenePrivate.Chat.MessageAllUsers("Sample Not Found");
                        }
                        TrackOffsets[loopNum] = TempOffsetsIn;
                       // dump();
                    }
                }
            }
        }

        if (DataCmdIn.Contains("stopall"))
        {
            StopAll();
        }

        if (DataCmdIn.Contains("memory"))
        {
          Log.Write("Bytes Used: " + Memory.UsedBytes);
        }

    }
#endregion
}

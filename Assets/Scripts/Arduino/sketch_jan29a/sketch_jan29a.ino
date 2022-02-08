#include "pitches.h"

// notes in the melody:
int melody[] = {
  NOTE_C4
};

// note durations: 4 = quarter note, 8 = eighth note, etc.:
int noteDurations[] = {
  1
};

void firstone(){
  // iterate over the notes of the melody:
  for (int thisNote = 0; thisNote < 3; thisNote++) {

    // to calculate the note duration, take one second divided by the note type.
    //e.g. quarter note = 1000 / 4, eighth note = 1000/8, etc.
    int noteDuration = 4000 / noteDurations[thisNote];
    tone(8, melody[thisNote], noteDuration);

    // to distinguish the notes, set a minimum time between them.
    // the note's duration + 30% seems to work well:
    int pauseBetweenNotes = noteDuration * 1.30;
    delay(pauseBetweenNotes);
    // stop the tone playing:
    noTone(8);
  }
}

/* pcomp_117
  * physical computing, p117
  *
  * Digital PWM output on pin 9 to piezo buzzer
  *
  */

// For more information about tones, see Arduino tutorial on
// playing a melody

int  ctone = 3830;  // 3830 261 Hz
int  dtone = 3400;  // 294 Hz
int  etone = 3038;  // 329 Hz

int speakerPin = 9;

long elapsedtime = 0;
long duration = 8000000;  // play each tone for about a half second (microseconds)

void secondone()
{
  while(elapsedtime < duration) {
    analogWrite(speakerPin, 255);
    delayMicroseconds(ctone / 2);
    analogWrite(speakerPin, 0);
    delayMicroseconds(ctone / 2);
    elapsedtime += ctone;
  }
  elapsedtime = 0;
  delay(500);
  while(elapsedtime < duration) {
    digitalWrite(speakerPin, HIGH);
    delayMicroseconds(ctone / 2);
    digitalWrite(speakerPin, LOW);
    delayMicroseconds(ctone / 2);
    elapsedtime += ctone;
  }
  elapsedtime = 0;
  delay(500);
  while(elapsedtime < duration) {
    digitalWrite(speakerPin, HIGH);
    delayMicroseconds(ctone / 2);
    digitalWrite(speakerPin, LOW);
    delayMicroseconds(ctone / 2);
    elapsedtime += ctone;
  }
  elapsedtime = 0;
  delay(500);
}

void setup() {
  pinMode(speakerPin, OUTPUT);
//  firstone();
  secondone();

//  for (int thisNote = 0; thisNote < 3; thisNote++) {
//    int noteDuration = 4000 / noteDurations[thisNote];
//    tone(8, melody[thisNote], noteDuration);
//    
//    while(elapsedtime < duration) {
//        analogWrite(speakerPin, 255);
//        delayMicroseconds(ctone / 2);
//        analogWrite(speakerPin, 0);
//        delayMicroseconds(ctone / 2);
//        elapsedtime += ctone;
//      }
//      elapsedtime = 0;
//
//    int pauseBetweenNotes = noteDuration * 1.30;
//    delay(pauseBetweenNotes);
//    
//    noTone(8);
//  }
  
}

void loop() {
  // no need to repeat the melody.
}

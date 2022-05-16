#include <Servo.h>

Servo tensionMotor_left;
Servo tensionMotor_right;

int motorPin_left = 5;
int motorPin_right = 6;

int slack = 0;
int tense = 5;

char curTensionMode_left = 'S';
char curTensionMode_right = 'S';

void setup() {
  tensionMotor_left.attach(motorPin_left);
  tensionMotor_right.attach(motorPin_right);
  
  Serial.begin(9600);

}

void loop() {
  String message = Serial.readString();

  char tensionMode_left = message[0];
  char tensionMode_right = message[1];

  if(tensionMode_left != curTensionMode_left){
    moveTensionMotor(tensionMotor_left, tensionMode_left);
  }
  if(tensionMode_right != curTensionMode_right){
    moveTensionMotor(tensionMotor_right, tensionMode_right);
  }

  curTensionMode_left = tensionMode_left;
  curTensionMode_right = tensionMode_right;

}

void moveTensionMotor(Servo motor, char mode){
  if(mode == 'S'){motor.write(slack);}
  else if(mode == 'T'){motor.write(tense);}
}

#include <Servo.h>

Servo tensionMotor_left;
Servo tensionMotor_right;

int motorPin_left = 5;
int motorPin_right = 6;

int slack = 90;
int tense = 20;

char curTensionMode_left = 'S';
char curTensionMode_right = 'S';

void setup() {
  tensionMotor_left.attach(motorPin_left);
  tensionMotor_right.attach(motorPin_right);

  Serial.begin(9600);

  tensionMotor_left.write(slack);
  tensionMotor_right.write(slack);

}

void loop() {
  String message = Serial.readString();

  char tensionMode_left = message[0];
  char tensionMode_right = message[1];

  moveTensionMotor(tensionMotor_left, tensionMode_left, 1);
  moveTensionMotor(tensionMotor_right, tensionMode_right, -1);

}

void moveTensionMotor(Servo motor, char mode, int dir) {
  if (mode == 'S') {
    motor.write(slack);
  }
  else if (mode == 'T') {
    motor.write(slack + (tense * dir));
  }
}

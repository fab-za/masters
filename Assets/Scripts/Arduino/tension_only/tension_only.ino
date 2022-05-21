#include <Servo.h>

Servo tensionMotor_left;
Servo tensionMotor_right;

int motorPin_left = 5;
int motorPin_right = 6;

//-------- VARIABLES ---------------------

char buf[4];

int slack = 90;
int tense = 20;

char tensionMode_left = 'S';
char tensionMode_right = 'S';

void setup() {  
  tensionMotor_left.attach(motorPin_left);
  tensionMotor_right.attach(motorPin_right);

  Serial.begin(9600);
  Serial.setTimeout(200);

  tensionMotor_left.write(110);
  tensionMotor_right.write(70);

  delay(500);
  
  tensionMotor_left.write(slack);
  tensionMotor_right.write(slack);
}

void loop() {
  String message = Serial.readStringUntil('\n');
//  Serial.println("message received: " + message);

  if(message != ""){
    message.toCharArray(buf, 4);
  }

//  delay(10);

  tensionMode_left = buf[0];
  tensionMode_right = buf[1];

//  Serial.println("left tension received: " + tensionMode_left);

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
//  delay(100);
}

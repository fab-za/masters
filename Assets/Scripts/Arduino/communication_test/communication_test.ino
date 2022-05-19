#include <Servo.h>

Servo tensionMotor_left;
Servo tensionMotor_right;

int motorPin_left = 5;
int motorPin_right = 6;
unsigned long prevTime;
unsigned long readTime;
bool firstRead = true;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
//  Serial.setTimeout(200);
  
  pinMode(LED_BUILTIN, OUTPUT);

  tensionMotor_left.attach(motorPin_left);
  tensionMotor_right.attach(motorPin_right);
}

void loop() {
  // put your main code here, to run repeatedly:

//  prevTime = millis();
  String message = Serial.readString();
//  if(firstRead){
//    readTime = millis() - prevTime;
//    firstRead = false;
//    
//    char buf[50];
//    String rtime = ltoa(readTime, buf, 10);
//    
//    Serial.println(rtime);
//  }

  betweenArduinoUnity(message);
  betweenArduinoMotor(message);
}

void betweenArduinoUnity(String message){
  if(message != ""){
    digitalWrite(LED_BUILTIN, HIGH);   // turn the LED on (HIGH is the voltage level)
    delay(1000);                       // wait for a second
    digitalWrite(LED_BUILTIN, LOW);    // turn the LED off by making the voltage LOW
  }
}

void betweenArduinoMotor(String message){
  if(message != ""){
    
    tensionMotor_left.write(0);
//    tensionMotor_right.write(0);

    delay(50);

    tensionMotor_left.write(20);
//    tensionMotor_right.write(90);
  }
}

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  Serial.println("start");

  pinMode(LED_BUILTIN, OUTPUT);

  digitalWrite(LED_BUILTIN, LOW);
  delay(2000);
  digitalWrite(LED_BUILTIN, HIGH);
  delay(2000);
  digitalWrite(LED_BUILTIN, LOW);
  delay(2000);
  digitalWrite(LED_BUILTIN, HIGH);
  delay(2000);
  digitalWrite(LED_BUILTIN, LOW);
}

void loop() {
//  long inByte = Serial.parseInt();
  
//  char inByte = Serial.read();
//  Serial.print(inByte);
//  Serial.print("hi");

//  if(inByte == '2'){
//    digitalWrite(LED_BUILTIN, HIGH);
//  }
//  else{
//    digitalWrite(LED_BUILTIN, LOW);
//  }

//  if(inByte > 100){
//    digitalWrite(LED_BUILTIN, HIGH);
//  }
//  else{
//    digitalWrite(LED_BUILTIN, LOW);
//  }


  if(Serial.available() > 0){
    int inByte = Serial.read();
    digitalWrite(LED_BUILTIN, inByte);
  }
}

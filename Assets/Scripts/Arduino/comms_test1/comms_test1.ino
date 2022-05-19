bool firstRead = true;
unsigned long readTime;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  Serial.println("hello");
}

void loop() {
  // put your main code here, to run repeatedly:
  if(firstRead){
    readTime = millis();
    firstRead = false;
    
    char buf[50];
    String rtime = ltoa(readTime, buf, 10);
    
    Serial.println(rtime);
  }
  delay(5000);
}

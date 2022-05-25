
int hapticPin_left = 11;
int hapticPin_right = 10;

int inputPin_left = A0;
int inputPin_right = A1;

//-------- VARIABLES ---------------------

char buf[5];

int frequency_left = 1;
int frequency_right = 1;

long elapsedtime = 0;
//long duration = 30;
long milliduration = 2000;

unsigned long startPeriod_left = 0;
unsigned long startPeriod_right = 0;

int leftVal = LOW;
int rightVal = LOW; 

long period_left;
long period_right;

void setup() {
  pinMode(hapticPin_left, OUTPUT);
  pinMode(hapticPin_right, OUTPUT);

  pinMode(inputPin_left, INPUT);
  pinMode(inputPin_right, INPUT);

  pinMode(LED_BUILTIN, OUTPUT);

  testVibrate(hapticPin_left, 100);
  testVibrate(hapticPin_right, 100);

  Serial.begin(9600);
}

void loop() {
//  frequency_left = analogRead(inputPin_left);
//  frequency_right = analogRead(inputPin_right);


  frequency_left = Serial.read();
  digitalWrite(LED_BUILTIN, HIGH);
  delay(frequency_left);
  digitalWrite(LED_BUILTIN, LOW);
  delay(frequency_left);
  
//  analogWrite(A2, frequency_left);

  Serial.println(frequency_left);

//  long period_left = 1000/(frequency_left);
//  long period_right = 1000/(frequency_right);
//
//  vibrateBoth(period_left, period_right);

//  period_left = 1000/frequency_left;
//  period_right = 1000/frequency_right;
//  
//  if(frequency_left > 0 && frequency_right > 0){
//    vibrateBoth(period_left, period_right);
//    }
//  else{
//    digitalWrite(hapticPin_left, LOW);
//    digitalWrite(hapticPin_right, LOW);
//    }
}

// ---------- SUB FUNCTIONS ------------------------
void vibrateBoth(long period_left, long period_right){
  if((millis() - startPeriod_left) > (period_left/2)){
        
    if(leftVal == LOW){
      leftVal = HIGH;
      } 
    else{
      leftVal = LOW;
      }
    
    digitalWrite(hapticPin_left, leftVal);
    startPeriod_left = millis();
  }

  if((millis() - startPeriod_right) > (period_right/2)){
    
    if(rightVal == LOW){
      rightVal = HIGH;
      } 
    else{
      rightVal = LOW;
      }

    digitalWrite(hapticPin_right, rightVal);
    startPeriod_right = millis();
  }
}

void testVibrate(int pin, int f){
  int period = (1000/f);
  while(elapsedtime < milliduration){
    digitalWrite(pin, HIGH);
    delay(period/2);
    digitalWrite(pin, LOW);
    delay(period/2);
    elapsedtime += f;
    }
  elapsedtime = 0;
}

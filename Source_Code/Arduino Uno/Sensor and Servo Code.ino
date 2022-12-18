#include <Servo.h>
Servo sv;
int leftpos=-20,rightpos=20;
void setup() {
  // put your setup code here, to run once:
  pinMode(13,INPUT);
  pinMode(12,INPUT);
  sv.attach(8);
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  if(digitalRead(13)==HIGH)
  {
    Serial.println("a");
    sv.write(leftpos);
    delay(1500);
    sv.write(rightpos);
    delay(1500);
  }
  else if(digitalRead(12)==HIGH)
  {
    Serial.println("b");
    sv.write(leftpos);
    delay(1500);
    sv.write(rightpos);
    delay(1500);
  }
  else{
    Serial.println("0");
    delay(1500);
  }
}

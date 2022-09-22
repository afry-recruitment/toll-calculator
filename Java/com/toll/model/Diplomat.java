
public class Diplomat implements Vehicle {
  private String number;
  @Override
  public String getType() {
    return "Diplomat";
  }

  @Override
  public String getNumber() {
    return number;
  }

  @Override
  public void setNumber(String number) {
    this.number = number;
  }
}

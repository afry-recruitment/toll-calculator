package afry.tollcollector.domain;

import java.util.Collection;
import java.util.List;

public class TollFees {
  private List<TollFee> tollFees;

  public TollFees(List<TollFee> tollFees) {
    this.tollFees = tollFees;
  }

  public int getTollFees(int hour, int minute) {
    return tollFees.stream()
        .filter(t -> t.matches(hour, minute))
        .map(TollFee::tollFee)
        .sorted().findFirst()
        .orElse(0);
  }

}

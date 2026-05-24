# Legal references — Swedish congestion-tax calendar

This file is the primary-source ledger for the toll-free-day rules
implemented in `src/calendar/swedish.ts`. Everything that the JSDoc in
that file abbreviates with a citation is spelled out here, in full.

Bug [D§4.6] in discovery only observes that the reference Java code
hard-codes the year 2013 — it does **not** answer "what is the correct
rule." That answer lives here, because it is the result of legal
research, not of reading the Java source.

---

## 1. Governing statutes

### 1.1 SFS 2004:629 — *Lag om trängselskatt* (Congestion-Tax Act)

The act that establishes the congestion tax and, in **Bilaga 1**
(Annex 1), enumerates the carve-outs for the Stockholm zone: which
days are toll-free, which hours apply on the days that are taxed,
and the July clause.

- Riksdagen source:
  https://www.riksdagen.se/sv/dokument-och-lagar/dokument/svensk-forfattningssamling/lag-2004629-om-trangselskatt_sfs-2004-629/

Relevant excerpts (Bilaga 1):

> *"Skatt ska inte tas ut på lördagar, helgdagar, dagar före helgdag
> eller under juli månad. Skatt ska dock tas ut på dagen efter
> långfredag (skärtorsdagen), dagen före Kristi himmelsfärds dag och
> dagen före allhelgonadagen, samt under de fem första vardagarna
> utom lördag i juli månad."*

In plain terms:
- Saturdays, public holidays, and the day before a public holiday are
  toll-free.
- July is toll-free **except** for the first five weekdays
  (Mon–Fri) of the month.
- Three day-before-holiday cases are explicitly **taxed**:
  Maundy Thursday, Ascension Eve, All Saints' Eve.

### 1.2 SFS 1989:253 — *Lag om allmänna helgdagar* (Public Holidays Act)

Defines what counts as a *helgdag* (public holiday) in Sweden. SFS
2004:629 Bilaga 1 references "helgdag" without redefining it, so the
1989 act is the authoritative enumeration.

- Riksdagen source:
  https://www.riksdagen.se/sv/dokument-och-lagar/dokument/svensk-forfattningssamling/lag-1989253-om-allmanna-helgdagar_sfs-1989-253/

§2 enumerates the *helgdagar*:
- Söndagar (every Sunday).
- Nyårsdagen (Jan 1).
- Trettondedag jul / Epiphany (Jan 6).
- Långfredagen (Good Friday).
- Påskdagen (Easter Sunday).
- Annandag påsk (Easter Monday).
- Första maj (May 1).
- Kristi himmelsfärds dag (Ascension Day — Easter + 39 days).
- Pingstdagen (Whitsunday — Easter + 49 days).
- Sveriges nationaldag (Jun 6).
- Midsommardagen (Midsummer Day — Saturday between Jun 20 and Jun 26).
- Alla helgons dag (All Saints' Day — Saturday between Oct 31 and Nov 6).
- Juldagen (Dec 25).
- Annandag jul (Dec 26).

---

## 2. Divergences between the Java reference 2013 list and the law

The Java reference's hard-coded 2013 list was compared against the
two statutes above. Four divergences emerge:

| # | Divergence | What Java does | What the law says |
|---|------------|----------------|-------------------|
| 1 | **Epiphany missing.** | Jan 6 is not in the 2013 list. In 2013 it fell on a Sunday so the weekend rule masked it; in 2025 it falls on Monday and Java wrongly bills it. | SFS 1989:253 §2 lists *Trettondedag jul* as a *helgdag*. |
| 2 | **Day-before-holiday not a rule.** | The eve dates that appear (Apr 30, Jun 5, Dec 24, Dec 31, Nov 1) are individual hard-coded entries. Epiphany Eve (Jan 5) is missing. | SFS 2004:629 Bilaga 1 says *dag före helgdag* is toll-free, derived from the helgdag list. |
| 3 | **Three eves are taxed, not free.** | Java's 2013 list includes Mar 28 (Maundy Thursday), May 8 (Ascension Eve), and Nov 1 (All Saints' Eve) as toll-free. | SFS 2004:629 Bilaga 1 carves these out explicitly as taxable. |
| 4 | **July is fully free.** | Java treats the entire month of July as toll-free. | SFS 2004:629 Bilaga 1: *"under de fem första vardagarna utom lördag i juli månad"* — the first five non-Saturday weekdays of July are taxed. |

---

## 3. How the implementation pins to these sources

`src/calendar/swedish.ts` opens with a JSDoc citation block that names
both SFS numbers and links back to this file. Each rule in the
algorithm is annotated with the clause it derives from:

```text
1. Sunday        → SFS 1989:253 §2
2. Public holiday → SFS 1989:253 §2 enumeration
3. July clause   → SFS 2004:629 Bilaga 1
4. Day before holiday, with Maundy Thursday / Ascension Eve /
   All Saints' Eve carve-outs → SFS 2004:629 Bilaga 1
```

The test suite verifies each carve-out explicitly against dated
samples from 2013, 2024, 2025, and 2027 so that any regression — for
instance, accidentally re-introducing the Java behavior — fails a
named test rather than silently changing outputs.

---

## 4. Why this lives in `plan/`, not `discovery/`

Discovery's job is to record what the reference code *does* — the
year-2013 hard-coding is observable from reading
`Java/TollCalculator.java:77`. The reason the 2013 list itself is
also wrong — i.e. the law it *fails* to implement — required reading
Swedish statutes, not reading the reference code. That research is a
**deliverable of the plan**, so it belongs in this directory.

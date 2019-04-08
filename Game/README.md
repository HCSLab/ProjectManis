
# A Rhythm Game Prototype

#### Unity Version 2018.1.6f1

## How to Play

* Normal Round:
    * Input notes when the flickering bar is green. 
        1. Q-W-O-P: Move Right
        2. P-O-W-Q: Move Left
        3. W-O-W-O: Attack
        4. P-Q-P-Q: Accumulate Strength
* Level-up Round:
    * Simply Hit:
        1. Q for 5 extra health
        2. W for 5 extra strength
        3. E for 5 extra defense
        4. R for 5 extra luck

* Terms
    * $EffectiveAttackStrength = {max}(0,attacker.strength - receiver.defense)$
    * (Strength Doubled) $CriticalHitPossibility = luck/1000.0$
    * Each Successful Accumulate will let your next attack strength be multiplied by $2.25$


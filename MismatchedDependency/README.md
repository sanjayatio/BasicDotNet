# MismatchedDependency

## Background
We encountered a segmentation-fault despite the fact that the application:
- practically only control single prismatic motor.
- has no direct C++ reference

### Preliminary checks
- there are 380+ libraries bundled into that trivial application
- there are 3300+ (web of) references
- different versions of the same library referenced by different libraries

Taking System.Runtime as an example:

| version    | Σ dependents | remark               |
|------------|--------------|----------------------|
| v.5.0.0.0  | 246          | expected reference   |
| v.0.0.0.0  | 18           | all ms               |
| v.4.0.10.0 | 1            | serilog              |
| 4.0.20.0   | 3            | all ms               |
| 4.1.0.0    | 4            | 2 serilog, 1 castle  |
| 4.2.1.0    | 7            | all ms               |
| 4.2.2.0    | 1            | sqlite               |

Even microsoft themselves are not consistent with dependencies.

What's the impact of mismatched dependency? this is a simplistic way to test it out.

## Setup

This solutions consits of 3 projects:
- Core
- Transaction (binary-referenced to Core)
- MismatchedDependency (project-referenced to Core and Transaction)

With this setup MismatchedDependency would depends on 2 versions of Core.

The differences between the 2 versions of Core are:

| item                | old    | new   |
|---------------------|--------|-------|
| EntityTwoVersion    | 1.1    | 1.2   |
| type                | short  | long  |
| EntityOneBufferSize | 10(-2) | 9(-2) |
| EntityTwoMultiplier | 2      | 3     |

## Result

```
Trx.1,2: From Console (MissingMethodException): multiply Direct. from 1 to 3
Util.1,2: From Console from 3: multiply Direct. from 3 to 9
Trx.1,2: From Transaction (MissingMethodException): multiply Trx.Con from 1 to 2
Util.1,2: From Transaction from 2: multiply Trx.Con from 2 to 4
```

### Observations
- EntityTwoVersion and EntityOneBufferSize are taken from the newer (project-referenced) `Core`.
- EntityTwoMultiplier is taken from the older (binary-referenced through `Transaction`) `Core`.
- Attempts to invoke `entity.Member.Content` from `Trx` throws because it expects one with a different type.
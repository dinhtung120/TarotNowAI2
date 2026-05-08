# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-08T14:57:19.752Z
- Benchmark generated at (UTC): 2026-05-08T14:57:01.370Z
- Benchmark input: Frontend/test-results/benchmark/latest/tarotnow-benchmark.json
- Base origin: http://127.0.0.1:3100
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 167
- Critical pages: 167
- High pages: 0
- Medium pages: 0
- Slow requests >800ms: 278
- Slow requests 400-800ms: 474
- Request thresholds: >25 suspicious, >35 severe
- Slow request thresholds: >400ms optimize, >800ms serious

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 38.0 | 3132 | 0 | 0 | yes |
| logged-in-admin | desktop | 38 | 49.5 | 4915 | 33 | 0 | yes |
| logged-in-reader | desktop | 38 | 47.4 | 2882 | 20 | 0 | yes |
| logged-out | mobile | 9 | 38.0 | 2485 | 1 | 0 | yes |
| logged-in-admin | mobile | 38 | 50.6 | 3821 | 16 | 0 | yes |
| logged-in-reader | mobile | 35 | 48.9 | 3242 | 28 | 0 | yes |

## Route Family Coverage

| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | auth-public | 8 | 37.8 | 2685 | 3 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | 3 | 56.0 | 8300 | 1 | 0 |
| logged-in-admin | desktop | home | 1 | 40.0 | 2565 | 1 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | 4 | 55.8 | 6042 | 16 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | 8 | 50.5 | 5767 | 4 | 0 |
| logged-in-admin | desktop | reader-chat | 9 | 52.6 | 4954 | 7 | 0 |
| logged-in-admin | desktop | reading | 5 | 54.0 | 4590 | 1 | 0 |
| logged-in-admin | mobile | auth-public | 8 | 37.8 | 2699 | 3 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | 3 | 57.0 | 4848 | 1 | 0 |
| logged-in-admin | mobile | home | 1 | 40.0 | 2804 | 1 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | 4 | 56.5 | 3974 | 1 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | 8 | 54.1 | 4186 | 7 | 0 |
| logged-in-admin | mobile | reader-chat | 9 | 53.6 | 4015 | 2 | 0 |
| logged-in-admin | mobile | reading | 5 | 53.4 | 4152 | 1 | 0 |
| logged-in-reader | desktop | auth-public | 8 | 37.8 | 2592 | 6 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | 3 | 51.7 | 3115 | 0 | 0 |
| logged-in-reader | desktop | home | 1 | 40.0 | 2713 | 1 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | 4 | 51.3 | 3122 | 8 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | 8 | 50.0 | 2878 | 1 | 0 |
| logged-in-reader | desktop | reader-chat | 9 | 50.2 | 2966 | 1 | 0 |
| logged-in-reader | desktop | reading | 5 | 49.6 | 2899 | 3 | 0 |
| logged-in-reader | mobile | auth-public | 8 | 37.8 | 2600 | 8 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | 3 | 57.3 | 3866 | 0 | 0 |
| logged-in-reader | mobile | home | 1 | 41.0 | 2870 | 1 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | 4 | 51.5 | 3583 | 12 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | 8 | 52.5 | 3597 | 2 | 0 |
| logged-in-reader | mobile | reader-chat | 6 | 53.0 | 3448 | 2 | 0 |
| logged-in-reader | mobile | reading | 5 | 50.4 | 2882 | 3 | 0 |
| logged-out | desktop | auth-public | 8 | 37.8 | 3203 | 0 | 0 |
| logged-out | desktop | home | 1 | 40.0 | 2567 | 0 | 0 |
| logged-out | mobile | auth-public | 8 | 37.8 | 2485 | 1 | 0 |
| logged-out | mobile | home | 1 | 40.0 | 2487 | 0 | 0 |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 61 | 4 | 56 | 0 | 5611 | 284 | 3216 | 24.0 | 0.0122 | 0 | 0 | 1865588 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 59 | 5 | 52 | 0 | 5268 | 2308 | 3356 | 30.0 | 0.0432 | 0 | 0 | 1736755 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 58 | 5 | 51 | 0 | 6544 | 2724 | 4600 | 24.0 | 0.0042 | 0 | 0 | 1739083 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 58 | 5 | 52 | 0 | 4397 | 288 | 2444 | 15.0 | 0.0071 | 0 | 0 | 1861450 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 58 | 5 | 52 | 0 | 3401 | 224 | 1432 | 19.0 | 0.0071 | 0 | 0 | 1861623 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 58 | 5 | 52 | 0 | 5014 | 240 | 860 | 15.0 | 0.0071 | 1 | 0 | 1732706 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 58 | 5 | 52 | 0 | 4831 | 272 | 2904 | 16.0 | 0.0071 | 0 | 0 | 1749703 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 58 | 5 | 51 | 0 | 3383 | 252 | 1288 | 15.0 | 0.0892 | 0 | 0 | 1714349 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 58 | 5 | 52 | 0 | 3371 | 192 | 1408 | 17.0 | 0.0071 | 0 | 0 | 1749645 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 58 | 5 | 52 | 0 | 4291 | 400 | 2376 | 20.0 | 0.0071 | 0 | 0 | 1735176 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 57 | 4 | 52 | 0 | 4968 | 1956 | 3152 | 20.0 | 0.0042 | 0 | 0 | 1860092 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 57 | 4 | 52 | 0 | 5919 | 1500 | 4040 | 22.0 | 0.0042 | 0 | 0 | 1860459 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 57 | 4 | 52 | 0 | 8841 | 2176 | 2176 | 43.0 | 0.0042 | 9 | 0 | 1731427 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 57 | 5 | 51 | 0 | 5192 | 1856 | 2840 | 22.0 | 0.0489 | 1 | 0 | 1712078 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 57 | 4 | 52 | 0 | 12833 | 1224 | 1860 | 424.0 | 0.0000 | 0 | 0 | 1722031 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/e85df6bc-7785-45bd-bde4-c2c1b178fd6f | 57 | 4 | 52 | 0 | 3992 | 296 | 2112 | 33.0 | 0.0042 | 0 | 0 | 1782596 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 57 | 4 | 51 | 0 | 3312 | 372 | 984 | 33.0 | 0.0726 | 0 | 0 | 1712648 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 57 | 5 | 51 | 0 | 3988 | 272 | 1844 | 15.0 | 0.0760 | 1 | 0 | 1709587 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 57 | 4 | 51 | 0 | 3648 | 192 | 1664 | 15.0 | 0.0071 | 0 | 0 | 1737650 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 57 | 4 | 51 | 0 | 4989 | 224 | 3020 | 15.0 | 0.0071 | 0 | 0 | 1737614 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 57 | 4 | 51 | 0 | 4136 | 256 | 2340 | 14.0 | 0.0071 | 0 | 0 | 1737731 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 56 | 5 | 50 | 0 | 3652 | 304 | 1744 | 31.0 | 0.0042 | 1 | 0 | 1710741 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 56 | 5 | 50 | 0 | 3540 | 368 | 1664 | 33.0 | 0.0042 | 1 | 0 | 1710514 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 56 | 5 | 50 | 0 | 3929 | 244 | 2356 | 327.0 | 0.0071 | 0 | 0 | 1710954 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 56 | 5 | 50 | 0 | 5214 | 220 | 3252 | 18.0 | 0.0071 | 0 | 0 | 1711346 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 56 | 5 | 50 | 0 | 3703 | 244 | 2192 | 14.0 | 0.0071 | 1 | 0 | 1710254 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 56 | 1 | 54 | 0 | 3937 | 272 | 1200 | 17.0 | 0.0127 | 0 | 0 | 1842943 |
| Critical | reading | logged-in-admin | desktop | /vi/reading | 55 | 5 | 49 | 0 | 5252 | 1520 | 3312 | 16.0 | 0.0042 | 0 | 0 | 1720727 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 55 | 4 | 50 | 0 | 12358 | 2780 | 10552 | 51.0 | 0.0042 | 0 | 0 | 1708290 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/286ea3a6-3336-45a4-942a-6ab6913fdf83 | 55 | 4 | 50 | 0 | 6390 | 2740 | 4548 | 32.0 | 0.0042 | 0 | 0 | 1711910 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/3875f179-6a01-4f7a-a063-419cba05b5e3 | 55 | 4 | 50 | 0 | 4549 | 296 | 2704 | 33.0 | 0.0051 | 0 | 0 | 1711911 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 55 | 4 | 50 | 0 | 4094 | 508 | 2012 | 21.0 | 0.0071 | 0 | 0 | 1708343 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/chat | 55 | 4 | 50 | 0 | 3365 | 244 | 1676 | 17.0 | 0.0071 | 1 | 0 | 1709757 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/89e93f59-4652-4062-a605-8f64fbf724ac | 55 | 4 | 50 | 0 | 5015 | 208 | 3072 | 19.0 | 0.0072 | 0 | 0 | 1711952 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 55 | 4 | 50 | 0 | 4866 | 232 | 2900 | 15.0 | 0.0071 | 0 | 0 | 1709641 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 55 | 4 | 50 | 0 | 5280 | 188 | 3292 | 16.0 | 0.0071 | 0 | 0 | 1708448 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers | 55 | 4 | 50 | 0 | 3910 | 256 | 1972 | 13.0 | 0.0071 | 2 | 0 | 1710819 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 55 | 5 | 49 | 0 | 3658 | 172 | 1736 | 15.0 | 0.0401 | 0 | 0 | 1710598 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 54 | 3 | 49 | 0 | 8444 | 4324 | 5268 | 84.0 | 0.0042 | 4 | 0 | 1714583 |
| Critical | reading | logged-in-admin | mobile | /vi/reading | 54 | 4 | 49 | 0 | 4059 | 236 | 2356 | 16.0 | 0.0071 | 0 | 0 | 1718925 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 54 | 4 | 49 | 0 | 5022 | 200 | 1428 | 11.0 | 0.0760 | 3 | 0 | 1707420 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 54 | 4 | 49 | 0 | 4182 | 236 | 2368 | 16.0 | 0.0071 | 0 | 0 | 1709863 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 54 | 4 | 49 | 0 | 5219 | 280 | 3272 | 14.0 | 0.0071 | 0 | 0 | 1707238 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 54 | 4 | 49 | 0 | 3582 | 192 | 1584 | 18.0 | 0.0071 | 1 | 0 | 1705678 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/history | 54 | 4 | 49 | 0 | 3513 | 232 | 1572 | 15.0 | 0.0071 | 1 | 0 | 1708254 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/a38d5a6f-c4b4-4041-a8d3-9054c1772885 | 54 | 3 | 50 | 0 | 4057 | 192 | 2036 | 18.0 | 0.0096 | 0 | 0 | 1740718 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 52 | 1 | 50 | 0 | 4439 | 1756 | 2580 | 22.0 | 0.0042 | 7 | 0 | 1708938 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 52 | 1 | 50 | 0 | 6798 | 3184 | 5204 | 46.0 | 0.0042 | 1 | 0 | 1709859 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 52 | 1 | 49 | 0 | 9110 | 5720 | 7216 | 63.0 | 0.0042 | 0 | 0 | 1714990 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 52 | 1 | 50 | 0 | 3052 | 396 | 1420 | 31.0 | 0.0039 | 0 | 0 | 1838025 |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 52 | 1 | 50 | 0 | 2854 | 252 | 1152 | 26.0 | 0.0177 | 0 | 0 | 1726266 |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 52 | 1 | 50 | 0 | 2871 | 308 | 976 | 27.0 | 0.0430 | 0 | 0 | 1711528 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/41f77e30-7090-4298-bcf3-25d046d7ce48 | 52 | 1 | 50 | 0 | 2864 | 280 | 1008 | 29.0 | 0.0039 | 2 | 0 | 1760069 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 52 | 1 | 49 | 0 | 3242 | 320 | 1388 | 31.0 | 0.0039 | 0 | 0 | 1715206 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 52 | 1 | 49 | 0 | 2828 | 284 | 988 | 28.0 | 0.0000 | 0 | 0 | 1715171 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 52 | 1 | 50 | 0 | 3085 | 236 | 1140 | 19.0 | 0.0071 | 0 | 0 | 1708862 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 52 | 1 | 50 | 0 | 4103 | 288 | 2352 | 15.0 | 0.0071 | 1 | 0 | 1710826 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 52 | 1 | 49 | 0 | 3954 | 796 | 2008 | 14.0 | 0.0071 | 1 | 0 | 1715005 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 52 | 1 | 50 | 0 | 2853 | 208 | 928 | 17.0 | 0.0071 | 2 | 0 | 1837410 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 52 | 1 | 50 | 0 | 5857 | 180 | 180 | 16.0 | 0.0072 | 9 | 0 | 1707929 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 52 | 1 | 49 | 0 | 3096 | 184 | 1144 | 16.0 | 0.0071 | 0 | 0 | 1716084 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 52 | 1 | 49 | 0 | 3149 | 240 | 1184 | 15.0 | 0.0071 | 0 | 0 | 1715234 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 52 | 1 | 49 | 0 | 3092 | 196 | 1120 | 15.0 | 0.0071 | 0 | 0 | 1715150 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 51 | 0 | 50 | 0 | 2771 | 284 | 936 | 27.0 | 0.0039 | 0 | 0 | 1708059 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 51 | 0 | 50 | 0 | 2794 | 320 | 1008 | 27.0 | 0.0039 | 0 | 0 | 1835301 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 51 | 0 | 50 | 0 | 3872 | 324 | 904 | 28.0 | 0.0040 | 8 | 0 | 1706893 |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 51 | 0 | 50 | 0 | 3620 | 280 | 1016 | 25.0 | 0.0039 | 0 | 0 | 1709192 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 51 | 0 | 49 | 0 | 2800 | 268 | 976 | 24.0 | 0.0039 | 0 | 0 | 1715212 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 51 | 0 | 49 | 0 | 2864 | 368 | 928 | 30.0 | 0.0039 | 0 | 0 | 1714188 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 51 | 0 | 50 | 0 | 2775 | 236 | 836 | 14.0 | 0.0071 | 1 | 0 | 1708011 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 51 | 0 | 50 | 0 | 2847 | 276 | 916 | 13.0 | 0.0071 | 0 | 0 | 1835249 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | 51 | 3 | 47 | 0 | 2899 | 272 | 1120 | 17.0 | 0.0071 | 1 | 0 | 1690205 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/f5383f64-a3df-40a8-9111-f378e7882969 | 51 | 1 | 49 | 0 | 2822 | 200 | 856 | 14.0 | 0.0071 | 1 | 0 | 1728410 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/f9aad5e4-fb38-4683-98fe-a239e6e9a0ff | 51 | 1 | 49 | 0 | 2923 | 240 | 952 | 15.0 | 0.0071 | 1 | 0 | 1728488 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/chat | 50 | 1 | 48 | 0 | 2793 | 276 | 964 | 27.0 | 0.0042 | 1 | 0 | 1686615 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 50 | 1 | 48 | 0 | 2873 | 252 | 1688 | 28.0 | 0.0039 | 1 | 0 | 1686768 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers | 50 | 1 | 48 | 0 | 2866 | 340 | 996 | 29.0 | 0.0039 | 0 | 0 | 1688223 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 50 | 1 | 48 | 0 | 2806 | 284 | 1412 | 322.0 | 0.0039 | 0 | 0 | 1687312 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/34455585-a481-4e01-b30e-91968f321448 | 50 | 1 | 48 | 0 | 2806 | 280 | 960 | 25.0 | 0.0039 | 0 | 0 | 1689490 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | 50 | 1 | 48 | 0 | 3258 | 272 | 1300 | 18.0 | 0.0071 | 0 | 0 | 1688008 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 50 | 1 | 48 | 0 | 3376 | 236 | 1424 | 15.0 | 0.0071 | 0 | 0 | 1686257 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/1bf8256e-c319-4561-9448-e703620450dd | 50 | 1 | 48 | 0 | 4116 | 200 | 2352 | 15.0 | 0.0072 | 0 | 0 | 1689326 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 50 | 1 | 48 | 0 | 3462 | 300 | 1512 | 15.0 | 0.0071 | 0 | 0 | 1687132 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/chat | 50 | 1 | 48 | 0 | 3303 | 344 | 1344 | 18.0 | 0.0071 | 0 | 0 | 1686784 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 50 | 1 | 48 | 0 | 3088 | 192 | 1140 | 199.0 | 0.0071 | 0 | 0 | 1687217 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/715caad8-e58a-405f-a14e-8ab680bc428c | 50 | 1 | 48 | 0 | 2860 | 232 | 912 | 18.0 | 0.0072 | 0 | 0 | 1689550 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 49 | 1 | 47 | 0 | 5202 | 328 | 1404 | 20.0 | 0.0489 | 2 | 0 | 1684606 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | 49 | 0 | 48 | 0 | 2754 | 256 | 888 | 30.0 | 0.0042 | 0 | 0 | 1687108 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 49 | 1 | 47 | 0 | 4460 | 1904 | 2640 | 25.0 | 0.0042 | 0 | 0 | 1684781 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 49 | 1 | 47 | 0 | 4912 | 384 | 1592 | 22.0 | 0.0042 | 1 | 0 | 1684754 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 49 | 0 | 48 | 0 | 4820 | 2316 | 3468 | 270.0 | 0.0042 | 0 | 0 | 1685812 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 49 | 0 | 48 | 0 | 4703 | 2196 | 2860 | 29.0 | 0.0042 | 0 | 0 | 1685219 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 49 | 0 | 48 | 0 | 3042 | 576 | 1188 | 28.0 | 0.0042 | 0 | 0 | 1686014 |
| Critical | reading | logged-in-reader | desktop | /vi/reading | 49 | 1 | 47 | 0 | 2975 | 320 | 1140 | 32.0 | 0.0039 | 1 | 0 | 1696380 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 49 | 0 | 48 | 0 | 2801 | 352 | 956 | 27.0 | 0.0039 | 0 | 0 | 1684779 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/chat | 49 | 0 | 48 | 0 | 2790 | 292 | 1004 | 35.0 | 0.0039 | 1 | 0 | 1685683 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 49 | 1 | 47 | 0 | 2805 | 280 | 968 | 27.0 | 0.0039 | 0 | 0 | 1684830 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 49 | 1 | 47 | 0 | 2840 | 304 | 1000 | 26.0 | 0.0095 | 0 | 0 | 1686238 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/8c5b5df3-cb2e-4e35-a19b-c69e251c62b1 | 49 | 0 | 48 | 0 | 2803 | 312 | 952 | 29.0 | 0.0039 | 0 | 0 | 1688497 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 49 | 0 | 48 | 0 | 3762 | 1276 | 1912 | 24.0 | 0.0039 | 0 | 0 | 1685995 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 49 | 0 | 48 | 0 | 2777 | 292 | 936 | 24.0 | 0.0039 | 0 | 0 | 1686195 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 49 | 0 | 48 | 0 | 2764 | 300 | 908 | 27.0 | 0.0039 | 0 | 0 | 1686154 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 49 | 1 | 47 | 0 | 3472 | 292 | 1352 | 12.0 | 0.0071 | 2 | 0 | 1683935 |
| Critical | reading | logged-in-reader | mobile | /vi/reading | 49 | 1 | 47 | 0 | 2905 | 240 | 964 | 17.0 | 0.0071 | 0 | 0 | 1696410 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 49 | 1 | 47 | 0 | 3056 | 268 | 1116 | 20.0 | 0.0073 | 0 | 0 | 1687233 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 49 | 1 | 47 | 0 | 3094 | 224 | 1144 | 15.0 | 0.0071 | 0 | 0 | 1684785 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 48 | 0 | 47 | 0 | 4654 | 2116 | 2860 | 27.0 | 0.0042 | 0 | 0 | 1686142 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 48 | 0 | 47 | 0 | 4535 | 2068 | 2716 | 25.0 | 0.0042 | 0 | 0 | 1684549 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/history | 48 | 0 | 47 | 0 | 2769 | 308 | 1004 | 30.0 | 0.0042 | 1 | 0 | 1685414 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 48 | 0 | 47 | 0 | 2813 | 292 | 940 | 29.0 | 0.0096 | 0 | 0 | 1686251 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 48 | 0 | 47 | 0 | 2776 | 284 | 932 | 26.0 | 0.0039 | 0 | 0 | 1683949 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | 48 | 0 | 47 | 0 | 3049 | 604 | 1236 | 38.0 | 0.0039 | 0 | 0 | 1685579 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 48 | 0 | 47 | 0 | 3514 | 1148 | 1564 | 16.0 | 0.0000 | 1 | 0 | 1683988 |
| Critical | auth-public | logged-in-reader | mobile | /vi | 41 | 0 | 40 | 0 | 2870 | 352 | 920 | 18.0 | 0.0055 | 1 | 0 | 1435884 |
| Critical | auth-public | logged-out | desktop | /vi | 40 | 0 | 39 | 0 | 2567 | 256 | 764 | 114.0 | 0.0000 | 0 | 0 | 1396153 |
| Critical | auth-public | logged-in-admin | desktop | /vi | 40 | 0 | 39 | 0 | 2565 | 324 | 880 | 137.0 | 0.0040 | 1 | 0 | 1397014 |
| Critical | auth-public | logged-in-reader | desktop | /vi | 40 | 0 | 39 | 0 | 2713 | 344 | 1132 | 475.0 | 0.0038 | 1 | 0 | 1396973 |
| Critical | auth-public | logged-out | mobile | /vi | 40 | 0 | 39 | 0 | 2487 | 220 | 588 | 19.0 | 0.0000 | 0 | 0 | 1396184 |
| Critical | auth-public | logged-in-admin | mobile | /vi | 40 | 0 | 39 | 0 | 2804 | 544 | 900 | 15.0 | 0.0055 | 1 | 0 | 1397000 |
| Critical | auth-public | logged-out | desktop | /vi/legal/tos | 39 | 0 | 38 | 0 | 3293 | 1048 | 1360 | 17.0 | 0.0000 | 0 | 0 | 1371741 |
| Critical | auth-public | logged-out | desktop | /vi/legal/privacy | 39 | 0 | 38 | 0 | 3250 | 992 | 1340 | 11.0 | 0.0000 | 0 | 0 | 1371791 |
| Critical | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 39 | 0 | 38 | 0 | 3311 | 1028 | 1356 | 11.0 | 0.0000 | 0 | 0 | 1372107 |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/tos | 39 | 0 | 38 | 0 | 2566 | 256 | 672 | 24.0 | 0.0020 | 0 | 0 | 1372436 |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 39 | 0 | 38 | 0 | 2624 | 404 | 680 | 28.0 | 0.0020 | 0 | 0 | 1372481 |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 39 | 0 | 38 | 0 | 2543 | 272 | 652 | 24.0 | 0.0020 | 0 | 0 | 1372813 |
| Critical | auth-public | logged-in-reader | desktop | /vi/legal/tos | 39 | 0 | 38 | 0 | 2578 | 324 | 668 | 25.0 | 0.0019 | 0 | 0 | 1372470 |
| Critical | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 39 | 0 | 38 | 0 | 2765 | 480 | 836 | 36.0 | 0.0019 | 0 | 0 | 1372541 |
| Critical | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 39 | 0 | 38 | 0 | 2561 | 304 | 624 | 24.0 | 0.0019 | 0 | 0 | 1372870 |
| Critical | auth-public | logged-out | mobile | /vi/legal/tos | 39 | 0 | 38 | 0 | 2480 | 184 | 532 | 14.0 | 0.0000 | 0 | 0 | 1371686 |
| Critical | auth-public | logged-out | mobile | /vi/legal/privacy | 39 | 0 | 38 | 0 | 2584 | 304 | 624 | 10.0 | 0.0000 | 0 | 0 | 1371769 |
| Critical | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 39 | 0 | 38 | 0 | 2473 | 232 | 556 | 13.0 | 0.0000 | 0 | 0 | 1371795 |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/tos | 39 | 0 | 38 | 0 | 2828 | 264 | 848 | 23.0 | 0.0055 | 0 | 0 | 1372505 |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 39 | 0 | 38 | 0 | 2854 | 280 | 888 | 14.0 | 0.0055 | 0 | 0 | 1372486 |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 39 | 0 | 38 | 0 | 3427 | 220 | 1456 | 14.0 | 0.0055 | 0 | 0 | 1372871 |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/tos | 39 | 0 | 38 | 0 | 2734 | 392 | 752 | 11.0 | 0.0032 | 0 | 0 | 1372523 |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 39 | 0 | 38 | 0 | 2582 | 260 | 620 | 13.0 | 0.0032 | 0 | 0 | 1372578 |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 39 | 0 | 38 | 0 | 2535 | 232 | 556 | 13.0 | 0.0032 | 0 | 0 | 1372855 |
| Critical | auth-public | logged-out | desktop | /vi/login | 37 | 0 | 36 | 0 | 3044 | 840 | 1160 | 7.0 | 0.0000 | 0 | 0 | 1349780 |
| Critical | auth-public | logged-out | desktop | /vi/register | 37 | 0 | 36 | 0 | 3211 | 956 | 1284 | 14.0 | 0.0000 | 0 | 0 | 1350309 |
| Critical | auth-public | logged-out | desktop | /vi/forgot-password | 37 | 0 | 36 | 0 | 2961 | 736 | 1060 | 10.0 | 0.0000 | 0 | 0 | 1349337 |
| Critical | auth-public | logged-out | desktop | /vi/reset-password | 37 | 0 | 36 | 0 | 3351 | 1044 | 1044 | 20.0 | 0.0000 | 0 | 0 | 1348418 |
| Critical | auth-public | logged-out | desktop | /vi/verify-email | 37 | 0 | 36 | 0 | 3199 | 952 | 952 | 7.0 | 0.0000 | 0 | 0 | 1348524 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | 37 | 0 | 36 | 0 | 2429 | 264 | 808 | 219.0 | 0.0040 | 2 | 0 | 1349393 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | 37 | 0 | 36 | 0 | 2416 | 244 | 760 | 33.0 | 0.0035 | 1 | 0 | 1349357 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | 37 | 0 | 36 | 0 | 2407 | 212 | 776 | 212.0 | 0.0040 | 0 | 0 | 1349398 |
| Critical | auth-public | logged-in-admin | desktop | /vi/reset-password | 37 | 0 | 36 | 0 | 2417 | 216 | 216 | 14.0 | 0.0000 | 0 | 0 | 1348450 |
| Critical | auth-public | logged-in-admin | desktop | /vi/verify-email | 37 | 0 | 36 | 0 | 4077 | 1668 | 1668 | 42.0 | 0.0000 | 0 | 0 | 1348432 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | 37 | 0 | 36 | 0 | 2547 | 336 | 876 | 157.0 | 0.0038 | 2 | 0 | 1349352 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | 37 | 0 | 36 | 0 | 2518 | 292 | 976 | 73.0 | 0.0038 | 2 | 0 | 1349405 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | 37 | 0 | 36 | 0 | 2541 | 300 | 932 | 323.0 | 0.0039 | 2 | 0 | 1349430 |
| Critical | auth-public | logged-in-reader | desktop | /vi/reset-password | 37 | 0 | 36 | 0 | 2610 | 332 | 332 | 28.0 | 0.0000 | 0 | 0 | 1348441 |
| Critical | auth-public | logged-in-reader | desktop | /vi/verify-email | 37 | 0 | 36 | 0 | 2618 | 388 | 388 | 30.0 | 0.0000 | 0 | 0 | 1348414 |
| Critical | auth-public | logged-out | mobile | /vi/login | 37 | 0 | 36 | 0 | 2450 | 176 | 508 | 19.0 | 0.0000 | 0 | 0 | 1349700 |
| Critical | auth-public | logged-out | mobile | /vi/register | 37 | 0 | 36 | 0 | 2490 | 184 | 524 | 15.0 | 0.0000 | 1 | 0 | 1350210 |
| Critical | auth-public | logged-out | mobile | /vi/forgot-password | 37 | 0 | 36 | 0 | 2466 | 192 | 520 | 12.0 | 0.0000 | 0 | 0 | 1349322 |
| Critical | auth-public | logged-out | mobile | /vi/reset-password | 37 | 0 | 36 | 0 | 2458 | 216 | 216 | 16.0 | 0.0000 | 0 | 0 | 1348302 |
| Critical | auth-public | logged-out | mobile | /vi/verify-email | 37 | 0 | 36 | 0 | 2478 | 184 | 184 | 14.0 | 0.0000 | 0 | 0 | 1348527 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | 37 | 0 | 36 | 0 | 2431 | 220 | 580 | 14.0 | 0.0055 | 1 | 0 | 1349409 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | 37 | 0 | 36 | 0 | 2446 | 236 | 604 | 12.0 | 0.0055 | 2 | 0 | 1349336 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | 37 | 0 | 36 | 0 | 2526 | 412 | 764 | 14.0 | 0.0055 | 0 | 0 | 1349401 |
| Critical | auth-public | logged-in-admin | mobile | /vi/reset-password | 37 | 0 | 36 | 0 | 2523 | 256 | 256 | 17.0 | 0.0000 | 0 | 0 | 1348304 |
| Critical | auth-public | logged-in-admin | mobile | /vi/verify-email | 37 | 0 | 36 | 0 | 2554 | 240 | 240 | 15.0 | 0.0000 | 0 | 0 | 1348422 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | 37 | 0 | 36 | 0 | 2549 | 340 | 908 | 12.0 | 0.0055 | 2 | 0 | 1349329 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | 37 | 0 | 36 | 0 | 2558 | 404 | 960 | 12.0 | 0.0024 | 3 | 0 | 1349381 |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | 37 | 0 | 36 | 0 | 2551 | 292 | 896 | 12.0 | 0.0024 | 3 | 0 | 1349405 |
| Critical | auth-public | logged-in-reader | mobile | /vi/reset-password | 37 | 0 | 36 | 0 | 2637 | 300 | 300 | 18.0 | 0.0000 | 0 | 0 | 1348326 |
| Critical | auth-public | logged-in-reader | mobile | /vi/verify-email | 37 | 0 | 36 | 0 | 2653 | 380 | 380 | 12.0 | 0.0000 | 0 | 0 | 1348451 |

## Major Issues Found

- Critical: 167 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: chưa phát hiện page High theo benchmark hiện tại.
- Medium: chưa phát hiện page Medium theo benchmark hiện tại.
- Duplicate: 19 nhóm duplicate request cần kiểm tra over-fetch/cache key.
- Pending: 49 page(s) có pending request không phải websocket/eventsource.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | GET | 200 | 10625 | 2386 | html | http://127.0.0.1:3100/vi/leaderboard |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10315 | 2655 | html | http://127.0.0.1:3100/vi/profile/mfa |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7037 | 5287 | html | http://127.0.0.1:3100/vi/readers/69e7b9fa5b238c89fadb3a0b |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 5131 | 3580 | html | http://127.0.0.1:3100/vi/readers/69dbe86b052d3c8f3f55e231 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 4509 | 2614 | html | http://127.0.0.1:3100/vi/readers/69d93c24bc68b27090414f6c |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/286ea3a6-3336-45a4-942a-6ab6913fdf83 | GET | 200 | 4380 | 2601 | html | http://127.0.0.1:3100/vi/reading/session/286ea3a6-3336-45a4-942a-6ab6913fdf83 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3964 | 689 | api | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3939 | 210 | api | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | GET | 200 | 3911 | 3041 | html | http://127.0.0.1:3100/vi/community |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3844 | 804 | api | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | GET | 200 | 3843 | 1408 | html | http://127.0.0.1:3100/vi/gacha/history |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3813 | 648 | api | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3638 | 95 | api | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 3636 | 3632 | api | http://127.0.0.1:3100/api/v1/reading/cards-catalog/chunks/0?v=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | POST | 200 | 3545 | 3540 | api | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3471 | 675 | api | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | GET | 200 | 3244 | 132 | html | http://127.0.0.1:3100/vi/profile/mfa |
| Critical | reading | logged-in-admin | desktop | /vi/reading | GET | 200 | 3224 | 1398 | html | http://127.0.0.1:3100/vi/reading |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 3218 | 2155 | html | http://127.0.0.1:3100/vi/gamification |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | GET | 200 | 3203 | 226 | html | http://127.0.0.1:3100/vi/wallet/deposit |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 3198 | 141 | html | http://127.0.0.1:3100/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | GET | 200 | 3190 | 2443 | html | http://127.0.0.1:3100/vi/profile/reader |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3077 | 2064 | html | http://127.0.0.1:3100/vi/collection |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3071 | 376 | api | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F08_The_Chariot_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3033 | 2140 | api | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/89e93f59-4652-4062-a605-8f64fbf724ac | GET | 200 | 2999 | 146 | html | http://127.0.0.1:3100/vi/reading/session/89e93f59-4652-4062-a605-8f64fbf724ac |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | GET | 200 | 2989 | 944 | html | http://127.0.0.1:3100/vi/profile/reader |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 2970 | 160 | html | http://127.0.0.1:3100/vi/readers/69e7b9fa5b238c89fadb3a0b |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | GET | 200 | 2956 | 1830 | html | http://127.0.0.1:3100/vi/gacha |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 2949 | 2945 | api | http://127.0.0.1:3100/api/auth/session |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2936 | 465 | api | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | GET | 200 | 2901 | 1943 | html | http://127.0.0.1:3100/vi/wallet/withdraw |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 2845 | 169 | html | http://127.0.0.1:3100/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | GET | 200 | 2816 | 217 | html | http://127.0.0.1:3100/vi/leaderboard |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | GET | 200 | 2807 | 2210 | html | http://127.0.0.1:3100/vi/notifications |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | GET | 200 | 2752 | 205 | html | http://127.0.0.1:3100/vi/community |
| Critical | reader-chat | logged-in-admin | desktop | /vi/reader/apply | GET | 200 | 2684 | 2057 | html | http://127.0.0.1:3100/vi/reader/apply |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | GET | 200 | 2640 | 1985 | html | http://127.0.0.1:3100/vi/wallet |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | GET | 200 | 2583 | 1752 | html | http://127.0.0.1:3100/vi/profile |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2577 | 780 | api | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/3875f179-6a01-4f7a-a063-419cba05b5e3 | GET | 200 | 2539 | 162 | html | http://127.0.0.1:3100/vi/reading/session/3875f179-6a01-4f7a-a063-419cba05b5e3 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 2494 | 1919 | html | http://127.0.0.1:3100/vi/wallet/deposit/history |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | GET | 200 | 2449 | 1789 | html | http://127.0.0.1:3100/vi/wallet/deposit |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 2418 | 1638 | html | http://127.0.0.1:3100/vi/inventory |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 2377 | 234 | html | http://127.0.0.1:3100/vi/gacha |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/1bf8256e-c319-4561-9448-e703620450dd | GET | 200 | 2327 | 2322 | api | http://127.0.0.1:3100/api/auth/session |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | GET | 200 | 2269 | 317 | html | http://127.0.0.1:3100/vi/gamification |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 2147 | 2134 | api | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 2108 | 200 | html | http://127.0.0.1:3100/vi/readers/69e7b9fa5b238c89fadb3a0b |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | GET | 200 | 2090 | 228 | html | http://127.0.0.1:3100/vi/gamification |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | GET | 200 | 2090 | 170 | html | http://127.0.0.1:3100/vi/wallet |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/1bf8256e-c319-4561-9448-e703620450dd | GET | 200 | 2090 | 140 | html | http://127.0.0.1:3100/vi/reading/session/1bf8256e-c319-4561-9448-e703620450dd |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 2056 | 185 | html | http://127.0.0.1:3100/vi/collection |
| Critical | reading | logged-in-admin | mobile | /vi/reading | GET | 200 | 2046 | 192 | html | http://127.0.0.1:3100/vi/reading |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/a38d5a6f-c4b4-4041-a8d3-9054c1772885 | GET | 200 | 1960 | 135 | html | http://127.0.0.1:3100/vi/reading/session/a38d5a6f-c4b4-4041-a8d3-9054c1772885 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/e85df6bc-7785-45bd-bde4-c2c1b178fd6f | GET | 200 | 1957 | 195 | html | http://127.0.0.1:3100/vi/reading/session/e85df6bc-7785-45bd-bde4-c2c1b178fd6f |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1939 | 736 | html | http://127.0.0.1:3100/vi/readers/69d93c24bc68b27090414f6c |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | GET | 200 | 1917 | 431 | html | http://127.0.0.1:3100/vi/profile/mfa |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | GET | 200 | 1910 | 191 | html | http://127.0.0.1:3100/vi/notifications |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1908 | 1757 | api | http://127.0.0.1:3100/api/auth/session?mode=lite |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers | GET | 200 | 1888 | 169 | html | http://127.0.0.1:3100/vi/readers |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/89e93f59-4652-4062-a605-8f64fbf724ac | POST | 200 | 1841 | 1836 | api | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | POST | 200 | 1783 | 1608 | api | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | GET | 200 | 1780 | 211 | html | http://127.0.0.1:3100/vi/profile |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | POST | 200 | 1772 | 1200 | api | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/34455585-a481-4e01-b30e-91968f321448 | GET | 200 | 1765 | 1758 | api | http://127.0.0.1:3100/api/reading/cards-catalog |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1759 | 36 | static | http://127.0.0.1:3100/_next/static/chunks/node_modules_0huvsdh._.js |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 1747 | 1149 | html | http://127.0.0.1:3100/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/e85df6bc-7785-45bd-bde4-c2c1b178fd6f | GET | 200 | 1737 | 1729 | api | http://127.0.0.1:3100/api/reading/cards-catalog |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | GET | 200 | 1719 | 1711 | api | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | GET | 200 | 1705 | 1698 | api | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1694 | 1688 | api | http://127.0.0.1:3100/api/auth/session |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | GET | 200 | 1689 | 170 | html | http://127.0.0.1:3100/vi/profile/reader |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 1685 | 1676 | api | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 1642 | 180 | html | http://127.0.0.1:3100/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 1641 | 128 | html | http://127.0.0.1:3100/vi/wallet/withdraw |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | POST | 200 | 1637 | 1634 | api | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1633 | 129 | html | http://127.0.0.1:3100/vi/readers/69dbe86b052d3c8f3f55e231 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1615 | 1422 | api | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1594 | 78 | static | http://127.0.0.1:3100/_next/static/chunks/node_modules_micromark-core-commonmark_dev_lib_13dj~6i._.js |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| High | reading | logged-in-admin | desktop | /vi/reading | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 2 | POST http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| Critical | auth-public | logged-in-admin | desktop | /vi | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | http://127.0.0.1:3100/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Ffree_draw_ticket_50_20260416_165452.avif&w=48&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | http://127.0.0.1:3100/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fdefense_booster_50_20260416_165452.avif&w=48&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | http://127.0.0.1:3100/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fexp_booster_50_20260416_165452.avif&w=48&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | http://127.0.0.1:3100/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=48&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | http://127.0.0.1:3100/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=48&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F16_The_Devil_50_20260325_181357.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/chat | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/history | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | http://127.0.0.1:3100/api/auth/session |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | http://127.0.0.1:3100/_next/static/chunks/node_modules_%40microsoft_signalr_dist_esm_03~bvgi._.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | http://127.0.0.1:3100/_next/static/chunks/node_modules_%40microsoft_signalr_dist_esm_index_0._6b2c.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | http://127.0.0.1:3100/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | http://127.0.0.1:3100/api/readers?page=1&pageSize=4 |
| Critical | reading | logged-in-reader | desktop | /vi/reading | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/chat | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/41f77e30-7090-4298-bcf3-25d046d7ce48 | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/41f77e30-7090-4298-bcf3-25d046d7ce48 | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-out | mobile | /vi/register | http://127.0.0.1:3100/api/legal/runtime-policies |
| Critical | auth-public | logged-in-admin | mobile | /vi | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | http://127.0.0.1:3100/api/readers?page=1&pageSize=4 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | http://127.0.0.1:3100/api/v1/reading/cards-catalog/manifest |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/chat | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | http://127.0.0.1:3100/vi/gamification |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/history | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | http://127.0.0.1:3100/api/me/runtime-policies |
| Critical | auth-public | logged-in-reader | mobile | /vi | http://127.0.0.1:3100/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | http://127.0.0.1:3100/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | http://127.0.0.1:3100/api/wallet/balance |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | http://127.0.0.1:3100/vi |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | http://127.0.0.1:3100/api/wallet/balance |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | http://127.0.0.1:3100/vi |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | http://127.0.0.1:3100/api/gacha/history?page=1&pageSize=6 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | http://127.0.0.1:3100/api/v1/reading/cards-catalog/details/1?v=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | http://127.0.0.1:3100/api/v1/reading/cards-catalog/details/2?v=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | http://127.0.0.1:3100/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers | http://127.0.0.1:3100/api/readers?page=1&pageSize=12&specialty=&status=&searchTerm= |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/f5383f64-a3df-40a8-9111-f378e7882969 | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/f9aad5e4-fb38-4683-98fe-a239e6e9a0ff | http://127.0.0.1:3100/api/v1/presence/negotiate?negotiateVersion=1 |

## Optimization Plan

1. Fix shared Critical issues first: auth/session loops, layout-level fetch churn, failed requests, pending non-persistent requests.
2. Fix feature Critical/High issues next, starting with the feature that has the most affected page-scenario combinations.
3. Fix duplicate API calls by inspecting query keys, staleTime, refetch triggers, and parent/child component fetch ownership.
4. Fix image/cache issues by checking Next Image usage, remote patterns, dimensions, lazy/eager strategy, and modal reopen behavior.
5. Re-run the affected feature benchmark after every hotspot fix, then run full matrix before final deploy validation.

## Recommended Refactors

- Middleware/session: inspect only if report shows session API churn, handshake redirects, or auth-related duplicate requests.
- TanStack Query: inspect feature hooks whose API endpoints appear in duplicate request candidates.
- App Router layouts: inspect layout/provider boundaries if multiple unrelated features share the same duplicate or slow request pattern.
- Custom hooks: inspect effects only when a route shows repeated interaction or post-load requests.
- Image loading: inspect collection/gacha/community routes when slow static/image requests dominate.
- Route prefetch: inspect Link usage only when benchmark shows route-navigation prefetch churn causing unnecessary requests.

## Final Validation

- Baseline benchmark: pending until run is recorded.
- Feature benchmark after fixes: pending until hotspot is selected.
- Local verification: pending.
- GitHub Actions: pending.
- Post-deploy full production benchmark: pending.

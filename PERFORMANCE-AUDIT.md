# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-07T13:25:25.984Z
- Benchmark generated at (UTC): 2026-05-07T13:25:12.351Z
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 161
- Critical pages: 100
- High pages: 61
- Medium pages: 0
- Slow requests >800ms: 3884
- Slow requests 400-800ms: 994

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 25.1 | 5775 | 1 | 1 | yes |
| logged-in-admin | desktop | 35 | 28.9 | 9683 | 176 | 1 | yes |
| logged-in-reader | desktop | 35 | 29.2 | 7061 | 88 | 1 | yes |
| logged-out | mobile | 9 | 24.9 | 4754 | 2 | 0 | yes |
| logged-in-admin | mobile | 35 | 29.3 | 6927 | 130 | 1 | yes |
| logged-in-reader | mobile | 38 | 28.8 | 8074 | 114 | 1 | yes |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 41 | 4 | 35 | 0 | 8418 | 1120 | 2544 | 0.0 | 0.0173 | 0 | 1 | 791182 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 40 | 4 | 34 | 0 | 7729 | 1308 | 1308 | 0.0 | 0.0039 | 0 | 0 | 746155 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 39 | 3 | 34 | 0 | 7246 | 2736 | 4356 | 0.0 | 0.0042 | 2 | 1 | 771740 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 39 | 3 | 34 | 0 | 6466 | 1956 | 2292 | 0.0 | 0.0071 | 0 | 0 | 803036 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 38 | 2 | 34 | 0 | 5040 | 1316 | 1316 | 0.0 | 0.0039 | 2 | 0 | 655176 |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 38 | 2 | 34 | 0 | 6365 | 1312 | 3160 | 0.0 | 0.0039 | 0 | 1 | 779875 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 38 | 3 | 33 | 0 | 8050 | 1416 | 1416 | 0.0 | 0.0071 | 1 | 0 | 800848 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 37 | 2 | 33 | 0 | 7819 | 3148 | 3148 | 0.0 | 0.0042 | 3 | 0 | 654043 |
| Critical | auth-public | logged-in-reader | desktop | /vi | 36 | 5 | 28 | 0 | 6772 | 1876 | 1876 | 362.0 | 0.0031 | 2 | 0 | 613638 |
| Critical | auth-public | logged-in-admin | mobile | /vi | 36 | 5 | 28 | 0 | 5160 | 1004 | 1004 | 0.0 | 0.0055 | 1 | 0 | 614834 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 36 | 1 | 33 | 0 | 8392 | 1548 | 3084 | 0.0 | 0.0173 | 0 | 1 | 769372 |
| Critical | auth-public | logged-in-admin | desktop | /vi | 35 | 5 | 27 | 0 | 28308 | 22736 | 22736 | 214.0 | 0.0040 | 3 | 0 | 612971 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 35 | 0 | 33 | 0 | 5152 | 736 | 1060 | 0.0 | 0.0071 | 2 | 0 | 661181 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 35 | 0 | 33 | 0 | 6559 | 1188 | 1188 | 0.0 | 0.0071 | 4 | 0 | 657600 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 34 | 3 | 29 | 0 | 8512 | 2584 | 2924 | 0.0 | 0.0071 | 1 | 0 | 728840 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/747b4529-82d6-4bc3-86ca-fe3c9eca4373 | 33 | 2 | 29 | 0 | 9674 | 1172 | 1172 | 0.0 | 0.0072 | 6 | 0 | 681514 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/789b367a-02d2-4de3-b131-a820b8f4446d | 33 | 2 | 29 | 0 | 7144 | 1404 | 1404 | 0.0 | 0.0072 | 4 | 0 | 677259 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 32 | 2 | 28 | 0 | 6898 | 1024 | 1024 | 0.0 | 0.0071 | 15 | 0 | 646790 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | 32 | 2 | 28 | 0 | 5344 | 1448 | 1448 | 0.0 | 0.0071 | 4 | 0 | 640392 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 32 | 2 | 28 | 0 | 19273 | 10532 | 10852 | 0.0 | 0.0071 | 3 | 0 | 638651 |
| Critical | auth-public | logged-out | desktop | /vi | 31 | 2 | 27 | 0 | 6911 | 3876 | 4376 | 166.0 | 0.0000 | 1 | 1 | 601848 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/chat | 31 | 2 | 27 | 0 | 5562 | 2276 | 2276 | 0.0 | 0.0042 | 1 | 0 | 627401 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 31 | 2 | 27 | 0 | 4905 | 1648 | 1648 | 0.0 | 0.0042 | 3 | 0 | 626743 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/30dfcecd-bc35-4875-bf86-f1c178b69b32 | 31 | 0 | 29 | 0 | 4888 | 1516 | 1516 | 0.0 | 0.0042 | 4 | 0 | 705222 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/d07353aa-8357-4bef-9a94-5f24326eaebd | 31 | 0 | 29 | 0 | 5498 | 1452 | 1452 | 0.0 | 0.0042 | 1 | 0 | 705268 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/dd6bd3cb-76a8-4176-a706-30cfc54f8aa0 | 31 | 0 | 29 | 0 | 8345 | 2064 | 2428 | 0.0 | 0.0042 | 1 | 0 | 704970 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/ed7a9895-677a-4657-9dc3-ccedc0286480 | 31 | 0 | 29 | 0 | 14080 | 5608 | 5608 | 0.0 | 0.0039 | 6 | 0 | 705196 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/8986c52b-4959-47f3-b4a8-65f1ac0d0e4c | 31 | 0 | 29 | 0 | 10157 | 5920 | 5920 | 0.0 | 0.0039 | 2 | 0 | 705153 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/5d12f152-decd-4d32-bf40-11b28fd91a3e | 31 | 0 | 29 | 0 | 4963 | 1124 | 1124 | 0.0 | 0.0039 | 4 | 0 | 705152 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 31 | 1 | 28 | 0 | 6298 | 2764 | 3108 | 0.0 | 0.0071 | 1 | 0 | 726710 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | 2 | 27 | 0 | 7038 | 2656 | 2972 | 0.0 | 0.0071 | 1 | 0 | 627629 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | 2 | 27 | 0 | 7922 | 1804 | 1804 | 0.0 | 0.0071 | 1 | 0 | 629066 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 30 | 0 | 28 | 0 | 6990 | 2084 | 2084 | 0.0 | 0.0042 | 3 | 0 | 725952 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 30 | 0 | 28 | 0 | 10448 | 5632 | 5632 | 0.0 | 0.0180 | 2 | 0 | 642162 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 30 | 2 | 26 | 0 | 8525 | 2956 | 2956 | 0.0 | 0.0039 | 3 | 0 | 627758 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/07e40f3a-8aad-4260-af12-26586c048b38 | 30 | 0 | 28 | 0 | 9687 | 3692 | 4028 | 0.0 | 0.0072 | 2 | 0 | 680610 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/de78776f-5892-4790-823f-b132be54609d | 30 | 0 | 28 | 0 | 7691 | 1448 | 1448 | 0.0 | 0.0071 | 2 | 0 | 680810 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 9362 | 1640 | 1640 | 0.0 | 0.0267 | 2 | 0 | 642049 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/5a454764-2a4a-41db-9d7b-0b2cb451d876 | 30 | 0 | 28 | 0 | 6037 | 1256 | 1256 | 0.0 | 0.0072 | 3 | 0 | 673586 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/f7039a71-8a13-4e89-9f9b-79f0f56eb615 | 30 | 0 | 28 | 0 | 7083 | 3552 | 3884 | 0.0 | 0.0072 | 3 | 0 | 673545 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 29 | 0 | 27 | 0 | 10681 | 1988 | 1988 | 0.0 | 0.0043 | 13 | 0 | 642943 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 29 | 0 | 27 | 0 | 8408 | 2968 | 2968 | 0.0 | 0.0489 | 2 | 0 | 636077 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | 29 | 0 | 27 | 0 | 5071 | 1296 | 1296 | 0.0 | 0.0042 | 2 | 0 | 628783 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 7014 | 2292 | 2292 | 0.0 | 0.0042 | 4 | 0 | 636547 |
| Critical | auth-public | logged-out | mobile | /vi | 29 | 0 | 27 | 0 | 7786 | 2636 | 2988 | 0.0 | 0.0000 | 1 | 0 | 606266 |
| Critical | auth-public | logged-in-reader | mobile | /vi | 29 | 0 | 27 | 0 | 7468 | 1896 | 2248 | 0.0 | 0.0439 | 1 | 0 | 607783 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 29 | 0 | 27 | 0 | 10447 | 1892 | 2196 | 0.0 | 0.1024 | 5 | 0 | 636832 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 6405 | 1460 | 1460 | 0.0 | 0.0071 | 1 | 0 | 636999 |
| Critical | reading | logged-in-admin | desktop | /vi/reading | 28 | 0 | 26 | 0 | 10505 | 3192 | 3192 | 0.0 | 0.0042 | 3 | 0 | 641450 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 8430 | 1792 | 1792 | 0.0 | 0.0000 | 24 | 0 | 624058 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 15966 | 12976 | 13288 | 0.0 | 0.0042 | 1 | 0 | 627164 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 10162 | 1608 | 2476 | 0.0 | 0.0042 | 2 | 0 | 624590 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 5058 | - | - | 0.0 | 0.0000 | 27 | 0 | 624103 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 6418 | 2368 | 2368 | 0.0 | 0.0042 | 3 | 0 | 625004 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 11019 | 6952 | 6952 | 0.0 | 0.0043 | 1 | 0 | 625959 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 6877 | 1728 | 1728 | 0.0 | 0.0042 | 3 | 0 | 624130 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 7821 | 1488 | 1488 | 0.0 | 0.0042 | 4 | 0 | 624526 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 6826 | 1832 | 1832 | 0.0 | 0.0039 | 3 | 0 | 631642 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 16999 | 1912 | 1912 | 0.0 | 0.0039 | 2 | 0 | 627231 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 12735 | 5392 | 5392 | 0.0 | 0.0044 | 3 | 0 | 624983 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 6622 | 2624 | 2992 | 0.0 | 0.0039 | 3 | 0 | 625613 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 8074 | 4792 | 4792 | 0.0 | 0.0039 | 1 | 0 | 624262 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 7147 | - | - | 0.0 | 0.0000 | 23 | 0 | 630489 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 6157 | 1756 | 2096 | 0.0 | 0.0071 | 1 | 0 | 633961 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 5298 | 1312 | 1312 | 0.0 | 0.0071 | 1 | 0 | 632232 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 8552 | 1776 | 1776 | 0.0 | 0.0000 | 22 | 0 | 630712 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 5608 | 1592 | 1928 | 0.0 | 0.0074 | 1 | 0 | 631807 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 4544 | 1208 | 1208 | 0.0 | 0.0071 | 1 | 0 | 632656 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 6281 | 2796 | 2796 | 0.0 | 0.0071 | 1 | 0 | 631055 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 16013 | 2620 | 2972 | 0.0 | 0.0071 | 3 | 0 | 631039 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 9521 | 5896 | 6232 | 0.0 | 0.0071 | 5 | 0 | 631166 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 8945 | 2148 | 2352 | 0.0 | 0.0071 | 2 | 0 | 625600 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 6294 | 2324 | 2676 | 0.0 | 0.0071 | 2 | 0 | 624643 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 5626 | 1144 | 1144 | 0.0 | 0.0071 | 2 | 0 | 627185 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 7151 | 1436 | 1436 | 0.0 | 0.0071 | 2 | 0 | 624830 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 9715 | 2428 | 2768 | 0.0 | 0.0071 | 2 | 0 | 624966 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 10065 | 5772 | 6112 | 0.0 | 0.0401 | 2 | 0 | 626069 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 9665 | 4672 | 4672 | 0.0 | 0.0087 | 2 | 0 | 625441 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 7892 | 4468 | 4804 | 0.0 | 0.0071 | 2 | 0 | 625539 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 4868 | 1060 | 1060 | 0.0 | 0.0071 | 3 | 0 | 625840 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 8632 | 4008 | 4008 | 0.0 | 0.0071 | 2 | 0 | 624559 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 9099 | 4308 | 4644 | 0.0 | 0.0071 | 2 | 0 | 624584 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 8293 | 2620 | 2964 | 0.0 | 0.0071 | 2 | 0 | 626050 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 7747 | 4296 | 4636 | 0.0 | 0.0071 | 3 | 0 | 626127 |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 6706 | 3032 | 3032 | 0.0 | 0.0014 | 1 | 0 | 520173 |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 6340 | 1724 | 2028 | 0.0 | 0.0055 | 1 | 0 | 525996 |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 5360 | 1828 | 1828 | 0.0 | 0.0055 | 1 | 0 | 520098 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | 24 | 0 | 22 | 0 | 8939 | 1336 | 1336 | 36.0 | 0.0000 | 19 | 0 | 511107 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | 24 | 0 | 22 | 0 | 27074 | - | - | 0.0 | 0.0000 | 23 | 0 | 511084 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 12066 | - | - | 0.0 | 0.0023 | 21 | 0 | 511191 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | 24 | 0 | 22 | 0 | 4877 | 1548 | 1548 | 65.0 | 0.0000 | 16 | 0 | 511102 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | 24 | 0 | 22 | 0 | 4166 | 1980 | 1980 | 0.0 | 0.0000 | 18 | 0 | 511225 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 8893 | 1376 | 1376 | 0.0 | 0.0000 | 23 | 0 | 511244 |
| Critical | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 4880 | 1744 | 1744 | 0.0 | 0.0000 | 1 | 0 | 512280 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | 24 | 0 | 22 | 0 | 4838 | - | - | 0.0 | 0.0000 | 20 | 0 | 511115 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | 24 | 0 | 22 | 0 | 5657 | 1420 | 1420 | 0.0 | 0.0384 | 17 | 0 | 511135 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 4636 | 1120 | 1120 | 0.0 | 0.0000 | 1 | 0 | 511125 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | 24 | 0 | 22 | 0 | 7255 | 1904 | 1904 | 0.0 | 0.0384 | 21 | 0 | 511030 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | 24 | 0 | 22 | 0 | 8354 | 1276 | 1276 | 0.0 | 0.0384 | 20 | 0 | 511110 |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 7688 | 1204 | 1204 | 0.0 | 0.0000 | 14 | 0 | 511192 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 35 | 0 | 33 | 0 | 6997 | 1372 | 1372 | 0.0 | 0.0042 | 0 | 0 | 733423 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 33 | 2 | 29 | 0 | 4830 | 1548 | 1548 | 0.0 | 0.0039 | 0 | 0 | 726929 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 33 | 2 | 29 | 0 | 6238 | 1440 | 1440 | 0.0 | 0.0267 | 0 | 0 | 653802 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 32 | 2 | 28 | 0 | 6055 | 1504 | 1800 | 0.0 | 0.0039 | 0 | 0 | 638642 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 32 | 3 | 27 | 0 | 5660 | 1648 | 1988 | 0.0 | 0.0071 | 0 | 0 | 646740 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 31 | 2 | 27 | 0 | 17378 | 13928 | 13928 | 0.0 | 0.0042 | 0 | 0 | 627828 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 31 | 1 | 28 | 0 | 5743 | 1368 | 1368 | 0.0 | 0.0177 | 0 | 0 | 650748 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 31 | 2 | 27 | 0 | 4804 | 1044 | 1044 | 0.0 | 0.0039 | 0 | 0 | 628376 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 31 | 2 | 27 | 0 | 8276 | 3952 | 3952 | 0.0 | 0.0760 | 0 | 0 | 648257 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 31 | 2 | 27 | 0 | 5975 | 1536 | 1536 | 0.0 | 0.0071 | 0 | 0 | 636623 |
| High | reading | logged-in-reader | mobile | /vi/reading | 31 | 2 | 27 | 0 | 5217 | 1452 | 1452 | 0.0 | 0.0071 | 0 | 0 | 645607 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 30 | 2 | 26 | 0 | 6739 | 2824 | 2824 | 0.0 | 0.0039 | 0 | 0 | 626515 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 30 | 2 | 26 | 0 | 7370 | 3972 | 3972 | 0.0 | 0.0071 | 0 | 0 | 633711 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 8100 | 1132 | 1132 | 0.0 | 0.0039 | 0 | 0 | 641710 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 29 | 0 | 27 | 0 | 4771 | 952 | 1240 | 0.0 | 0.0726 | 0 | 0 | 635927 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 6185 | 1232 | 1672 | 0.0 | 0.0190 | 0 | 0 | 643416 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 29 | 0 | 27 | 0 | 6048 | 1564 | 1900 | 0.0 | 0.0071 | 0 | 0 | 634056 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 6904 | 3216 | 3216 | 0.0 | 0.0042 | 0 | 0 | 625413 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 6681 | 3076 | 3076 | 0.0 | 0.0042 | 0 | 0 | 624275 |
| High | reading | logged-in-reader | desktop | /vi/reading | 28 | 0 | 26 | 0 | 4082 | 768 | 1116 | 0.0 | 0.0039 | 0 | 0 | 641397 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 4821 | 1908 | 1908 | 0.0 | 0.0039 | 0 | 0 | 631245 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 3716 | 1104 | 1104 | 0.0 | 0.0039 | 0 | 0 | 632150 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 5165 | 1788 | 1788 | 0.0 | 0.0040 | 0 | 0 | 633855 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 6318 | 2264 | 2264 | 0.0 | 0.0039 | 0 | 0 | 624369 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 4635 | 2004 | 2004 | 0.0 | 0.0039 | 0 | 0 | 624418 |
| High | reading | logged-in-admin | mobile | /vi/reading | 28 | 0 | 26 | 0 | 4928 | 1212 | 1212 | 0.0 | 0.0071 | 0 | 0 | 641672 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 6261 | 2076 | 2076 | 0.0 | 0.0071 | 0 | 0 | 631291 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 7869 | 2144 | 2368 | 0.0 | 0.0071 | 0 | 0 | 631341 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 6023 | 2560 | 2908 | 0.0 | 0.0071 | 0 | 0 | 624378 |
| High | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 4957 | 992 | 992 | 0.0 | 0.0000 | 0 | 0 | 525710 |
| High | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 5330 | 2512 | 2512 | 0.0 | 0.0000 | 0 | 0 | 525786 |
| High | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 5707 | 1600 | 1916 | 0.0 | 0.0000 | 0 | 0 | 525843 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 8084 | 4112 | 4112 | 0.0 | 0.0020 | 0 | 0 | 519899 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 14181 | 3316 | 3316 | 0.0 | 0.0020 | 0 | 0 | 520196 |
| High | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 11621 | 8400 | 8400 | 0.0 | 0.0019 | 0 | 0 | 519903 |
| High | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3832 | 1276 | 1276 | 0.0 | 0.0019 | 0 | 0 | 519586 |
| High | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 5030 | 788 | 788 | 0.0 | 0.0019 | 0 | 0 | 520237 |
| High | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 4228 | 1388 | 1704 | 0.0 | 0.0000 | 0 | 0 | 525654 |
| High | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3670 | 952 | 1288 | 0.0 | 0.0000 | 0 | 0 | 525911 |
| High | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 4156 | 1340 | 1340 | 0.0 | 0.0000 | 0 | 0 | 525901 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 6968 | 1876 | 2220 | 0.0 | 0.0055 | 0 | 0 | 526007 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 10780 | 5996 | 5996 | 0.0 | 0.0055 | 0 | 0 | 526269 |
| High | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 6614 | 1996 | 2304 | 0.0 | 0.0055 | 0 | 0 | 520224 |
| High | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 4316 | 780 | 780 | 0.0 | 0.0055 | 0 | 0 | 519944 |
| High | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 4952 | 1756 | 1756 | 0.0 | 0.0000 | 0 | 0 | 512183 |
| High | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 9022 | 5304 | 5304 | 0.0 | 0.0000 | 0 | 0 | 512696 |
| High | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 5252 | 1756 | 1756 | 0.0 | 0.0000 | 0 | 0 | 511751 |
| High | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 4423 | 1252 | 1252 | 0.0 | 0.0000 | 0 | 0 | 511899 |
| High | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 5423 | 2504 | 2504 | 0.0 | 0.0000 | 0 | 0 | 511989 |
| High | auth-public | logged-in-admin | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 6400 | 1260 | 1260 | 0.0 | 0.0000 | 0 | 0 | 511861 |
| High | auth-public | logged-in-admin | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 14052 | 6028 | 6028 | 0.0 | 0.0000 | 0 | 0 | 512010 |
| High | auth-public | logged-in-reader | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 12084 | 3716 | 3716 | 0.0 | 0.0000 | 0 | 0 | 511867 |
| High | auth-public | logged-in-reader | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 5779 | 2392 | 2392 | 0.0 | 0.0001 | 0 | 0 | 512049 |
| High | auth-public | logged-out | mobile | /vi/register | 24 | 0 | 22 | 0 | 4833 | 1420 | 1420 | 0.0 | 0.0000 | 0 | 0 | 512816 |
| High | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 3787 | 744 | 744 | 0.0 | 0.0000 | 0 | 0 | 511819 |
| High | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 4448 | 1116 | 1116 | 0.0 | 0.0000 | 0 | 0 | 511972 |
| High | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 4998 | 1524 | 1524 | 0.0 | 0.0000 | 0 | 0 | 512053 |
| High | auth-public | logged-in-admin | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 6204 | 2664 | 2664 | 0.0 | 0.0000 | 0 | 0 | 511944 |
| High | auth-public | logged-in-admin | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 4534 | 1524 | 1524 | 0.0 | 0.0000 | 0 | 0 | 512034 |
| High | auth-public | logged-in-reader | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 8599 | 4592 | 4592 | 0.0 | 0.0000 | 0 | 0 | 511926 |
| High | auth-public | logged-in-reader | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 17656 | 11588 | 11588 | 0.0 | 0.0004 | 0 | 0 | 511969 |

## Major Issues Found

- Critical: 100 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: 61 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: chưa phát hiện page Medium theo benchmark hiện tại.
- Duplicate: 1 nhóm duplicate request cần kiểm tra over-fetch/cache key.
- Pending: 95 page(s) có pending request không phải websocket/eventsource.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | auth-public | logged-in-admin | desktop | /vi | GET | 200 | 20674 | 20480 | html | https://www.tarotnow.xyz/vi |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | GET | 200 | 14739 | 5923 | static | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | GET | 200 | 14678 | 6198 | static | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 14082 | 811 | static | https://www.tarotnow.xyz/_next/static/chunks/0p7wdasoeo.fs.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 14013 | 736 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 14012 | 757 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13990 | 723 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 13815 | 1163 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13782 | 687 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13402 | 160 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13387 | 1409 | static | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13326 | 1534 | static | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13287 | 1516 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13287 | 1525 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13283 | 1354 | static | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13282 | 1509 | static | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13260 | 1489 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13237 | 1489 | static | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 13119 | 10599 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 13112 | 10603 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 13110 | 53 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 13081 | 575 | static | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 13008 | 10160 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12992 | 513 | static | https://www.tarotnow.xyz/_next/static/chunks/0p7wdasoeo.fs.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12920 | 491 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12919 | 233 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12917 | 420 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12901 | 242 | static | https://www.tarotnow.xyz/_next/static/chunks/0ddj0tt3hbb_o.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | GET | 200 | 12890 | 12444 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12868 | 453 | static | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 12865 | 10167 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12855 | 417 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 12846 | 10157 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12841 | 439 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12698 | 48 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12687 | 218 | static | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12463 | 570 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12454 | 492 | static | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12440 | 554 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12436 | 472 | static | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12428 | 464 | static | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12426 | 272 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12409 | 520 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12404 | 516 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 12156 | 243 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 11962 | 2128 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 11922 | 175 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 11913 | 161 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 11897 | 2055 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 11860 | 1792 | static | https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 11851 | 1790 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 11761 | 623 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 11500 | 490 | static | https://www.tarotnow.xyz/_next/static/chunks/0p7wdasoeo.fs.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 11457 | 528 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 11429 | 596 | static | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 11426 | 404 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 11383 | 403 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 11364 | 565 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 11358 | 561 | static | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 11356 | 561 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/verify-email | GET | 200 | 11283 | 11125 | html | https://www.tarotnow.xyz/vi/verify-email |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 11200 | 58 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 11024 | 194 | static | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10997 | 199 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10963 | 191 | static | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10932 | 194 | static | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 10899 | 8185 | static | https://www.tarotnow.xyz/_next/static/chunks/0p7wdasoeo.fs.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 10827 | 8473 | static | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10820 | 374 | static | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10819 | 375 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10818 | 375 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10800 | 377 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10799 | 392 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10799 | 377 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10771 | 344 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 10738 | 356 | static | https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | GET | 200 | 10612 | 400 | static | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | GET | 200 | 10602 | 386 | static | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | GET | 200 | 10602 | 400 | static | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | GET | 200 | 10549 | 364 | static | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| Critical | auth-public | logged-out | desktop | /vi | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | auth-public | logged-in-admin | desktop | /vi | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| Critical | auth-public | logged-in-admin | desktop | /vi | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | auth-public | logged-in-admin | desktop | /vi | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | reading | logged-in-admin | desktop | /vi/reading | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| Critical | reading | logged-in-admin | desktop | /vi/reading | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-admin | desktop | /vi/reading | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | https://www.tarotnow.xyz/api/auth/session |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F09_Strength_50_20260325_181351.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F11_Wheel_of%2B_Fortune_50_20260325_181353.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F12_Justice_50_20260325_181353.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F13_The_Hanged_Man_50_20260325_181356.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F14_Death_50_20260325_181356.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F15_Temperance_50_20260325_181356.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F16_The_Devil_50_20260325_181357.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0p7wdasoeo.fs.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=128&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/chat | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0p7wdasoeo.fs.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/reader/apply | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reader-chat | logged-in-admin | desktop | /vi/reader/apply | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/reader/apply | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | reading | logged-in-admin | desktop | /vi/reading/history | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/tos | https://www.tarotnow.xyz/vi/legal/tos |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/30dfcecd-bc35-4875-bf86-f1c178b69b32 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/30dfcecd-bc35-4875-bf86-f1c178b69b32 | https://www.tarotnow.xyz/api/reading/cards-catalog |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/30dfcecd-bc35-4875-bf86-f1c178b69b32 | https://www.tarotnow.xyz/_next/static/chunks/09.78-sm.tf5_.js |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/30dfcecd-bc35-4875-bf86-f1c178b69b32 | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/d07353aa-8357-4bef-9a94-5f24326eaebd | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/dd6bd3cb-76a8-4176-a706-30cfc54f8aa0 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| Critical | auth-public | logged-in-reader | desktop | /vi | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | auth-public | logged-in-reader | desktop | /vi | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/chat | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | reader-chat | logged-in-reader | desktop | /vi/chat | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/chat | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/reader/apply | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | reader-chat | logged-in-reader | desktop | /vi/reader/apply | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reader-chat | logged-in-reader | desktop | /vi/reader/apply | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/ed7a9895-677a-4657-9dc3-ccedc0286480 | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/ed7a9895-677a-4657-9dc3-ccedc0286480 | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/ed7a9895-677a-4657-9dc3-ccedc0286480 | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/ed7a9895-677a-4657-9dc3-ccedc0286480 | https://www.tarotnow.xyz/api/reading/cards-catalog |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/ed7a9895-677a-4657-9dc3-ccedc0286480 | https://www.tarotnow.xyz/_next/static/chunks/09.78-sm.tf5_.js |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/ed7a9895-677a-4657-9dc3-ccedc0286480 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/8986c52b-4959-47f3-b4a8-65f1ac0d0e4c | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/8986c52b-4959-47f3-b4a8-65f1ac0d0e4c | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/5d12f152-decd-4d32-bf40-11b28fd91a3e | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/5d12f152-decd-4d32-bf40-11b28fd91a3e | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/5d12f152-decd-4d32-bf40-11b28fd91a3e | https://www.tarotnow.xyz/api/reading/cards-catalog |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/5d12f152-decd-4d32-bf40-11b28fd91a3e | https://www.tarotnow.xyz/_next/static/chunks/09.78-sm.tf5_.js |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-out | mobile | /vi | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=1200&q=75 |
| Critical | auth-public | logged-out | mobile | /vi/login | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | auth-public | logged-in-admin | mobile | /vi | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F02_The_Magician_50_20260325_181348.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F03_The_High%2BPriestess%2B_50_20260325_181348.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F04_The_Empress_50_20260325_181348.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F05_The_Emperor_50_20260325_181348.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F06_The_Hierophant_50_20260325_181348.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F07_The_Lovers_50_20260325_181351.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F08_The_Chariot_50_20260325_181351.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F09_Strength_50_20260325_181351.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F10_The_Hermit_50_20260325_181353.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F11_Wheel_of%2B_Fortune_50_20260325_181353.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F12_Justice_50_20260325_181353.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F13_The_Hanged_Man_50_20260325_181356.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F14_Death_50_20260325_181356.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F15_Temperance_50_20260325_181356.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F16_The_Devil_50_20260325_181357.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0p7wdasoeo.fs.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=384&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0p7wdasoeo.fs.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reading | logged-in-admin | mobile | /vi/reading/history | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/tos | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/07e40f3a-8aad-4260-af12-26586c048b38 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/07e40f3a-8aad-4260-af12-26586c048b38 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/747b4529-82d6-4bc3-86ca-fe3c9eca4373 | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/747b4529-82d6-4bc3-86ca-fe3c9eca4373 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/747b4529-82d6-4bc3-86ca-fe3c9eca4373 | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/747b4529-82d6-4bc3-86ca-fe3c9eca4373 | https://www.tarotnow.xyz/api/reading/cards-catalog |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/747b4529-82d6-4bc3-86ca-fe3c9eca4373 | https://www.tarotnow.xyz/_next/static/chunks/09.78-sm.tf5_.js |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/747b4529-82d6-4bc3-86ca-fe3c9eca4373 | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/de78776f-5892-4790-823f-b132be54609d | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/de78776f-5892-4790-823f-b132be54609d | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=1200&q=75 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0npt-1adflxcq.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=96&q=75 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F14_Death_50_20260325_181356.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=640&q=75 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | https://www.tarotnow.xyz/vi/profile |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/chat | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/chat | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/5a454764-2a4a-41db-9d7b-0b2cb451d876 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/5a454764-2a4a-41db-9d7b-0b2cb451d876 | https://www.tarotnow.xyz/_next/static/chunks/09.78-sm.tf5_.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/5a454764-2a4a-41db-9d7b-0b2cb451d876 | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/789b367a-02d2-4de3-b131-a820b8f4446d | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/789b367a-02d2-4de3-b131-a820b8f4446d | https://www.tarotnow.xyz/api/reading/cards-catalog |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/789b367a-02d2-4de3-b131-a820b8f4446d | https://www.tarotnow.xyz/_next/static/chunks/09.78-sm.tf5_.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/789b367a-02d2-4de3-b131-a820b8f4446d | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/f7039a71-8a13-4e89-9f9b-79f0f56eb615 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/f7039a71-8a13-4e89-9f9b-79f0f56eb615 | https://www.tarotnow.xyz/_next/static/chunks/09.78-sm.tf5_.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/f7039a71-8a13-4e89-9f9b-79f0f56eb615 | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

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

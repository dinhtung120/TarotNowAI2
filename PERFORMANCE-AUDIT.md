# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-08T16:39:22.529Z
- Benchmark generated at (UTC): 2026-05-08T16:39:11.304Z
- Benchmark input: Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 190
- Critical pages: 27
- High pages: 151
- Medium pages: 12
- Slow requests >800ms: 270
- Slow requests 400-800ms: 526
- Request thresholds: >25 suspicious, >35 severe
- Slow request thresholds: >400ms optimize, >800ms serious

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 3824 | 1 | 0 | yes |
| logged-in-admin | desktop | 48 | 30.6 | 3572 | 20 | 0 | yes |
| logged-in-reader | desktop | 38 | 30.4 | 3411 | 1 | 0 | yes |
| logged-out | mobile | 9 | 24.9 | 3356 | 0 | 0 | yes |
| logged-in-admin | mobile | 48 | 30.2 | 3572 | 8 | 0 | yes |
| logged-in-reader | mobile | 38 | 31.0 | 3398 | 1 | 0 | yes |

## Route Family Coverage

| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 3586 | 0 | 0 |
| logged-in-admin | desktop | auth-public | 8 | 32.8 | 3639 | 3 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | 3 | 33.0 | 3727 | 0 | 0 |
| logged-in-admin | desktop | home | 1 | 35.0 | 3267 | 0 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | 4 | 33.8 | 4625 | 1 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | 8 | 29.4 | 3639 | 16 | 0 |
| logged-in-admin | desktop | reader-chat | 9 | 28.6 | 3230 | 0 | 0 |
| logged-in-admin | desktop | reading | 5 | 30.2 | 3070 | 0 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.6 | 3155 | 0 | 0 |
| logged-in-admin | mobile | auth-public | 8 | 32.5 | 4506 | 6 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | 3 | 29.7 | 3306 | 0 | 0 |
| logged-in-admin | mobile | home | 1 | 29.0 | 3401 | 0 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | 4 | 34.3 | 4205 | 0 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | 8 | 28.9 | 3139 | 0 | 0 |
| logged-in-admin | mobile | reader-chat | 9 | 29.4 | 3587 | 2 | 0 |
| logged-in-admin | mobile | reading | 5 | 28.8 | 3268 | 0 | 0 |
| logged-in-reader | desktop | auth-public | 8 | 31.5 | 3410 | 1 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | 3 | 31.0 | 4121 | 0 | 0 |
| logged-in-reader | desktop | home | 1 | 29.0 | 3066 | 0 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | 4 | 33.8 | 4308 | 0 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | 8 | 29.6 | 3067 | 0 | 0 |
| logged-in-reader | desktop | reader-chat | 9 | 28.9 | 3374 | 0 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.8 | 2958 | 0 | 0 |
| logged-in-reader | mobile | auth-public | 8 | 35.1 | 3584 | 0 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | 3 | 31.7 | 3421 | 0 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2707 | 0 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | 4 | 32.8 | 3993 | 0 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | 8 | 30.0 | 3505 | 0 | 0 |
| logged-in-reader | mobile | reader-chat | 9 | 28.8 | 3090 | 0 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.0 | 3131 | 1 | 0 |
| logged-out | desktop | auth-public | 8 | 24.4 | 3781 | 0 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4168 | 1 | 0 |
| logged-out | mobile | auth-public | 8 | 24.4 | 3369 | 0 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 3248 | 0 | 0 |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | 62 | 7 | 50 | 0 | 4806 | 644 | 1320 | 402.0 | 0.0035 | 1 | 0 | 1123508 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | 60 | 5 | 49 | 0 | 4310 | 836 | 1200 | 0.0 | 0.0055 | 0 | 0 | 1107198 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | 53 | 0 | 49 | 0 | 4964 | 700 | 1348 | 437.0 | 0.0038 | 0 | 0 | 1108154 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | 53 | 0 | 49 | 0 | 11637 | 3164 | 3164 | 0.0 | 0.0439 | 0 | 0 | 1108086 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | 53 | 0 | 49 | 0 | 4416 | 568 | 932 | 0.0 | 0.0055 | 0 | 0 | 1092326 |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | 53 | 0 | 49 | 0 | 3792 | 572 | 916 | 0.0 | 0.0055 | 0 | 0 | 1092479 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | 51 | 0 | 46 | 0 | 4144 | 780 | 1404 | 408.0 | 0.0035 | 0 | 0 | 1048847 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | 50 | 0 | 46 | 0 | 3500 | 684 | 1320 | 267.0 | 0.0033 | 0 | 0 | 1037857 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | 50 | 0 | 46 | 0 | 3426 | 556 | 920 | 0.0 | 0.0055 | 0 | 0 | 1037836 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 39 | 3 | 34 | 0 | 4377 | 788 | 1088 | 0.0 | 0.0042 | 0 | 0 | 664381 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 39 | 4 | 33 | 0 | 3986 | 680 | 1032 | 0.0 | 0.0071 | 0 | 0 | 801997 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 37 | 2 | 33 | 0 | 3624 | 640 | 1332 | 0.0 | 0.0039 | 0 | 0 | 654920 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 37 | 2 | 33 | 0 | 3693 | 668 | 1056 | 0.0 | 0.0039 | 0 | 0 | 735352 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 37 | 2 | 33 | 0 | 3796 | 580 | 924 | 0.0 | 0.0071 | 0 | 0 | 799633 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 36 | 2 | 32 | 0 | 4786 | 608 | 2064 | 23.0 | 0.0042 | 0 | 0 | 778787 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/60b1af90-19b3-4b78-80c3-b720c958cb1a | 36 | 4 | 30 | 0 | 3062 | 628 | 1024 | 0.0 | 0.0042 | 0 | 0 | 727762 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/2385fd1c-641f-41a2-9aa0-2216f893cc49 | 36 | 4 | 30 | 0 | 3165 | 668 | 1064 | 0.0 | 0.0039 | 0 | 0 | 727645 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 36 | 2 | 32 | 0 | 3611 | 784 | 1568 | 0.0 | 0.0000 | 0 | 0 | 779101 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 33 | 4 | 27 | 0 | 4713 | 776 | 1688 | 0.0 | 0.0071 | 1 | 0 | 646235 |
| Critical | reading | logged-in-reader | mobile | /vi/reading | 33 | 4 | 27 | 0 | 3950 | 568 | 896 | 0.0 | 0.0000 | 1 | 0 | 654782 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 32 | 3 | 27 | 0 | 4585 | 912 | 1232 | 0.0 | 0.0000 | 1 | 0 | 643088 |
| Critical | auth-public | logged-out | desktop | /vi | 29 | 0 | 27 | 0 | 4168 | 1536 | 1536 | 493.0 | 0.0000 | 1 | 0 | 601425 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6655 | 608 | 608 | 15.0 | 0.0043 | 1 | 0 | 643325 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2994 | - | - | 55.0 | 0.0000 | 16 | 0 | 631211 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | 24 | 0 | 22 | 0 | 3382 | 692 | 1248 | 41.0 | 0.0035 | 2 | 0 | 510982 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | 24 | 0 | 22 | 0 | 3077 | 684 | 1308 | 432.0 | 0.0038 | 1 | 0 | 511028 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 3388 | 564 | 924 | 0.0 | 0.0024 | 6 | 0 | 511075 |
| High | auth-public | logged-in-admin | desktop | /vi | 35 | 5 | 27 | 0 | 3267 | 696 | 1332 | 261.0 | 0.0035 | 0 | 0 | 613329 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 35 | 0 | 33 | 0 | 3813 | 1080 | 1508 | 0.0 | 0.0042 | 0 | 0 | 732696 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 35 | 0 | 33 | 0 | 3461 | 716 | 1068 | 0.0 | 0.0071 | 0 | 0 | 661424 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 35 | 0 | 33 | 0 | 3082 | 560 | 916 | 0.0 | 0.0071 | 0 | 0 | 661564 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 34 | 4 | 28 | 0 | 4266 | 608 | 1044 | 0.0 | 0.0489 | 0 | 0 | 647428 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 34 | 0 | 32 | 0 | 4045 | 644 | 1804 | 0.0 | 0.0039 | 0 | 0 | 776557 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 33 | 4 | 27 | 0 | 5977 | 636 | 1092 | 0.0 | 0.0042 | 0 | 0 | 644875 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 33 | 4 | 27 | 0 | 3466 | 608 | 992 | 0.0 | 0.0042 | 0 | 0 | 645610 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 33 | 3 | 27 | 0 | 3286 | 616 | 1088 | 0.0 | 0.0726 | 0 | 0 | 639095 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 33 | 3 | 27 | 0 | 3014 | 552 | 888 | 0.0 | 0.0000 | 0 | 0 | 639155 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 33 | 4 | 27 | 0 | 4542 | 592 | 932 | 0.0 | 0.0000 | 0 | 0 | 645269 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | 4 | 27 | 0 | 4389 | 700 | 1020 | 0.0 | 0.0000 | 0 | 0 | 644615 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 32 | 2 | 28 | 0 | 3655 | 764 | 1188 | 0.0 | 0.0042 | 0 | 0 | 728593 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 32 | 2 | 28 | 0 | 3168 | 644 | 976 | 0.0 | 0.0180 | 0 | 0 | 652356 |
| High | admin | logged-in-admin | desktop | /vi/admin/disputes | 32 | 2 | 28 | 0 | 5666 | 668 | 1004 | 0.0 | 0.0000 | 0 | 0 | 650098 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 32 | 2 | 28 | 0 | 3413 | 588 | 992 | 0.0 | 0.0039 | 0 | 0 | 726955 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 32 | 3 | 26 | 0 | 3051 | 644 | 1080 | 0.0 | 0.0095 | 0 | 0 | 637842 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 32 | 3 | 27 | 0 | 3683 | 608 | 1012 | 0.0 | 0.0039 | 0 | 0 | 644925 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 32 | 2 | 28 | 0 | 3416 | 772 | 1128 | 0.0 | 0.0071 | 0 | 0 | 728550 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 32 | 3 | 27 | 0 | 3384 | 956 | 1300 | 0.0 | 0.0000 | 0 | 0 | 637429 |
| High | admin | logged-in-admin | mobile | /vi/admin/users | 32 | 1 | 29 | 0 | 3259 | 868 | 1196 | 0.0 | 0.0000 | 0 | 0 | 652453 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 32 | 3 | 27 | 0 | 3472 | 712 | 1032 | 0.0 | 0.0000 | 0 | 0 | 643344 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 31 | 2 | 27 | 0 | 3228 | 664 | 1032 | 0.0 | 0.0432 | 0 | 0 | 644725 |
| High | admin | logged-in-admin | desktop | /vi/admin/gamification | 31 | 0 | 29 | 0 | 3757 | 612 | 956 | 0.0 | 0.0022 | 0 | 0 | 696750 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/f828c893-4746-46f5-8e08-7de94cd84f35 | 31 | 0 | 29 | 0 | 3338 | 664 | 1100 | 0.0 | 0.0042 | 0 | 0 | 712886 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 31 | 3 | 26 | 0 | 3103 | 656 | 1040 | 0.0 | 0.0039 | 0 | 0 | 634874 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 31 | 3 | 26 | 0 | 3090 | 620 | 1008 | 0.0 | 0.0039 | 0 | 0 | 634440 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 31 | 2 | 27 | 0 | 5958 | 704 | 1052 | 0.0 | 0.0000 | 0 | 0 | 645248 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 31 | 3 | 26 | 0 | 3262 | 812 | 1148 | 0.0 | 0.0000 | 0 | 0 | 634796 |
| High | admin | logged-in-admin | mobile | /vi/admin/deposits | 31 | 1 | 28 | 0 | 4209 | 728 | 1056 | 0.0 | 0.0000 | 0 | 0 | 650228 |
| High | admin | logged-in-admin | mobile | /vi/admin/reader-requests | 31 | 1 | 28 | 0 | 3417 | 900 | 1212 | 0.0 | 0.0000 | 0 | 0 | 649102 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 31 | 3 | 26 | 0 | 3178 | 580 | 904 | 0.0 | 0.0000 | 0 | 0 | 635077 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 31 | 3 | 26 | 0 | 3019 | 560 | 900 | 0.0 | 0.0000 | 0 | 0 | 637559 |
| High | admin | logged-in-admin | desktop | /vi/admin/users | 30 | 0 | 28 | 0 | 3115 | 668 | 1040 | 0.0 | 0.0000 | 0 | 0 | 650083 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 30 | 0 | 28 | 0 | 5454 | 1308 | 1308 | 0.0 | 0.0177 | 0 | 0 | 650016 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 3413 | 540 | 868 | 0.0 | 0.0267 | 0 | 0 | 649718 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 30 | 0 | 28 | 0 | 3454 | 532 | 1876 | 0.0 | 0.0051 | 0 | 0 | 642945 |
| High | admin | logged-in-admin | mobile | /vi/admin/gamification | 30 | 0 | 28 | 0 | 2942 | 708 | 1020 | 0.0 | 0.0000 | 0 | 0 | 664709 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/4d68762d-32b0-4770-9e25-bfc9d9cb7e62 | 30 | 0 | 28 | 0 | 3397 | 780 | 1120 | 0.0 | 0.0071 | 0 | 0 | 680962 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/a27600a5-fbc0-4d42-acec-89870a2009c6 | 30 | 0 | 28 | 0 | 3848 | 584 | 920 | 0.0 | 0.0071 | 0 | 0 | 681117 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 30 | 0 | 28 | 0 | 3318 | 588 | 932 | 0.0 | 0.0071 | 0 | 0 | 724401 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2880 | 584 | 912 | 0.0 | 0.0196 | 0 | 0 | 649945 |
| High | admin | logged-in-admin | desktop | /vi/admin | 29 | 0 | 27 | 0 | 3221 | 664 | 1060 | 0.0 | 0.0000 | 0 | 0 | 647730 |
| High | admin | logged-in-admin | desktop | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2794 | 632 | 1172 | 0.0 | 0.0000 | 0 | 0 | 647770 |
| High | admin | logged-in-admin | desktop | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 4319 | 692 | 1032 | 0.0 | 0.0000 | 0 | 0 | 646580 |
| High | admin | logged-in-admin | desktop | /vi/admin/readings | 29 | 0 | 27 | 0 | 4131 | 636 | 1028 | 0.0 | 0.0000 | 0 | 0 | 649054 |
| High | admin | logged-in-admin | desktop | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2926 | 624 | 920 | 0.0 | 0.0000 | 0 | 0 | 646236 |
| High | auth-public | logged-in-reader | desktop | /vi | 29 | 0 | 27 | 0 | 3066 | 1152 | 1152 | 95.0 | 0.0038 | 0 | 0 | 607859 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6501 | 636 | 636 | 0.0 | 0.0040 | 0 | 0 | 642397 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 29 | 1 | 26 | 0 | 3034 | 900 | 1260 | 0.0 | 0.0039 | 0 | 0 | 632773 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 2864 | 692 | 1092 | 0.0 | 0.0430 | 0 | 0 | 642151 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 29 | 1 | 26 | 0 | 3003 | 756 | 1316 | 0.0 | 0.0039 | 0 | 0 | 633979 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 1 | 26 | 0 | 2863 | 600 | 1016 | 0.0 | 0.0039 | 0 | 0 | 632294 |
| High | auth-public | logged-out | mobile | /vi | 29 | 0 | 27 | 0 | 3248 | 996 | 996 | 0.0 | 0.0000 | 0 | 0 | 601217 |
| High | auth-public | logged-in-admin | mobile | /vi | 29 | 0 | 27 | 0 | 3401 | 720 | 1092 | 0.0 | 0.0055 | 0 | 0 | 607837 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 3052 | 760 | 1096 | 0.0 | 0.0000 | 0 | 0 | 642013 |
| High | admin | logged-in-admin | mobile | /vi/admin | 29 | 0 | 27 | 0 | 2848 | 644 | 980 | 0.0 | 0.0000 | 0 | 0 | 647586 |
| High | admin | logged-in-admin | mobile | /vi/admin/disputes | 29 | 0 | 27 | 0 | 3292 | 1056 | 1360 | 0.0 | 0.0000 | 0 | 0 | 645911 |
| High | admin | logged-in-admin | mobile | /vi/admin/readings | 29 | 0 | 27 | 0 | 2981 | 792 | 1128 | 0.0 | 0.0000 | 0 | 0 | 648730 |
| High | admin | logged-in-admin | mobile | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2824 | 744 | 744 | 0.0 | 0.0000 | 0 | 0 | 646112 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5774 | 552 | 552 | 0.0 | 0.0000 | 0 | 0 | 642170 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 29 | 1 | 26 | 0 | 2937 | 568 | 908 | 0.0 | 0.0000 | 0 | 0 | 635380 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 3772 | 704 | 1040 | 0.0 | 0.0071 | 0 | 0 | 642211 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 1 | 26 | 0 | 2832 | 568 | 900 | 0.0 | 0.0000 | 0 | 0 | 634096 |
| High | reading | logged-in-admin | desktop | /vi/reading | 28 | 0 | 26 | 0 | 2870 | 640 | 1048 | 0.0 | 0.0042 | 0 | 0 | 641806 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 3309 | 1232 | 1596 | 0.0 | 0.0042 | 0 | 0 | 631806 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 3465 | 628 | 1048 | 0.0 | 0.0489 | 0 | 0 | 630950 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers | 28 | 0 | 26 | 0 | 3106 | 856 | 1364 | 0.0 | 0.0042 | 0 | 0 | 633940 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 3058 | 612 | 1020 | 0.0 | 0.0042 | 0 | 0 | 632044 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 3307 | 612 | 1088 | 0.0 | 0.0042 | 0 | 0 | 634314 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2725 | 620 | 1008 | 0.0 | 0.0042 | 0 | 0 | 631677 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3065 | 672 | 1264 | 0.0 | 0.0042 | 0 | 0 | 632213 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 3236 | 616 | 1096 | 0.0 | 0.0042 | 0 | 0 | 633083 |
| High | admin | logged-in-admin | desktop | /vi/admin/promotions | 28 | 0 | 26 | 0 | 3002 | 636 | 968 | 0.0 | 0.0000 | 0 | 0 | 644708 |
| High | admin | logged-in-admin | desktop | /vi/admin/system-configs | 28 | 0 | 26 | 0 | 2930 | 628 | 1096 | 0.0 | 0.0000 | 0 | 0 | 655762 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/4cedf788-12bb-4123-9073-992b264c96c2 | 28 | 0 | 26 | 0 | 2843 | 772 | 1172 | 0.0 | 0.0042 | 0 | 0 | 632353 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 3040 | 628 | 996 | 0.0 | 0.0042 | 0 | 0 | 631269 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 3133 | 620 | 1044 | 0.0 | 0.0042 | 0 | 0 | 631319 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 3013 | 860 | 860 | 0.0 | 0.0042 | 0 | 0 | 631485 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2871 | 616 | 1212 | 0.0 | 0.0042 | 0 | 0 | 632889 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 3063 | 932 | 932 | 0.0 | 0.0042 | 0 | 0 | 632980 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 4323 | 624 | 1000 | 0.0 | 0.0042 | 0 | 0 | 633135 |
| High | reading | logged-in-reader | desktop | /vi/reading | 28 | 0 | 26 | 0 | 2885 | 636 | 1112 | 0.0 | 0.0039 | 0 | 0 | 641693 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 3184 | 596 | 1012 | 0.0 | 0.0039 | 0 | 0 | 632657 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 28 | 0 | 26 | 0 | 4867 | 676 | 1136 | 0.0 | 0.0039 | 0 | 0 | 634207 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2803 | 632 | 1040 | 0.0 | 0.0039 | 0 | 0 | 631966 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 2928 | 696 | 1240 | 0.0 | 0.0039 | 0 | 0 | 634288 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3102 | 680 | 1076 | 0.0 | 0.0039 | 0 | 0 | 632057 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2851 | 608 | 1060 | 0.0 | 0.0044 | 0 | 0 | 632336 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2828 | 696 | 1120 | 0.0 | 0.0039 | 0 | 0 | 632774 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/e4197b22-32f1-4a80-9529-3f4c62e3a202 | 28 | 0 | 26 | 0 | 2903 | 668 | 1080 | 0.0 | 0.0043 | 0 | 0 | 632462 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/fa2fc343-c413-437a-b088-9b30ecb4fcd3 | 28 | 0 | 26 | 0 | 2836 | 640 | 1032 | 0.0 | 0.0039 | 0 | 0 | 632581 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 4731 | 624 | 992 | 0.0 | 0.0039 | 0 | 0 | 631655 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2740 | 628 | 1224 | 0.0 | 0.0039 | 0 | 0 | 633235 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2758 | 628 | 1188 | 0.0 | 0.0039 | 0 | 0 | 633379 |
| High | reading | logged-in-admin | mobile | /vi/reading | 28 | 0 | 26 | 0 | 3308 | 1044 | 1388 | 0.0 | 0.0000 | 0 | 0 | 641723 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 3060 | 676 | 1016 | 0.0 | 0.0000 | 0 | 0 | 631543 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2824 | 796 | 1120 | 0.0 | 0.0760 | 0 | 0 | 630885 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 28 | 0 | 26 | 0 | 3823 | 612 | 952 | 0.0 | 0.0071 | 0 | 0 | 633764 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2867 | 724 | 1052 | 0.0 | 0.0000 | 0 | 0 | 631767 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 3003 | 780 | 1204 | 0.0 | 0.0000 | 0 | 0 | 634051 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3556 | 752 | 1092 | 0.0 | 0.0071 | 0 | 0 | 632258 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 3095 | 712 | 1052 | 0.0 | 0.0071 | 0 | 0 | 631115 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 2926 | 872 | 872 | 0.0 | 0.0000 | 0 | 0 | 631940 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2885 | 760 | 1096 | 0.0 | 0.0000 | 0 | 0 | 632636 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2813 | 620 | 952 | 0.0 | 0.0000 | 0 | 0 | 632918 |
| High | admin | logged-in-admin | mobile | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2856 | 636 | 944 | 0.0 | 0.0000 | 0 | 0 | 644577 |
| High | admin | logged-in-admin | mobile | /vi/admin/system-configs | 28 | 0 | 26 | 0 | 2926 | 680 | 1060 | 0.0 | 0.0000 | 0 | 0 | 655641 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/9ad599f4-5a77-4a9e-91df-11084765bf37 | 28 | 0 | 26 | 0 | 2976 | 692 | 1028 | 0.0 | 0.0000 | 0 | 0 | 632351 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2958 | 580 | 904 | 0.0 | 0.0000 | 0 | 0 | 631174 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 4022 | 684 | 1028 | 0.0 | 0.0071 | 0 | 0 | 632702 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2955 | 600 | 944 | 0.0 | 0.0000 | 0 | 0 | 632868 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 3239 | 552 | 892 | 0.0 | 0.0071 | 0 | 0 | 632302 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2698 | 536 | 860 | 0.0 | 0.0000 | 0 | 0 | 631846 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 4379 | 600 | 944 | 0.0 | 0.0071 | 0 | 0 | 631876 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3804 | 568 | 896 | 0.0 | 0.0071 | 0 | 0 | 631965 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2864 | 584 | 940 | 0.0 | 0.0330 | 0 | 0 | 633176 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2927 | 600 | 960 | 0.0 | 0.0000 | 0 | 0 | 632603 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 3284 | 576 | 916 | 0.0 | 0.0071 | 0 | 0 | 632999 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/5b5051d3-906e-4110-8726-89e662180866 | 28 | 0 | 26 | 0 | 2761 | 556 | 880 | 0.0 | 0.0000 | 0 | 0 | 632476 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/31cb1daf-28c3-4dae-beef-f752cffead2e | 28 | 0 | 26 | 0 | 2808 | 564 | 892 | 0.0 | 0.0000 | 0 | 0 | 632308 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/04d92163-9af4-41cc-ab5e-d8e301f6c293 | 28 | 0 | 26 | 0 | 2852 | 620 | 956 | 0.0 | 0.0000 | 0 | 0 | 632654 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 3704 | 616 | 944 | 0.0 | 0.0071 | 0 | 0 | 631523 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2812 | 548 | 876 | 0.0 | 0.0000 | 0 | 0 | 631392 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2762 | 552 | 964 | 0.0 | 0.0000 | 0 | 0 | 633023 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2748 | 564 | 916 | 0.0 | 0.0000 | 0 | 0 | 633345 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/tos | 27 | 1 | 23 | 0 | 3053 | 612 | 952 | 0.0 | 0.0020 | 0 | 0 | 527940 |
| High | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 27 | 1 | 23 | 0 | 4174 | 660 | 992 | 0.0 | 0.0019 | 0 | 0 | 528064 |
| High | auth-public | logged-in-reader | mobile | /vi/legal/tos | 27 | 1 | 23 | 0 | 4184 | 588 | 900 | 0.0 | 0.0032 | 0 | 0 | 528260 |
| High | auth-public | logged-in-reader | mobile | /vi | 26 | 0 | 24 | 0 | 2707 | 580 | 948 | 0.0 | 0.0032 | 0 | 0 | 537660 |
| High | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3505 | 652 | 652 | 0.0 | 0.0000 | 0 | 0 | 526055 |
| High | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 5171 | 736 | 736 | 0.0 | 0.0000 | 0 | 0 | 525995 |
| High | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3290 | 588 | 588 | 0.0 | 0.0000 | 0 | 0 | 526046 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3348 | 672 | 1008 | 0.0 | 0.0055 | 0 | 0 | 526136 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3027 | 900 | 1212 | 0.0 | 0.0032 | 0 | 0 | 526258 |
| High | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3374 | 584 | 900 | 0.0 | 0.0055 | 0 | 0 | 526440 |
| High | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 5162 | 632 | 632 | 0.0 | 0.0000 | 0 | 0 | 512983 |
| High | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 3695 | 664 | 664 | 0.0 | 0.0000 | 0 | 0 | 511984 |
| High | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 4291 | 608 | 608 | 0.0 | 0.0000 | 0 | 0 | 512197 |
| High | auth-public | logged-in-admin | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 3437 | 736 | 736 | 0.0 | 0.0000 | 0 | 0 | 512080 |
| High | auth-public | logged-in-admin | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 4384 | 656 | 656 | 0.0 | 0.0000 | 0 | 0 | 512194 |
| High | auth-public | logged-in-reader | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 3171 | 676 | 676 | 0.0 | 0.0000 | 0 | 0 | 512062 |
| High | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 3492 | 572 | 572 | 0.0 | 0.0000 | 0 | 0 | 512341 |
| High | auth-public | logged-out | mobile | /vi/register | 24 | 0 | 22 | 0 | 3194 | 576 | 576 | 0.0 | 0.0000 | 0 | 0 | 512972 |
| High | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 3933 | 564 | 564 | 0.0 | 0.0000 | 0 | 0 | 511969 |
| High | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 3591 | 544 | 544 | 0.0 | 0.0000 | 0 | 0 | 512212 |
| High | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 3662 | 576 | 576 | 0.0 | 0.0000 | 0 | 0 | 512185 |
| High | auth-public | logged-in-admin | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 3587 | 872 | 872 | 0.0 | 0.0000 | 0 | 0 | 512109 |
| High | auth-public | logged-in-admin | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 3890 | 1456 | 1456 | 0.0 | 0.0000 | 0 | 0 | 512208 |
| High | auth-public | logged-in-reader | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 3565 | 580 | 580 | 0.0 | 0.0000 | 0 | 0 | 511985 |
| High | auth-public | logged-in-reader | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 3202 | 564 | 564 | 0.0 | 0.0000 | 0 | 0 | 512230 |
| Medium | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2763 | 680 | 680 | 0.0 | 0.0000 | 0 | 0 | 526041 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2848 | 812 | 1120 | 0.0 | 0.0020 | 0 | 0 | 526166 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3055 | 596 | 908 | 0.0 | 0.0020 | 0 | 0 | 526450 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2777 | 652 | 956 | 0.0 | 0.0019 | 0 | 0 | 526474 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2773 | 600 | 952 | 0.0 | 0.0019 | 0 | 0 | 526558 |
| Medium | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2730 | 612 | 612 | 0.0 | 0.0000 | 0 | 0 | 525970 |
| Medium | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3061 | 692 | 692 | 0.0 | 0.0000 | 0 | 0 | 525968 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2859 | 752 | 1072 | 0.0 | 0.0032 | 0 | 0 | 526101 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2711 | 548 | 860 | 0.0 | 0.0032 | 0 | 0 | 526380 |
| Medium | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 2921 | 700 | 700 | 0.0 | 0.0000 | 0 | 0 | 512332 |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2738 | 608 | 608 | 0.0 | 0.0000 | 0 | 0 | 512106 |
| Medium | auth-public | logged-in-reader | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2845 | 716 | 716 | 0.0 | 0.0001 | 0 | 0 | 512136 |

## Major Issues Found

- Critical: 27 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: 151 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 12 page(s) có request trong dải 400-800ms.
- Duplicate: 207 nhóm duplicate request cần kiểm tra over-fetch/cache key.
- Pending: 10 page(s) có pending request không phải websocket/eventsource.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5745 | 4751 | static | https://www.tarotnow.xyz/_next/static/chunks/0y-5.-cr3v3~z.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5730 | 4708 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5662 | 5209 | static | https://www.tarotnow.xyz/_next/static/chunks/064xhtgi5eo9n.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5602 | 2300 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5531 | 1559 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5374 | 5210 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5374 | 5214 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5336 | 5214 | static | https://www.tarotnow.xyz/_next/static/chunks/0rm0_6_wsunhe.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5323 | 4389 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5313 | 5214 | static | https://www.tarotnow.xyz/_next/static/chunks/0_yn3f1mymbbi.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5295 | 4395 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5244 | 4387 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5242 | 2317 | static | https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5233 | 5214 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5193 | 4361 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 5176 | 4388 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 4817 | 4793 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 4726 | 2317 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 4155 | 3323 | static | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 4018 | 3369 | static | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 3812 | 3398 | static | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | GET | 200 | 3632 | 328 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | admin | logged-in-admin | desktop | /vi/admin/disputes | GET | 200 | 3253 | 328 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | GET | 200 | 3039 | 3023 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | GET | 200 | 2785 | 932 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-out | desktop | /vi/register | GET | 200 | 2749 | 2729 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 2487 | 2317 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers | GET | 200 | 2466 | 2447 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 2372 | 2317 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 2352 | 2325 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 2288 | 271 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2242 | 515 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 2195 | 1435 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | GET | 200 | 2173 | 1108 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 2059 | 2044 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 2036 | 251 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-admin | desktop | /vi/verify-email | GET | 200 | 1994 | 1963 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1981 | 351 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1956 | 410 | html | https://www.tarotnow.xyz/vi/login |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | GET | 200 | 1952 | 1928 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1938 | 1642 | html | https://www.tarotnow.xyz/vi |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | GET | 200 | 1925 | 1624 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1919 | 640 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | admin | logged-in-admin | desktop | /vi/admin/reader-requests | GET | 200 | 1917 | 1886 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-out | desktop | /vi/verify-email | GET | 200 | 1904 | 1895 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1832 | 398 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/legal/privacy | GET | 200 | 1831 | 1683 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/tos | GET | 200 | 1829 | 310 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1811 | 152 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1789 | 81 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | GET | 200 | 1786 | 662 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | GET | 200 | 1775 | 1738 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | admin | logged-in-admin | desktop | /vi/admin/readings | GET | 200 | 1729 | 1709 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1727 | 173 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1721 | 419 | static | https://www.tarotnow.xyz/_next/static/chunks/0y-5.-cr3v3~z.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | GET | 200 | 1719 | 247 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1711 | 419 | static | https://www.tarotnow.xyz/_next/static/chunks/064xhtgi5eo9n.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1710 | 420 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1699 | 88 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/deposits | GET | 200 | 1672 | 336 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1633 | 81 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1622 | 421 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1619 | 442 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1618 | 494 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1612 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1606 | 266 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1605 | 421 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading | GET | 200 | 1605 | 1450 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1598 | 420 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-out | mobile | /vi/forgot-password | GET | 200 | 1594 | 709 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1584 | 155 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | GET | 200 | 1541 | 312 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1535 | 181 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | GET | 200 | 1529 | 266 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1524 | 80 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | GET | 200 | 1520 | 220 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1510 | 188 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 1501 | 1479 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1497 | 136 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1491 | 279 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0y-5.-cr3v3~z.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/064xhtgi5eo9n.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-admin | desktop | /vi/login | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0y-5.-cr3v3~z.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/064xhtgi5eo9n.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| High | auth-public | logged-in-reader | desktop | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| Critical | auth-public | logged-out | desktop | /vi | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0y-5.-cr3v3~z.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/064xhtgi5eo9n.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0sa2w0m9.t3c8.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/wallet/balance |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/chat/unread-count |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/notifications/unread-count |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/vi |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | mobile | /vi/reading | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

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

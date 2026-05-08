# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-08T13:40:35.715Z
- Benchmark generated at (UTC): 2026-05-08T13:40:07.341Z
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 194
- Critical pages: 29
- High pages: 129
- Medium pages: 36
- Slow requests >800ms: 110
- Slow requests 400-800ms: 455
- Request thresholds: >25 suspicious, >35 severe
- Slow request thresholds: >400ms optimize, >800ms serious

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2954 | 0 | 0 | yes |
| logged-in-admin | desktop | 49 | 30.1 | 3092 | 0 | 4 | yes |
| logged-in-reader | desktop | 39 | 29.4 | 3057 | 0 | 2 | yes |
| logged-out | mobile | 9 | 25.1 | 2848 | 0 | 1 | yes |
| logged-in-admin | mobile | 49 | 30.6 | 3029 | 0 | 11 | yes |
| logged-in-reader | mobile | 39 | 31.0 | 3280 | 0 | 5 | yes |

## Route Family Coverage

| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 3106 | 0 | 0 |
| logged-in-admin | desktop | auth-public | 8 | 31.3 | 2897 | 0 | 2 |
| logged-in-admin | desktop | community-leaderboard-quest | 4 | 30.0 | 3453 | 0 | 2 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2787 | 0 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | 4 | 33.8 | 4157 | 0 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | 8 | 29.6 | 3008 | 0 | 0 |
| logged-in-admin | desktop | reader-chat | 9 | 29.2 | 2870 | 0 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.8 | 2834 | 0 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.5 | 2890 | 0 | 0 |
| logged-in-admin | mobile | auth-public | 8 | 35.3 | 3140 | 0 | 3 |
| logged-in-admin | mobile | community-leaderboard-quest | 4 | 24.8 | 3212 | 0 | 1 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2684 | 0 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | 4 | 31.8 | 3552 | 0 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | 8 | 29.5 | 2983 | 0 | 0 |
| logged-in-admin | mobile | reader-chat | 9 | 32.6 | 2965 | 0 | 7 |
| logged-in-admin | mobile | reading | 5 | 28.4 | 2826 | 0 | 0 |
| logged-in-reader | desktop | auth-public | 8 | 28.0 | 2930 | 0 | 1 |
| logged-in-reader | desktop | community-leaderboard-quest | 4 | 25.0 | 2958 | 0 | 1 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2807 | 0 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | 4 | 35.3 | 4022 | 0 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | 8 | 30.1 | 2998 | 0 | 0 |
| logged-in-reader | desktop | reader-chat | 9 | 28.9 | 2855 | 0 | 0 |
| logged-in-reader | desktop | reading | 5 | 30.6 | 3081 | 0 | 0 |
| logged-in-reader | mobile | auth-public | 8 | 34.5 | 3101 | 0 | 3 |
| logged-in-reader | mobile | community-leaderboard-quest | 4 | 26.3 | 3147 | 0 | 2 |
| logged-in-reader | mobile | home | 1 | 41.0 | 4338 | 0 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | 4 | 33.3 | 3949 | 0 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | 8 | 30.0 | 3012 | 0 | 0 |
| logged-in-reader | mobile | reader-chat | 9 | 30.0 | 3427 | 0 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.0 | 3092 | 0 | 0 |
| logged-out | desktop | auth-public | 8 | 24.4 | 2869 | 0 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3630 | 0 | 0 |
| logged-out | mobile | auth-public | 8 | 24.4 | 2790 | 0 | 0 |
| logged-out | mobile | home | 1 | 31.0 | 3311 | 0 | 1 |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | 59 | 5 | 49 | 0 | 3598 | 540 | 900 | 0.0 | 0.0051 | 0 | 1 | 1113443 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/chat | 57 | 4 | 49 | 0 | 3649 | 868 | 1224 | 0.0 | 0.0051 | 0 | 3 | 1135277 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | 53 | 0 | 49 | 0 | 4470 | 680 | 1212 | 56.0 | 0.0038 | 0 | 1 | 1108448 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | 53 | 0 | 49 | 0 | 3497 | 720 | 720 | 0.0 | 0.0055 | 0 | 1 | 1108091 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | 50 | 0 | 46 | 0 | 3220 | 584 | 1096 | 148.0 | 0.0035 | 0 | 1 | 1038118 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | 50 | 0 | 46 | 0 | 3213 | 604 | 1084 | 18.0 | 0.0035 | 0 | 1 | 1037911 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | 50 | 0 | 46 | 0 | 3227 | 532 | 896 | 0.0 | 0.0051 | 0 | 1 | 1037993 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | 50 | 0 | 46 | 0 | 3345 | 604 | 956 | 0.0 | 0.0051 | 0 | 1 | 1038080 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | 50 | 0 | 46 | 0 | 3316 | 552 | 904 | 0.0 | 0.0055 | 0 | 1 | 1038078 |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | 50 | 0 | 46 | 0 | 3314 | 564 | 924 | 0.0 | 0.0055 | 0 | 1 | 1037952 |
| Critical | auth-public | logged-in-reader | mobile | /vi | 41 | 8 | 30 | 0 | 4338 | 620 | 972 | 0.0 | 0.0032 | 0 | 0 | 644743 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/e45cd035-4284-42a9-a061-e51f15d14232 | 40 | 6 | 32 | 0 | 4128 | 716 | 3364 | 0.0 | 0.0039 | 0 | 0 | 739768 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 38 | 3 | 33 | 0 | 3378 | 584 | 960 | 0.0 | 0.0039 | 0 | 0 | 737232 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 38 | 3 | 33 | 0 | 3945 | 612 | 1880 | 0.0 | 0.0039 | 0 | 1 | 780090 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 38 | 3 | 33 | 0 | 3181 | 592 | 988 | 0.0 | 0.0039 | 0 | 0 | 737146 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 38 | 2 | 34 | 0 | 3507 | 616 | 960 | 0.0 | 0.0071 | 0 | 0 | 802411 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 37 | 5 | 30 | 0 | 3688 | 632 | 968 | 0.0 | 0.0039 | 0 | 0 | 664599 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/09d47355-7176-48b9-a07b-a5049c924895 | 37 | 4 | 31 | 0 | 3075 | 584 | 940 | 0.0 | 0.0039 | 0 | 0 | 728862 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | 37 | 6 | 28 | 0 | 3149 | 520 | 852 | 0.0 | 0.0000 | 0 | 4 | 647241 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 36 | 5 | 29 | 0 | 3676 | 756 | 1100 | 0.0 | 0.0039 | 0 | 0 | 658241 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 36 | 1 | 33 | 0 | 2912 | 600 | 980 | 0.0 | 0.0039 | 0 | 0 | 653400 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 36 | 1 | 33 | 0 | 3018 | 640 | 996 | 0.0 | 0.0000 | 0 | 0 | 799252 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers | 36 | 5 | 29 | 0 | 5918 | 552 | 880 | 0.0 | 0.0000 | 0 | 0 | 656179 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 35 | 0 | 33 | 0 | 3589 | 736 | 1560 | 0.0 | 0.0000 | 0 | 1 | 776962 |
| Critical | auth-public | logged-out | mobile | /vi | 31 | 2 | 27 | 0 | 3311 | 988 | 988 | 0.0 | 0.0000 | 0 | 1 | 603588 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community/69db54fc297f66f734421a3c | 9 | 0 | 7 | 0 | 2501 | 376 | 376 | 0.0 | 0.0000 | 0 | 1 | 166149 |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/community/69db54fc297f66f734421a3c | 9 | 0 | 7 | 0 | 2599 | 424 | 424 | 0.0 | 0.0000 | 0 | 1 | 166163 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community/69db54fc297f66f734421a3c | 9 | 0 | 7 | 0 | 3582 | 1352 | 1352 | 0.0 | 0.0000 | 0 | 1 | 166155 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community/69db54fc297f66f734421a3c | 9 | 0 | 7 | 0 | 3056 | 332 | 332 | 0.0 | 0.0000 | 0 | 1 | 166179 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 35 | 5 | 28 | 0 | 3500 | 632 | 968 | 0.0 | 0.0039 | 0 | 0 | 645928 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 35 | 5 | 28 | 0 | 3484 | 616 | 1104 | 0.0 | 0.0039 | 0 | 0 | 645988 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 35 | 5 | 28 | 0 | 3321 | 560 | 888 | 0.0 | 0.0000 | 0 | 0 | 650036 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 35 | 0 | 33 | 0 | 3643 | 636 | 976 | 0.0 | 0.0071 | 0 | 0 | 661497 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 35 | 5 | 28 | 0 | 3459 | 684 | 1104 | 0.0 | 0.0000 | 0 | 0 | 648745 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 34 | 3 | 29 | 0 | 3361 | 604 | 1656 | 0.0 | 0.0039 | 0 | 0 | 648989 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 34 | 4 | 28 | 0 | 6557 | 604 | 968 | 0.0 | 0.0040 | 0 | 0 | 647261 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 34 | 3 | 28 | 0 | 3063 | 604 | 972 | 0.0 | 0.0726 | 0 | 0 | 641721 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 34 | 3 | 28 | 0 | 3319 | 564 | 1324 | 0.0 | 0.0821 | 0 | 0 | 650442 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 33 | 3 | 28 | 0 | 3671 | 596 | 1736 | 0.0 | 0.0039 | 0 | 0 | 645975 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 33 | 3 | 28 | 0 | 3438 | 636 | 1272 | 0.0 | 0.0039 | 0 | 0 | 727440 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 33 | 3 | 28 | 0 | 3083 | 588 | 920 | 0.0 | 0.0000 | 0 | 0 | 651749 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 32 | 3 | 27 | 0 | 6791 | 568 | 1388 | 0.0 | 0.0040 | 0 | 0 | 646238 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 32 | 3 | 27 | 0 | 2973 | 616 | 984 | 0.0 | 0.0544 | 0 | 0 | 639676 |
| High | admin | logged-in-admin | desktop | /vi/admin/gamification | 32 | 0 | 30 | 0 | 3841 | 620 | 964 | 0.0 | 0.0022 | 0 | 0 | 698713 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 32 | 3 | 27 | 0 | 3060 | 736 | 1100 | 0.0 | 0.0039 | 0 | 0 | 636065 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 32 | 3 | 27 | 0 | 3182 | 560 | 888 | 0.0 | 0.0000 | 0 | 0 | 637032 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 3097 | 620 | 1004 | 0.0 | 0.0039 | 0 | 0 | 727238 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 31 | 3 | 26 | 0 | 3149 | 572 | 956 | 0.0 | 0.0486 | 0 | 0 | 634205 |
| High | admin | logged-in-admin | desktop | /vi/admin/disputes | 31 | 1 | 28 | 0 | 3153 | 608 | 932 | 0.0 | 0.0000 | 0 | 0 | 649138 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 31 | 3 | 26 | 0 | 2984 | 612 | 1084 | 0.0 | 0.0039 | 0 | 0 | 636432 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 31 | 1 | 28 | 0 | 2919 | 584 | 952 | 0.0 | 0.0177 | 0 | 0 | 650852 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 31 | 2 | 27 | 0 | 3010 | 628 | 1348 | 0.0 | 0.0039 | 0 | 0 | 637573 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 31 | 2 | 27 | 0 | 3100 | 604 | 1396 | 0.0 | 0.0095 | 0 | 0 | 636341 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 31 | 1 | 28 | 0 | 2820 | 548 | 880 | 0.0 | 0.0000 | 0 | 0 | 645616 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 31 | 1 | 28 | 0 | 2861 | 520 | 944 | 0.0 | 0.0000 | 0 | 0 | 727133 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 31 | 1 | 28 | 0 | 2912 | 556 | 888 | 0.0 | 0.0196 | 0 | 0 | 650963 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 31 | 2 | 27 | 0 | 2963 | 548 | 916 | 0.0 | 0.0000 | 0 | 0 | 637025 |
| High | admin | logged-in-admin | mobile | /vi/admin/gamification | 31 | 1 | 28 | 0 | 2827 | 540 | 852 | 0.0 | 0.0000 | 0 | 0 | 665979 |
| High | admin | logged-in-admin | mobile | /vi/admin/reader-requests | 31 | 1 | 28 | 0 | 3038 | 544 | 856 | 0.0 | 0.0000 | 0 | 0 | 649243 |
| High | reading | logged-in-reader | mobile | /vi/reading | 31 | 2 | 27 | 0 | 3120 | 584 | 1104 | 0.0 | 0.0000 | 0 | 0 | 645849 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 31 | 1 | 28 | 0 | 3082 | 584 | 916 | 0.0 | 0.0071 | 0 | 0 | 725490 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 31 | 1 | 28 | 0 | 2922 | 572 | 908 | 0.0 | 0.0196 | 0 | 0 | 650799 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 31 | 2 | 27 | 0 | 2861 | 664 | 1020 | 0.0 | 0.0330 | 0 | 0 | 637237 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | 2 | 27 | 0 | 3225 | 596 | 932 | 0.0 | 0.0000 | 0 | 0 | 636912 |
| High | admin | logged-in-admin | desktop | /vi/admin/users | 30 | 0 | 28 | 0 | 2991 | 728 | 1080 | 0.0 | 0.0000 | 0 | 0 | 650094 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 30 | 0 | 28 | 0 | 3451 | 604 | 1732 | 0.0 | 0.0039 | 0 | 0 | 643338 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 2862 | 608 | 1000 | 0.0 | 0.0190 | 0 | 0 | 644782 |
| High | reading | logged-in-admin | mobile | /vi/reading | 30 | 2 | 26 | 0 | 2911 | 548 | 888 | 0.0 | 0.0000 | 0 | 0 | 644048 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 30 | 0 | 28 | 0 | 3436 | 572 | 1872 | 0.0 | 0.0051 | 0 | 0 | 643395 |
| High | admin | logged-in-admin | mobile | /vi/admin/system-configs | 30 | 0 | 28 | 0 | 3044 | 628 | 956 | 0.0 | 0.0000 | 0 | 0 | 689450 |
| High | admin | logged-in-admin | mobile | /vi/admin/users | 30 | 0 | 28 | 0 | 2863 | 556 | 1188 | 0.0 | 0.0000 | 0 | 0 | 650152 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 30 | 1 | 27 | 0 | 3021 | 764 | 1092 | 0.0 | 0.0000 | 0 | 0 | 645009 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 30 | 2 | 26 | 0 | 2840 | 768 | 768 | 0.0 | 0.0000 | 0 | 0 | 634597 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/5ee1ae7a-f9a3-4a46-974b-109190abef8a | 30 | 0 | 28 | 0 | 3384 | 576 | 912 | 0.0 | 0.0071 | 0 | 0 | 681217 |
| High | auth-public | logged-out | desktop | /vi | 29 | 0 | 27 | 0 | 3630 | 1012 | 1012 | 285.0 | 0.0000 | 0 | 0 | 601313 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers | 29 | 1 | 26 | 0 | 2827 | 624 | 976 | 0.0 | 0.0039 | 0 | 0 | 635519 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 29 | 1 | 26 | 0 | 2837 | 608 | 1160 | 0.0 | 0.0039 | 0 | 0 | 635318 |
| High | admin | logged-in-admin | desktop | /vi/admin | 29 | 0 | 27 | 0 | 3542 | 624 | 1004 | 0.0 | 0.0000 | 0 | 0 | 647474 |
| High | admin | logged-in-admin | desktop | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2950 | 624 | 1056 | 0.0 | 0.0000 | 0 | 0 | 647794 |
| High | admin | logged-in-admin | desktop | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2847 | 624 | 964 | 0.0 | 0.0000 | 0 | 0 | 646656 |
| High | admin | logged-in-admin | desktop | /vi/admin/readings | 29 | 0 | 27 | 0 | 3007 | 704 | 1124 | 0.0 | 0.0000 | 0 | 0 | 648927 |
| High | admin | logged-in-admin | desktop | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2881 | 644 | 992 | 0.0 | 0.0000 | 0 | 0 | 646275 |
| High | reading | logged-in-reader | desktop | /vi/reading | 29 | 1 | 26 | 0 | 2932 | 616 | 984 | 0.0 | 0.0039 | 0 | 0 | 642951 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 29 | 1 | 26 | 0 | 2881 | 644 | 1012 | 0.0 | 0.0039 | 0 | 0 | 635365 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 29 | 1 | 26 | 0 | 2839 | 644 | 1000 | 0.0 | 0.0040 | 0 | 0 | 633509 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5507 | 604 | 604 | 0.0 | 0.0000 | 0 | 0 | 643479 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 2917 | 548 | 872 | 0.0 | 0.0000 | 0 | 0 | 643756 |
| High | admin | logged-in-admin | mobile | /vi/admin | 29 | 0 | 27 | 0 | 3494 | 568 | 884 | 0.0 | 0.0000 | 0 | 0 | 647633 |
| High | admin | logged-in-admin | mobile | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2692 | 528 | 852 | 0.0 | 0.0000 | 0 | 0 | 647680 |
| High | admin | logged-in-admin | mobile | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2666 | 532 | 836 | 0.0 | 0.0000 | 0 | 0 | 645945 |
| High | admin | logged-in-admin | mobile | /vi/admin/readings | 29 | 0 | 27 | 0 | 2762 | 556 | 892 | 0.0 | 0.0000 | 0 | 0 | 648979 |
| High | admin | logged-in-admin | mobile | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2787 | 592 | 904 | 0.0 | 0.0000 | 0 | 0 | 646182 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5563 | 564 | 564 | 0.0 | 0.0000 | 0 | 0 | 642278 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 29 | 1 | 26 | 0 | 2808 | 660 | 992 | 0.0 | 0.0000 | 0 | 0 | 632847 |
| High | reading | logged-in-admin | desktop | /vi/reading | 28 | 0 | 26 | 0 | 2857 | 588 | 980 | 0.0 | 0.0039 | 0 | 0 | 642065 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2703 | 576 | 932 | 0.0 | 0.0039 | 0 | 0 | 631562 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2711 | 564 | 960 | 0.0 | 0.0039 | 0 | 0 | 631849 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2777 | 624 | 992 | 0.0 | 0.0039 | 0 | 0 | 631700 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 3192 | 648 | 1160 | 0.0 | 0.0039 | 0 | 0 | 631190 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2760 | 692 | 1044 | 0.0 | 0.0044 | 0 | 0 | 632487 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2748 | 664 | 1008 | 0.0 | 0.0039 | 0 | 0 | 632486 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2824 | 584 | 1048 | 0.0 | 0.0039 | 0 | 0 | 633023 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/tos | 28 | 1 | 24 | 0 | 3209 | 696 | 992 | 0.0 | 0.0019 | 0 | 0 | 528961 |
| High | admin | logged-in-admin | desktop | /vi/admin/promotions | 28 | 0 | 26 | 0 | 3010 | 616 | 992 | 0.0 | 0.0000 | 0 | 0 | 644751 |
| High | admin | logged-in-admin | desktop | /vi/admin/system-configs | 28 | 0 | 26 | 0 | 2841 | 648 | 1072 | 0.0 | 0.0000 | 0 | 0 | 655982 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/d7608b10-f43f-4f2f-8ce2-211eb46881fe | 28 | 0 | 26 | 0 | 2745 | 644 | 1004 | 0.0 | 0.0039 | 0 | 0 | 632492 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/dab1af0a-2e0f-461d-8199-7eabfb4197fd | 28 | 0 | 26 | 0 | 2670 | 584 | 944 | 0.0 | 0.0039 | 0 | 0 | 632336 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2689 | 584 | 912 | 0.0 | 0.0039 | 0 | 0 | 631108 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2706 | 584 | 948 | 0.0 | 0.0039 | 0 | 0 | 631285 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2943 | 644 | 1152 | 0.0 | 0.0039 | 0 | 0 | 632919 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2724 | 788 | 868 | 0.0 | 0.0039 | 0 | 0 | 633255 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 3398 | 628 | 992 | 0.0 | 0.0039 | 0 | 0 | 631836 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2723 | 600 | 1276 | 0.0 | 0.0039 | 0 | 0 | 632713 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2712 | 572 | 976 | 0.0 | 0.0039 | 0 | 0 | 631774 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2788 | 636 | 1016 | 0.0 | 0.0039 | 0 | 0 | 631856 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2726 | 588 | 988 | 0.0 | 0.0039 | 0 | 0 | 632587 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2747 | 604 | 1152 | 0.0 | 0.0039 | 0 | 0 | 632938 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/990ae2a5-857f-4dcc-bef8-6905f735bbd1 | 28 | 0 | 26 | 0 | 2766 | 616 | 968 | 0.0 | 0.0039 | 0 | 0 | 632520 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/3ec512b4-a850-41cb-897f-3e4d6efac79f | 28 | 0 | 26 | 0 | 2831 | 636 | 1008 | 0.0 | 0.0039 | 0 | 0 | 632491 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2723 | 648 | 984 | 0.0 | 0.0039 | 0 | 0 | 631390 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2725 | 636 | 996 | 0.0 | 0.0039 | 0 | 0 | 631283 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2801 | 660 | 1152 | 0.0 | 0.0039 | 0 | 0 | 633130 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2814 | 636 | 1104 | 0.0 | 0.0039 | 0 | 0 | 633271 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2825 | 696 | 1224 | 0.0 | 0.0039 | 0 | 0 | 633435 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2729 | 568 | 900 | 0.0 | 0.0000 | 0 | 0 | 631729 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2718 | 580 | 924 | 0.0 | 0.0757 | 0 | 0 | 631015 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2747 | 532 | 864 | 0.0 | 0.0000 | 0 | 0 | 631716 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2737 | 528 | 856 | 0.0 | 0.0000 | 0 | 0 | 632227 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2811 | 588 | 912 | 0.0 | 0.0069 | 0 | 0 | 631073 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 3719 | 544 | 872 | 0.0 | 0.0072 | 0 | 0 | 632244 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2804 | 532 | 860 | 0.0 | 0.0000 | 0 | 0 | 632855 |
| High | admin | logged-in-admin | mobile | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2729 | 560 | 872 | 0.0 | 0.0000 | 0 | 0 | 644764 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/b3786d7f-1f99-42aa-93ab-9c69c9bff3ff | 28 | 0 | 26 | 0 | 2805 | 536 | 860 | 0.0 | 0.0000 | 0 | 0 | 632350 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/4d8e7a80-3e53-49f9-88bf-8fd4020205ed | 28 | 0 | 26 | 0 | 2790 | 576 | 896 | 0.0 | 0.0000 | 0 | 0 | 632491 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/9bf8b60e-afc7-450a-9f45-0e8610f17c60 | 28 | 0 | 26 | 0 | 2821 | 584 | 916 | 0.0 | 0.0000 | 0 | 0 | 632438 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2805 | 580 | 900 | 0.0 | 0.0000 | 0 | 0 | 631619 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2857 | 560 | 884 | 0.0 | 0.0000 | 0 | 0 | 631261 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2785 | 656 | 984 | 0.0 | 0.0000 | 0 | 0 | 631219 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2703 | 548 | 876 | 0.0 | 0.0000 | 0 | 0 | 632776 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2771 | 548 | 932 | 0.0 | 0.0000 | 0 | 0 | 632900 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 3006 | 552 | 888 | 0.0 | 0.0069 | 0 | 0 | 633270 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2832 | 604 | 932 | 0.0 | 0.0000 | 0 | 0 | 632859 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2911 | 624 | 948 | 0.0 | 0.0000 | 0 | 0 | 631893 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 2916 | 568 | 988 | 0.0 | 0.0000 | 0 | 0 | 634410 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3341 | 1352 | 1352 | 0.0 | 0.0000 | 0 | 0 | 632114 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 3305 | 1280 | 1604 | 0.0 | 0.0000 | 0 | 0 | 632598 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 3260 | 1264 | 1264 | 0.0 | 0.0000 | 0 | 0 | 633123 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/11865119-b59e-409c-a5ec-40acec724d21 | 28 | 0 | 26 | 0 | 2857 | 680 | 1004 | 0.0 | 0.0000 | 0 | 0 | 632515 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/d86ce147-b023-4d99-a3bb-b3f6d9d84f93 | 28 | 0 | 26 | 0 | 2839 | 588 | 924 | 0.0 | 0.0000 | 0 | 0 | 632482 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 3631 | 596 | 932 | 0.0 | 0.0071 | 0 | 0 | 631341 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2855 | 552 | 864 | 0.0 | 0.0000 | 0 | 0 | 631449 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2835 | 608 | 928 | 0.0 | 0.0000 | 0 | 0 | 631341 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2701 | 548 | 884 | 0.0 | 0.0000 | 0 | 0 | 633228 |
| High | auth-public | logged-in-admin | desktop | /vi | 26 | 0 | 24 | 0 | 2787 | 684 | 1188 | 132.0 | 0.0035 | 0 | 0 | 537603 |
| High | auth-public | logged-in-reader | desktop | /vi | 26 | 0 | 24 | 0 | 2807 | 640 | 1148 | 147.0 | 0.0033 | 0 | 0 | 537661 |
| High | auth-public | logged-in-admin | mobile | /vi | 26 | 0 | 24 | 0 | 2684 | 592 | 944 | 0.0 | 0.0028 | 0 | 0 | 537864 |
| High | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 3403 | 628 | 628 | 0.0 | 0.0000 | 0 | 0 | 512329 |
| High | auth-public | logged-in-admin | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 3957 | 720 | 720 | 0.0 | 0.0000 | 0 | 0 | 512136 |
| Medium | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2837 | 604 | 604 | 0.0 | 0.0000 | 0 | 0 | 525979 |
| Medium | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2781 | 616 | 616 | 0.0 | 0.0000 | 0 | 0 | 526023 |
| Medium | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2840 | 608 | 608 | 0.0 | 0.0000 | 0 | 0 | 526145 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2696 | 652 | 964 | 0.0 | 0.0019 | 0 | 0 | 526314 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2710 | 584 | 916 | 0.0 | 0.0019 | 0 | 0 | 526426 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2707 | 608 | 960 | 0.0 | 0.0019 | 0 | 0 | 526515 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2711 | 612 | 896 | 0.0 | 0.0019 | 0 | 0 | 526156 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2661 | 652 | 960 | 0.0 | 0.0019 | 0 | 0 | 526390 |
| Medium | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2722 | 588 | 588 | 0.0 | 0.0000 | 0 | 0 | 526083 |
| Medium | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2761 | 596 | 596 | 0.0 | 0.0000 | 0 | 0 | 526009 |
| Medium | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2745 | 588 | 588 | 0.0 | 0.0000 | 0 | 0 | 526166 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2756 | 536 | 856 | 0.0 | 0.0028 | 0 | 0 | 526178 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2730 | 536 | 844 | 0.0 | 0.0028 | 0 | 0 | 526053 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2861 | 544 | 872 | 0.0 | 0.0028 | 0 | 0 | 526372 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2967 | 948 | 1260 | 0.0 | 0.0032 | 0 | 0 | 526299 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3097 | 1072 | 1388 | 0.0 | 0.0032 | 0 | 0 | 526445 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3114 | 700 | 700 | 0.0 | 0.0032 | 0 | 0 | 526503 |
| Medium | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 2838 | 688 | 688 | 0.0 | 0.0000 | 0 | 0 | 512512 |
| Medium | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 2742 | 644 | 644 | 0.0 | 0.0000 | 0 | 0 | 512974 |
| Medium | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2693 | 592 | 592 | 0.0 | 0.0000 | 0 | 0 | 511997 |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2821 | 592 | 592 | 0.0 | 0.0000 | 0 | 0 | 512161 |
| Medium | auth-public | logged-in-admin | desktop | /vi/login | 24 | 0 | 22 | 0 | 2761 | 632 | 1136 | 130.0 | 0.0035 | 0 | 0 | 511043 |
| Medium | auth-public | logged-in-admin | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2650 | 664 | 664 | 0.0 | 0.0000 | 0 | 0 | 512193 |
| Medium | auth-public | logged-in-admin | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2719 | 680 | 680 | 0.0 | 0.0000 | 0 | 0 | 512292 |
| Medium | auth-public | logged-in-reader | desktop | /vi/register | 24 | 0 | 22 | 0 | 2820 | 636 | 1148 | 108.0 | 0.0033 | 0 | 0 | 511183 |
| Medium | auth-public | logged-in-reader | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2716 | 908 | 908 | 58.0 | 0.0038 | 0 | 0 | 511124 |
| Medium | auth-public | logged-in-reader | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2658 | 664 | 664 | 0.0 | 0.0000 | 0 | 0 | 512233 |
| Medium | auth-public | logged-in-reader | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2696 | 720 | 720 | 0.0 | 0.0000 | 0 | 0 | 512150 |
| Medium | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 2802 | 736 | 736 | 0.0 | 0.0000 | 0 | 0 | 512503 |
| Medium | auth-public | logged-out | mobile | /vi/register | 24 | 0 | 22 | 0 | 2683 | 596 | 596 | 0.0 | 0.0000 | 0 | 0 | 512966 |
| Medium | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 2808 | 592 | 592 | 0.0 | 0.0000 | 0 | 0 | 511944 |
| Medium | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2660 | 588 | 588 | 0.0 | 0.0000 | 0 | 0 | 512150 |
| Medium | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 3141 | 616 | 616 | 0.0 | 0.0000 | 0 | 0 | 512203 |
| Medium | auth-public | logged-in-admin | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2648 | 588 | 588 | 0.0 | 0.0000 | 0 | 0 | 512184 |
| Medium | auth-public | logged-in-reader | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2739 | 588 | 588 | 0.0 | 0.0000 | 0 | 0 | 512096 |
| Medium | auth-public | logged-in-reader | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2763 | 680 | 680 | 0.0 | 0.0000 | 0 | 0 | 512225 |

## Major Issues Found

- Critical: 29 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: 129 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 36 page(s) có request trong dải 400-800ms.
- Duplicate: 239 nhóm duplicate request cần kiểm tra over-fetch/cache key.
- Pending: chưa phát hiện pending request bất thường.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers | GET | 200 | 3571 | 297 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1986 | 301 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-in-reader | mobile | /vi | GET | 200 | 1981 | 309 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1902 | 201 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1765 | 276 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/e45cd035-4284-42a9-a061-e51f15d14232 | GET | 200 | 1709 | 967 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1688 | 200 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1666 | 310 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1660 | 283 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1654 | 403 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1549 | 150 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1534 | 113 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1531 | 318 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 1515 | 318 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1480 | 274 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/verify-email | GET | 200 | 1463 | 1446 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1435 | 282 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F08_The_Chariot_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1420 | 311 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1391 | 113 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1366 | 200 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1359 | 378 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | GET | 200 | 1358 | 1342 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community/69db54fc297f66f734421a3c | GET | 404 | 1344 | 1332 | html | https://www.tarotnow.xyz/vi/community/69db54fc297f66f734421a3c |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1315 | 321 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | GET | 200 | 1277 | 1258 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1266 | 297 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1263 | 326 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F16_The_Devil_50_20260325_181357.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1261 | 113 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1248 | 379 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1247 | 307 | html | https://www.tarotnow.xyz/vi/collection |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1244 | 123 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1219 | 127 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1209 | 1123 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | GET | 200 | 1197 | 1177 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1192 | 331 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1181 | 189 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | GET | 200 | 1176 | 327 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | admin | logged-in-admin | desktop | /vi/admin | GET | 200 | 1162 | 814 | static | https://www.tarotnow.xyz/_next/static/chunks/0sssfnyzk3ei5.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1156 | 253 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | GET | 200 | 1142 | 320 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1133 | 246 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin | GET | 200 | 1124 | 980 | static | https://www.tarotnow.xyz/_next/static/chunks/0sssfnyzk3ei5.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1123 | 281 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1100 | 403 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | GET | 200 | 1084 | 362 | html | https://www.tarotnow.xyz/vi/profile/reader |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1076 | 341 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1063 | 172 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1051 | 559 | static | https://www.tarotnow.xyz/_next/static/chunks/00xu_deqr9.nw.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | GET | 200 | 1046 | 1034 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | GET | 200 | 1042 | 317 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| Critical | reading | logged-in-reader | mobile | /vi/reading | GET | 200 | 1032 | 317 | html | https://www.tarotnow.xyz/vi/reading |
| Critical | auth-public | logged-out | desktop | /vi/verify-email | GET | 200 | 1030 | 370 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | GET | 200 | 1011 | 990 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1007 | 84 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | desktop | /vi/admin/gamification | GET | 200 | 1006 | 996 | static | https://www.tarotnow.xyz/_next/static/chunks/0pndj~zu5fxx~.js |
| Critical | auth-public | logged-out | mobile | /vi | GET | 200 | 1004 | 588 | html | https://www.tarotnow.xyz/vi |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 993 | 117 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/5ee1ae7a-f9a3-4a46-974b-109190abef8a | GET | 200 | 992 | 975 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 986 | 260 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | GET | 200 | 978 | 373 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 975 | 128 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 969 | 271 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | GET | 200 | 954 | 492 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 953 | 947 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 949 | 307 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | GET | 200 | 949 | 485 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 948 | 337 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 930 | 476 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 926 | 694 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 921 | 543 | static | https://www.tarotnow.xyz/_next/static/chunks/0mddo4pcihb63.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | GET | 200 | 915 | 327 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | GET | 200 | 908 | 396 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | admin | logged-in-admin | desktop | /vi/admin/readings | GET | 200 | 907 | 406 | html | https://www.tarotnow.xyz/vi/admin/readings |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 894 | 314 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F16_The_Devil_50_20260325_181357.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | desktop | /vi/admin/promotions | GET | 200 | 882 | 316 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| Critical | reader-chat | logged-in-admin | mobile | /vi/reader/apply | GET | 200 | 877 | 306 | html | https://www.tarotnow.xyz/vi/reader/apply |
| Critical | admin | logged-in-admin | desktop | /vi/admin/deposits | GET | 200 | 876 | 363 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| Critical | admin | logged-in-admin | mobile | /vi/admin/system-configs | GET | 200 | 872 | 367 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | GET | 200 | 870 | 866 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=96&q=75 |
| Critical | admin | logged-in-admin | desktop | /vi/admin/users | GET | 200 | 861 | 336 | html | https://www.tarotnow.xyz/vi/admin/users |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0q8qdu-5ybfjk.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/00xu_deqr9.nw.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0mddo4pcihb63.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
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
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0q8qdu-5ybfjk.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/00xu_deqr9.nw.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0mddo4pcihb63.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-admin | desktop | /vi/forgot-password | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
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

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| - | - | - | - | - | - |

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

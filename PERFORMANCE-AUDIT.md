# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-07T17:05:01.301Z
- Benchmark generated at (UTC): 2026-05-07T16:56:50.006Z
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: feature-matrix
- Total scenarios: 6
- Total pages measured: 116
- Critical pages: 16
- High pages: 92
- Medium pages: 8
- Slow requests >800ms: 165
- Slow requests 400-800ms: 907

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 0 | 0.0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 29 | 30.5 | 3846 | 5 | 2 | yes |
| logged-in-reader | desktop | 29 | 30.0 | 3460 | 0 | 1 | yes |
| logged-out | mobile | 0 | 0.0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 29 | 29.1 | 3487 | 1 | 1 | yes |
| logged-in-reader | mobile | 29 | 29.9 | 3630 | 0 | 1 | yes |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 43 | 5 | 36 | 0 | 5126 | 1176 | 2276 | 8.0 | 0.0042 | 0 | 2 | 791273 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 40 | 4 | 34 | 0 | 4370 | 624 | 1024 | 0.0 | 0.0071 | 0 | 0 | 811036 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 39 | 3 | 34 | 0 | 3990 | 1160 | 1592 | 0.0 | 0.0042 | 0 | 0 | 656088 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/3a9b8f41-a1ab-459f-a32a-8cabc1f45fb0 | 39 | 5 | 32 | 0 | 5067 | 2348 | 4712 | 0.0 | 0.0042 | 0 | 0 | 738057 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 38 | 2 | 34 | 0 | 3484 | 1132 | 1500 | 0.0 | 0.0042 | 1 | 0 | 736766 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 38 | 3 | 33 | 0 | 3790 | 644 | 1080 | 0.0 | 0.0039 | 0 | 0 | 655247 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 38 | 2 | 34 | 0 | 3696 | 820 | 1268 | 8.0 | 0.0000 | 0 | 0 | 802140 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/f10a769f-c400-4ca4-b124-dd8e16c4e471 | 36 | 3 | 31 | 0 | 3286 | 956 | 956 | 0.0 | 0.0039 | 0 | 0 | 727636 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 36 | 5 | 29 | 0 | 4981 | 780 | 1144 | 0.0 | 0.0000 | 0 | 0 | 659044 |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 35 | 0 | 33 | 0 | 3975 | 912 | 1796 | 0.0 | 0.0039 | 0 | 1 | 776489 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 35 | 0 | 33 | 0 | 5501 | 904 | 1736 | 0.0 | 0.0122 | 0 | 1 | 776476 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 35 | 0 | 33 | 0 | 3939 | 832 | 1744 | 0.0 | 0.0000 | 0 | 1 | 776722 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | 31 | 2 | 27 | 0 | 3402 | 792 | 1548 | 0.0 | 0.0042 | 1 | 0 | 637060 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/history | 29 | 1 | 26 | 0 | 3301 | 816 | 1516 | 0.0 | 0.0042 | 2 | 0 | 633682 |
| Critical | auth-public | logged-in-admin | mobile | /vi | 29 | 0 | 27 | 0 | 3500 | 932 | 932 | 27.0 | 0.0055 | 1 | 0 | 607880 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 3502 | 1504 | 1504 | 0.0 | 0.0042 | 1 | 0 | 632815 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 35 | 0 | 33 | 0 | 3182 | 644 | 1204 | 0.0 | 0.0039 | 0 | 0 | 733738 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 35 | 0 | 33 | 0 | 3685 | 872 | 1196 | 7.0 | 0.0071 | 0 | 0 | 661274 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/4f554976-e378-4b40-972a-a84b9c4dbd6c | 35 | 3 | 30 | 0 | 3620 | 924 | 3560 | 0.0 | 0.0000 | 0 | 0 | 696419 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/3386b7d8-e66b-4eac-a09c-fb2bae2bfc00 | 35 | 3 | 30 | 0 | 3440 | 912 | 3380 | 0.0 | 0.0000 | 0 | 0 | 696544 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 34 | 4 | 28 | 0 | 5097 | 916 | 916 | 0.0 | 0.0039 | 0 | 0 | 645183 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 34 | 3 | 29 | 0 | 3528 | 620 | 1240 | 0.0 | 0.0071 | 0 | 0 | 728867 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/eaf7a444-b9dc-4edc-98f9-6360ea4173b9 | 33 | 2 | 29 | 0 | 3393 | 792 | 1152 | 0.0 | 0.0051 | 0 | 0 | 714814 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/d4ce3736-f57b-47ca-a152-39504a46522b | 33 | 2 | 29 | 0 | 2791 | 824 | 824 | 0.0 | 0.0052 | 0 | 0 | 714913 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/0c61d67c-8908-45c6-9a4f-a50dd06f1fd0 | 33 | 2 | 29 | 0 | 3232 | 656 | 1092 | 0.0 | 0.0052 | 0 | 0 | 715054 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 33 | 3 | 28 | 0 | 4424 | 944 | 2388 | 0.0 | 0.0267 | 0 | 0 | 653028 |
| High | reading | logged-in-admin | desktop | /vi/reading | 32 | 3 | 27 | 0 | 3469 | 776 | 1444 | 0.0 | 0.0042 | 0 | 0 | 645735 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 32 | 2 | 28 | 0 | 3064 | 800 | 800 | 0.0 | 0.0177 | 0 | 0 | 651955 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/03f18107-9dd0-41e8-b1e4-19d608f357c4 | 32 | 2 | 28 | 0 | 4336 | 816 | 816 | 0.0 | 0.0071 | 0 | 0 | 682997 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 3278 | 880 | 1664 | 0.0 | 0.0042 | 0 | 0 | 727061 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/13517306-d93c-4085-974f-6ea013169e01 | 31 | 0 | 29 | 0 | 5757 | 3164 | 3496 | 0.0 | 0.0042 | 0 | 0 | 712811 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 31 | 2 | 27 | 0 | 3430 | 1028 | 1640 | 0.0 | 0.0042 | 0 | 0 | 637280 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 31 | 1 | 28 | 0 | 3647 | 1004 | 1420 | 0.0 | 0.0180 | 0 | 0 | 650532 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 31 | 2 | 27 | 0 | 3564 | 1252 | 1252 | 0.0 | 0.0042 | 0 | 0 | 634659 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 31 | 2 | 27 | 0 | 3013 | 864 | 864 | 16.0 | 0.0039 | 0 | 0 | 634545 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 31 | 2 | 27 | 0 | 3358 | 640 | 972 | 0.0 | 0.0000 | 0 | 0 | 635313 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 31 | 2 | 27 | 0 | 3610 | 1412 | 1412 | 0.0 | 0.0000 | 0 | 0 | 648119 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 31 | 2 | 27 | 0 | 3364 | 784 | 784 | 0.0 | 0.0000 | 0 | 0 | 635324 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 30 | 1 | 27 | 0 | 6911 | 964 | 964 | 0.0 | 0.0043 | 0 | 0 | 643922 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 30 | 0 | 28 | 0 | 5663 | 764 | 1156 | 0.0 | 0.0039 | 0 | 0 | 724067 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 30 | 2 | 26 | 0 | 2891 | 800 | 800 | 0.0 | 0.0039 | 0 | 0 | 635063 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 30 | 0 | 28 | 0 | 3318 | 652 | 1012 | 0.0 | 0.0071 | 0 | 0 | 725811 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/9199e5e8-5550-4842-a985-7fe0001be3dc | 30 | 0 | 28 | 0 | 4128 | 676 | 1012 | 0.0 | 0.0071 | 0 | 0 | 680643 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2993 | 808 | 808 | 0.0 | 0.0196 | 0 | 0 | 649544 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 30 | 0 | 28 | 0 | 3037 | 740 | 1100 | 0.0 | 0.0000 | 0 | 0 | 644646 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 30 | 2 | 26 | 0 | 3122 | 860 | 1216 | 3.0 | 0.0000 | 0 | 0 | 636019 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 29 | 1 | 26 | 0 | 3148 | 948 | 1344 | 0.0 | 0.0042 | 0 | 0 | 632100 |
| High | auth-public | logged-in-admin | desktop | /vi | 29 | 0 | 27 | 0 | 3477 | 1544 | 1544 | 64.0 | 0.0035 | 0 | 0 | 607958 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 6983 | 4424 | 5008 | 0.0 | 0.0192 | 0 | 0 | 643446 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 29 | 0 | 27 | 0 | 3360 | 1012 | 1396 | 0.0 | 0.0543 | 0 | 0 | 636119 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 29 | 1 | 26 | 0 | 3595 | 836 | 1892 | 0.0 | 0.0042 | 0 | 0 | 633855 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 7433 | 624 | 1076 | 19.0 | 0.0040 | 0 | 0 | 641855 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 1 | 26 | 0 | 2845 | 604 | 1024 | 0.0 | 0.0039 | 0 | 0 | 632365 |
| High | auth-public | logged-in-reader | desktop | /vi | 29 | 0 | 27 | 0 | 3244 | 1272 | 1272 | 146.0 | 0.0033 | 0 | 0 | 607812 |
| High | reading | logged-in-reader | desktop | /vi/reading | 29 | 1 | 26 | 0 | 3094 | 664 | 1128 | 0.0 | 0.0039 | 0 | 0 | 642493 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 29 | 0 | 27 | 0 | 3326 | 636 | 1136 | 0.0 | 0.0039 | 0 | 0 | 635244 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 2892 | 752 | 1192 | 0.0 | 0.0190 | 0 | 0 | 643655 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 29 | 0 | 27 | 0 | 4335 | 636 | 1100 | 0.0 | 0.0740 | 0 | 0 | 635910 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 6542 | 892 | 892 | 0.0 | 0.0000 | 0 | 0 | 643030 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 29 | 0 | 27 | 0 | 3100 | 692 | 1040 | 0.0 | 0.0000 | 0 | 0 | 640522 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 2998 | 676 | 1028 | 0.0 | 0.0000 | 0 | 0 | 643364 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 8422 | 740 | 740 | 47.0 | 0.0071 | 0 | 0 | 642067 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 29 | 0 | 27 | 0 | 3663 | 1128 | 1128 | 0.0 | 0.0000 | 0 | 0 | 640720 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 29 | 0 | 27 | 0 | 3241 | 600 | 964 | 0.0 | 0.0892 | 0 | 0 | 643966 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 3380 | 1444 | 1444 | 0.0 | 0.0042 | 0 | 0 | 631098 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 3419 | 876 | 1272 | 0.0 | 0.0042 | 0 | 0 | 631053 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 3671 | 1760 | 1760 | 11.0 | 0.0042 | 0 | 0 | 631810 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3215 | 876 | 1588 | 0.0 | 0.0042 | 0 | 0 | 632147 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 3221 | 1144 | 1144 | 0.0 | 0.0047 | 0 | 0 | 631990 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 3462 | 1632 | 1856 | 0.0 | 0.0042 | 0 | 0 | 632895 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2803 | 648 | 1064 | 0.0 | 0.0039 | 0 | 0 | 630981 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 3202 | 732 | 1076 | 0.0 | 0.0039 | 0 | 0 | 631203 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 2986 | 732 | 1176 | 0.0 | 0.0039 | 0 | 0 | 634094 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 3069 | 808 | 808 | 0.0 | 0.0039 | 0 | 0 | 632805 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2950 | 888 | 888 | 0.0 | 0.0039 | 0 | 0 | 631531 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 3346 | 856 | 856 | 0.0 | 0.0044 | 0 | 0 | 632170 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 3298 | 608 | 1096 | 0.0 | 0.0039 | 0 | 0 | 632907 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2736 | 780 | 780 | 0.0 | 0.0039 | 0 | 0 | 633046 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/e4044012-c2c4-4a44-bd62-19363ca42385 | 28 | 0 | 26 | 0 | 2979 | 692 | 1048 | 0.0 | 0.0000 | 0 | 0 | 632052 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/772facb5-df86-44c3-928d-bb3d5835547e | 28 | 0 | 26 | 0 | 2995 | 676 | 1032 | 0.0 | 0.0000 | 0 | 0 | 632045 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 3038 | 772 | 772 | 0.0 | 0.0000 | 0 | 0 | 630750 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 3041 | 764 | 764 | 0.0 | 0.0000 | 0 | 0 | 631078 |
| High | reading | logged-in-admin | mobile | /vi/reading | 28 | 0 | 26 | 0 | 3813 | 700 | 1044 | 0.0 | 0.0071 | 0 | 0 | 641572 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 3716 | 692 | 1052 | 0.0 | 0.0071 | 0 | 0 | 633863 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2862 | 676 | 1016 | 0.0 | 0.0000 | 0 | 0 | 632393 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2941 | 668 | 1012 | 0.0 | 0.0000 | 0 | 0 | 631177 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 3137 | 776 | 776 | 0.0 | 0.0000 | 0 | 0 | 631444 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2964 | 832 | 832 | 0.0 | 0.0000 | 0 | 0 | 631891 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 2944 | 640 | 984 | 0.0 | 0.0000 | 0 | 0 | 631865 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 4659 | 820 | 820 | 0.0 | 0.0071 | 0 | 0 | 632700 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 3304 | 816 | 816 | 0.0 | 0.0071 | 0 | 0 | 632552 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2971 | 872 | 872 | 0.0 | 0.0000 | 0 | 0 | 632855 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 3692 | 808 | 808 | 0.0 | 0.0071 | 0 | 0 | 630974 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 3188 | 692 | 1060 | 0.0 | 0.0000 | 0 | 0 | 631086 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2998 | 760 | 760 | 0.0 | 0.0000 | 0 | 0 | 631149 |
| High | reading | logged-in-reader | mobile | /vi/reading | 28 | 0 | 26 | 0 | 3226 | 788 | 1156 | 0.0 | 0.0000 | 0 | 0 | 641612 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 3057 | 688 | 688 | 0.0 | 0.0000 | 0 | 0 | 632724 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 3563 | 844 | 844 | 0.0 | 0.0071 | 0 | 0 | 631586 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3202 | 680 | 680 | 0.0 | 0.0071 | 0 | 0 | 631624 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 3085 | 964 | 964 | 0.0 | 0.0000 | 0 | 0 | 632208 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 3044 | 808 | 808 | 0.0 | 0.0000 | 0 | 0 | 632717 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 3344 | 932 | 932 | 7.0 | 0.0000 | 0 | 0 | 633027 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 3186 | 796 | 796 | 0.0 | 0.0000 | 0 | 0 | 633217 |
| High | auth-public | logged-in-reader | mobile | /vi | 26 | 0 | 24 | 0 | 2885 | 616 | 1020 | 0.0 | 0.0032 | 0 | 0 | 537430 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 3054 | 828 | 1216 | 0.0 | 0.0020 | 0 | 0 | 526155 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3228 | 1224 | 1224 | 0.0 | 0.0020 | 0 | 0 | 526041 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2988 | 868 | 1220 | 0.0 | 0.0020 | 0 | 0 | 526144 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 3036 | 876 | 876 | 0.0 | 0.0032 | 0 | 0 | 525980 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2820 | 620 | 948 | 0.0 | 0.0019 | 0 | 0 | 526138 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3021 | 652 | 964 | 0.0 | 0.0019 | 0 | 0 | 526167 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2963 | 720 | 720 | 0.0 | 0.0019 | 0 | 0 | 526352 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3373 | 1048 | 1048 | 0.0 | 0.0032 | 0 | 0 | 526079 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2916 | 828 | 828 | 0.0 | 0.0032 | 0 | 0 | 526134 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 3114 | 556 | 968 | 5.0 | 0.0032 | 0 | 0 | 526173 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2930 | 716 | 716 | 0.0 | 0.0032 | 0 | 0 | 526098 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3266 | 1012 | 1012 | 0.0 | 0.0032 | 0 | 0 | 526190 |

## Major Issues Found

- Critical: 16 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: 92 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 8 page(s) có request trong dải 400-800ms.
- Duplicate: 5 nhóm duplicate request cần kiểm tra over-fetch/cache key.
- Pending: 5 page(s) có pending request không phải websocket/eventsource.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 4712 | 3999 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 3436 | 1389 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | GET | 200 | 3226 | 615 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/13517306-d93c-4085-974f-6ea013169e01 | GET | 200 | 3134 | 2709 | html | https://www.tarotnow.xyz/vi/reading/session/13517306-d93c-4085-974f-6ea013169e01 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2624 | 426 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/chat | GET | 200 | 2522 | 2072 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | GET | 200 | 2490 | 1737 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2411 | 2406 | api | https://www.tarotnow.xyz/api/auth/session |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2373 | 2353 | api | https://www.tarotnow.xyz/api/v1/reading/cards-catalog/details/4?v=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | GET | 200 | 2226 | 311 | html | https://www.tarotnow.xyz/vi/leaderboard |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 400 | 2218 | 2210 | static | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F02_The_Magician_50_20260325_181348.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=384&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2191 | 806 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | GET | 200 | 2158 | 2139 | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 2151 | 939 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | GET | 200 | 2021 | 1995 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1946 | 433 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 400 | 1868 | 1862 | static | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F03_The_High%2BPriestess%2B_50_20260325_181348.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 400 | 1848 | 1819 | static | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F06_The_Hierophant_50_20260325_181348.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 400 | 1817 | 1808 | static | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F14_Death_50_20260325_181356.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | GET | 200 | 1804 | 1708 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | POST | 200 | 1786 | 1770 | other | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/3a9b8f41-a1ab-459f-a32a-8cabc1f45fb0 | GET | 200 | 1705 | 443 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/03f18107-9dd0-41e8-b1e4-19d608f357c4 | GET | 200 | 1703 | 1473 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/3a9b8f41-a1ab-459f-a32a-8cabc1f45fb0 | GET | 200 | 1603 | 1598 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1597 | 1575 | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1572 | 434 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1572 | 614 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/history | GET | 200 | 1563 | 1539 | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/9199e5e8-5550-4842-a985-7fe0001be3dc | GET | 200 | 1536 | 1127 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1528 | 1519 | api | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | reading | logged-in-admin | desktop | /vi/reading/history | GET | 200 | 1489 | 1431 | api | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1465 | 435 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1455 | 484 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1451 | 1430 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | GET | 200 | 1399 | 322 | static | https://www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/03f18107-9dd0-41e8-b1e4-19d608f357c4 | GET | 200 | 1391 | 637 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 1340 | 428 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | GET | 200 | 1332 | 455 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 1301 | 303 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1294 | 339 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1292 | 346 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1275 | 341 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | reading | logged-in-admin | mobile | /vi/reading | GET | 200 | 1274 | 275 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | GET | 200 | 1260 | 475 | html | https://www.tarotnow.xyz/vi/readers |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 1246 | 445 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | GET | 200 | 1245 | 306 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | reading | logged-in-admin | desktop | /vi/reading | GET | 200 | 1228 | 460 | html | https://www.tarotnow.xyz/vi/reading |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 1223 | 1142 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 1207 | 288 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | GET | 200 | 1196 | 433 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1188 | 435 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | GET | 200 | 1169 | 476 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | GET | 200 | 1163 | 518 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | GET | 200 | 1115 | 423 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | GET | 200 | 1113 | 1080 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | GET | 200 | 1103 | 229 | static | https://www.tarotnow.xyz/_next/static/chunks/13onjvrcl5bfv.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/privacy | GET | 200 | 1099 | 486 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1097 | 1089 | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1088 | 1073 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | GET | 200 | 1083 | 305 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | GET | 200 | 1082 | 530 | html | https://www.tarotnow.xyz/vi/community |
| Critical | reader-chat | logged-in-reader | mobile | /vi/chat | GET | 200 | 1073 | 331 | html | https://www.tarotnow.xyz/vi/chat |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1069 | 346 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | GET | 200 | 1055 | 998 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | GET | 200 | 1049 | 368 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 1045 | 322 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1044 | 340 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | GET | 200 | 1017 | 482 | html | https://www.tarotnow.xyz/vi/leaderboard |
| Critical | reader-chat | logged-in-admin | desktop | /vi/chat | GET | 200 | 981 | 938 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | GET | 200 | 981 | 899 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/chat | GET | 200 | 970 | 430 | html | https://www.tarotnow.xyz/vi/chat |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/3a9b8f41-a1ab-459f-a32a-8cabc1f45fb0 | GET | 200 | 968 | 533 | html | https://www.tarotnow.xyz/vi/reading/session/3a9b8f41-a1ab-459f-a32a-8cabc1f45fb0 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 965 | 357 | html | https://www.tarotnow.xyz/vi/gacha/history |
| Critical | auth-public | logged-in-admin | desktop | /vi/legal/tos | GET | 200 | 959 | 538 | html | https://www.tarotnow.xyz/vi/legal/tos |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/9199e5e8-5550-4842-a985-7fe0001be3dc | GET | 200 | 956 | 923 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | GET | 200 | 955 | 655 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 954 | 433 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | GET | 200 | 951 | 398 | html | https://www.tarotnow.xyz/vi/inventory |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/4f554976-e378-4b40-972a-a84b9c4dbd6c | GET | 200 | 950 | 319 | html | https://www.tarotnow.xyz/vi/reading/session/4f554976-e378-4b40-972a-a84b9c4dbd6c |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | GET | 200 | 946 | 454 | html | https://www.tarotnow.xyz/vi/wallet/deposit |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| High | reading | logged-in-admin | desktop | /vi/reading/session/3a9b8f41-a1ab-459f-a32a-8cabc1f45fb0 | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 2 | GET https://www.tarotnow.xyz/_next/image?q=75&url=https://media.tarotnow.xyz/avatars/1bf7374304584c0488e06621bbc1454f.webp&w=48 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=64&q=75 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | auth-public | logged-in-admin | mobile | /vi | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=1200&q=75 |

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

## Optimization Cycle Notes

- Optimized feature: `inventory-gacha-collection`.
- Root cause: collection card image URLs used `/api/collection/card-image?...` through Next Image optimization, producing repeated production `/_next/image` 400 responses on `/vi/collection`.
- Fix commit: `6fb3e6d3` bypasses Next Image optimization for collection image proxy URLs.
- Verification: `npx vitest run src/shared/http/assetUrl.test.ts` passed locally.
- Verification: `npm run lint` passed locally.
- Targeted production benchmark: completed for `inventory-gacha-collection` before deploy; final post-deploy full benchmark is still required to confirm production 400s are removed.
- Browser check: production `/vi/collection` currently reproduces the pre-deploy `_next/image` 400 errors, matching benchmark evidence.

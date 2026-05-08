# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-08T16:58:36.326Z
- Benchmark generated at (UTC): 2026-05-08T16:58:30.517Z
- Benchmark input: Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 174
- Critical pages: 19
- High pages: 142
- Medium pages: 13
- Slow requests >800ms: 160
- Slow requests 400-800ms: 404
- Request thresholds: >25 suspicious, >35 severe
- Slow request thresholds: >400ms optimize, >800ms serious

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 25.0 | 3346 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.7 | 3287 | 2 | 0 | yes |
| logged-in-reader | desktop | 33 | 29.3 | 3324 | 3 | 0 | yes |
| logged-out | mobile | 9 | 25.1 | 3262 | 0 | 1 | yes |
| logged-in-admin | mobile | 45 | 30.8 | 3156 | 5 | 0 | yes |
| logged-in-reader | mobile | 35 | 31.0 | 3278 | 1 | 0 | yes |

## Route Family Coverage

| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.8 | 3414 | 0 | 0 |
| logged-in-admin | desktop | auth-public | 3 | 25.0 | 2939 | 0 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | 3 | 32.7 | 3687 | 0 | 0 |
| logged-in-admin | desktop | home | 1 | 35.0 | 3459 | 0 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | 4 | 33.3 | 4279 | 0 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | 8 | 28.8 | 3081 | 2 | 0 |
| logged-in-admin | desktop | reader-chat | 9 | 28.6 | 2896 | 0 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.8 | 3205 | 0 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.6 | 3066 | 0 | 0 |
| logged-in-admin | mobile | auth-public | 5 | 35.6 | 3212 | 0 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | 3 | 39.3 | 3677 | 0 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2733 | 0 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | 4 | 34.0 | 3775 | 0 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | 8 | 28.4 | 3023 | 5 | 0 |
| logged-in-admin | mobile | reader-chat | 9 | 28.2 | 2912 | 0 | 0 |
| logged-in-admin | mobile | reading | 5 | 30.6 | 3213 | 0 | 0 |
| logged-in-reader | desktop | auth-public | 3 | 25.0 | 3875 | 0 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | 3 | 29.7 | 3109 | 0 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2698 | 0 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | 4 | 32.8 | 4193 | 0 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | 8 | 29.4 | 3348 | 3 | 0 |
| logged-in-reader | desktop | reader-chat | 9 | 28.9 | 3019 | 0 | 0 |
| logged-in-reader | desktop | reading | 5 | 30.4 | 3061 | 0 | 0 |
| logged-in-reader | mobile | auth-public | 5 | 30.6 | 3115 | 0 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | 3 | 34.0 | 3708 | 1 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2720 | 0 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | 4 | 34.5 | 3903 | 0 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | 8 | 32.6 | 3086 | 0 | 0 |
| logged-in-reader | mobile | reader-chat | 9 | 28.8 | 3378 | 0 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.0 | 2923 | 0 | 0 |
| logged-out | desktop | auth-public | 8 | 24.5 | 3268 | 0 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3971 | 0 | 0 |
| logged-out | mobile | auth-public | 8 | 24.4 | 3247 | 0 | 0 |
| logged-out | mobile | home | 1 | 31.0 | 3386 | 0 | 1 |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 58 | 3 | 52 | 0 | 4867 | 912 | 1276 | 0.0 | 0.0000 | 0 | 0 | 1206615 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 56 | 3 | 49 | 0 | 3739 | 872 | 1256 | 0.0 | 0.0000 | 0 | 0 | 1135425 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | 53 | 0 | 49 | 0 | 3715 | 540 | 904 | 0.0 | 0.0055 | 0 | 0 | 1108143 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | 50 | 0 | 46 | 0 | 3425 | 532 | 888 | 0.0 | 0.0055 | 0 | 0 | 1037676 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | 50 | 0 | 46 | 0 | 3438 | 556 | 928 | 0.0 | 0.0055 | 0 | 0 | 1037989 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 41 | 5 | 34 | 0 | 3695 | 564 | 912 | 0.0 | 0.0071 | 0 | 0 | 812151 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 38 | 3 | 33 | 0 | 3046 | 540 | 884 | 0.0 | 0.0000 | 0 | 0 | 664874 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 37 | 2 | 33 | 0 | 3179 | 632 | 1028 | 0.0 | 0.0042 | 0 | 0 | 654647 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 37 | 2 | 33 | 0 | 3374 | 596 | 996 | 0.0 | 0.0039 | 0 | 0 | 735356 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 37 | 2 | 33 | 0 | 3195 | 592 | 920 | 0.0 | 0.0071 | 0 | 0 | 799499 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 37 | 3 | 32 | 0 | 3842 | 572 | 1680 | 0.0 | 0.0051 | 0 | 0 | 779718 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 36 | 2 | 32 | 0 | 4085 | 628 | 2032 | 0.0 | 0.0042 | 0 | 0 | 779135 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/33d0cbf1-f592-4e7f-a9ab-3513d4ab39f2 | 36 | 4 | 30 | 0 | 4367 | 548 | 4064 | 0.0 | 0.0071 | 0 | 0 | 704595 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 33 | 4 | 27 | 0 | 4032 | 568 | 964 | 0.0 | 0.0039 | 1 | 0 | 644897 |
| Critical | auth-public | logged-out | mobile | /vi | 31 | 2 | 27 | 0 | 3386 | 1024 | 1024 | 0.0 | 0.0000 | 0 | 1 | 603572 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 30 | 1 | 27 | 0 | 2921 | 560 | 884 | 0.0 | 0.0000 | 1 | 0 | 643200 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2773 | 676 | 1060 | 0.0 | 0.0042 | 2 | 0 | 631276 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 27 | 0 | 25 | 0 | 3459 | 924 | 1628 | 330.0 | 0.0038 | 2 | 0 | 607151 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 27 | 0 | 25 | 0 | 2787 | 564 | 924 | 0.0 | 0.0000 | 5 | 0 | 607180 |
| High | auth-public | logged-in-admin | desktop | /vi | 35 | 5 | 27 | 0 | 3459 | 876 | 1528 | 341.0 | 0.0040 | 0 | 0 | 613395 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 35 | 0 | 33 | 0 | 3493 | 628 | 1076 | 0.0 | 0.0042 | 0 | 0 | 732550 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 35 | 0 | 33 | 0 | 3244 | 628 | 1060 | 0.0 | 0.0039 | 0 | 0 | 652349 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 35 | 0 | 33 | 0 | 3207 | 600 | 940 | 0.0 | 0.0071 | 0 | 0 | 661536 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 35 | 4 | 29 | 0 | 4361 | 584 | 912 | 0.0 | 0.0196 | 0 | 0 | 662930 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 34 | 5 | 27 | 0 | 6705 | 644 | 980 | 0.0 | 0.0000 | 0 | 0 | 646704 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 33 | 4 | 27 | 0 | 3371 | 604 | 1120 | 0.0 | 0.0042 | 0 | 0 | 645285 |
| High | admin | logged-in-admin | desktop | /vi/admin/gamification | 33 | 1 | 30 | 0 | 3248 | 684 | 1000 | 0.0 | 0.0022 | 0 | 0 | 699581 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 33 | 3 | 27 | 0 | 3050 | 628 | 1044 | 0.0 | 0.0726 | 0 | 0 | 638862 |
| High | admin | logged-in-admin | mobile | /vi/admin/gamification | 33 | 1 | 30 | 0 | 3873 | 548 | 860 | 0.0 | 0.0000 | 0 | 0 | 699527 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 33 | 3 | 28 | 0 | 3057 | 552 | 884 | 0.0 | 0.0000 | 0 | 0 | 727349 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 32 | 2 | 28 | 0 | 3750 | 616 | 1080 | 0.0 | 0.0042 | 0 | 0 | 728814 |
| High | reading | logged-in-reader | desktop | /vi/reading | 32 | 3 | 27 | 0 | 3408 | 600 | 988 | 0.0 | 0.0039 | 0 | 0 | 653933 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 32 | 2 | 28 | 0 | 3224 | 572 | 912 | 0.0 | 0.0071 | 0 | 0 | 728753 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 32 | 3 | 27 | 0 | 3074 | 560 | 904 | 0.0 | 0.0000 | 0 | 0 | 637567 |
| High | reading | logged-in-admin | desktop | /vi/reading | 31 | 3 | 26 | 0 | 3311 | 652 | 1032 | 0.0 | 0.0042 | 0 | 0 | 645038 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 31 | 1 | 28 | 0 | 3213 | 632 | 976 | 0.0 | 0.0180 | 0 | 0 | 650901 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 31 | 2 | 27 | 0 | 3764 | 632 | 988 | 0.0 | 0.0279 | 0 | 0 | 644644 |
| High | admin | logged-in-admin | desktop | /vi/admin/disputes | 31 | 1 | 28 | 0 | 4903 | 660 | 1000 | 0.0 | 0.0000 | 0 | 0 | 649209 |
| High | admin | logged-in-admin | desktop | /vi/admin/reader-requests | 31 | 1 | 28 | 0 | 4480 | 728 | 1076 | 0.0 | 0.0000 | 0 | 0 | 649376 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | 31 | 0 | 29 | 0 | 4089 | 644 | 1356 | 0.0 | 0.0042 | 0 | 0 | 713107 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/2a0a3080-de77-4852-9a58-b3d1e92fff2f | 31 | 0 | 29 | 0 | 3064 | 636 | 1016 | 0.0 | 0.0042 | 0 | 0 | 713349 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | 3 | 26 | 0 | 3294 | 652 | 1224 | 0.0 | 0.0042 | 0 | 0 | 635943 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/eacbcb32-5bb0-4271-bd9b-0c5345f67219 | 31 | 0 | 29 | 0 | 3073 | 976 | 976 | 0.0 | 0.0039 | 0 | 0 | 713012 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/ba4ed496-3f1c-4bff-8ff1-4527fcb1cf44 | 31 | 0 | 29 | 0 | 3087 | 608 | 1012 | 0.0 | 0.0039 | 0 | 0 | 713175 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | 3 | 26 | 0 | 3232 | 616 | 1208 | 0.0 | 0.0039 | 0 | 0 | 636313 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | 3 | 26 | 0 | 3157 | 624 | 1216 | 0.0 | 0.0039 | 0 | 0 | 636531 |
| High | reading | logged-in-admin | mobile | /vi/reading | 31 | 3 | 26 | 0 | 2911 | 548 | 888 | 0.0 | 0.0000 | 0 | 0 | 645076 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 31 | 2 | 27 | 0 | 3097 | 560 | 900 | 0.0 | 0.1024 | 0 | 0 | 636931 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 31 | 3 | 26 | 0 | 3499 | 920 | 1256 | 0.0 | 0.0000 | 0 | 0 | 635008 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 31 | 3 | 26 | 0 | 2951 | 556 | 892 | 0.0 | 0.0000 | 0 | 0 | 635170 |
| High | admin | logged-in-admin | desktop | /vi/admin/users | 30 | 0 | 28 | 0 | 3242 | 628 | 1016 | 0.0 | 0.0000 | 0 | 0 | 650298 |
| High | admin | logged-in-admin | desktop | /vi/admin/withdrawals | 30 | 1 | 27 | 0 | 2928 | 732 | 1000 | 0.0 | 0.0000 | 0 | 0 | 647170 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 30 | 0 | 28 | 0 | 3308 | 592 | 992 | 0.0 | 0.0039 | 0 | 0 | 724365 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 30 | 0 | 28 | 0 | 2792 | 620 | 972 | 0.0 | 0.0177 | 0 | 0 | 649813 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 30 | 0 | 28 | 0 | 3430 | 628 | 1772 | 0.0 | 0.0039 | 0 | 0 | 643376 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 30 | 2 | 26 | 0 | 2970 | 600 | 1144 | 0.0 | 0.0039 | 0 | 0 | 635455 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2707 | 532 | 864 | 0.0 | 0.0196 | 0 | 0 | 649543 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 30 | 0 | 28 | 0 | 3457 | 544 | 1900 | 0.0 | 0.0051 | 0 | 0 | 643214 |
| High | admin | logged-in-admin | mobile | /vi/admin/readings | 30 | 0 | 28 | 0 | 3528 | 580 | 908 | 0.0 | 0.0000 | 0 | 0 | 650576 |
| High | admin | logged-in-admin | mobile | /vi/admin/system-configs | 30 | 0 | 28 | 0 | 3226 | 560 | 896 | 0.0 | 0.0000 | 0 | 0 | 689330 |
| High | admin | logged-in-admin | mobile | /vi/admin/users | 30 | 0 | 28 | 0 | 2769 | 544 | 868 | 0.0 | 0.0000 | 0 | 0 | 649743 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/6d3e854c-16d8-49dc-8b91-086f1421b9bc | 30 | 0 | 28 | 0 | 3007 | 568 | 896 | 0.0 | 0.0000 | 0 | 0 | 680918 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 30 | 2 | 26 | 0 | 2998 | 572 | 924 | 0.0 | 0.0000 | 0 | 0 | 634977 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/ac436e4f-fad9-4cdc-bb04-edfbbb70f7dd | 30 | 0 | 28 | 0 | 3042 | 576 | 912 | 0.0 | 0.0071 | 0 | 0 | 681178 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/306aa5d9-2a5c-4068-9fb0-74d97a4a410a | 30 | 0 | 28 | 0 | 3037 | 556 | 892 | 0.0 | 0.0071 | 0 | 0 | 681164 |
| High | auth-public | logged-out | desktop | /vi | 29 | 0 | 27 | 0 | 3971 | 1460 | 1460 | 170.0 | 0.0000 | 0 | 0 | 601524 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6693 | 592 | 592 | 0.0 | 0.0043 | 0 | 0 | 643480 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 29 | 0 | 27 | 0 | 4184 | 644 | 1076 | 0.0 | 0.0489 | 0 | 0 | 634241 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers | 29 | 1 | 26 | 0 | 2830 | 600 | 984 | 0.0 | 0.0042 | 0 | 0 | 634796 |
| High | admin | logged-in-admin | desktop | /vi/admin | 29 | 0 | 27 | 0 | 3194 | 632 | 984 | 0.0 | 0.0000 | 0 | 0 | 647686 |
| High | admin | logged-in-admin | desktop | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2909 | 600 | 1036 | 0.0 | 0.0000 | 0 | 0 | 647744 |
| High | admin | logged-in-admin | desktop | /vi/admin/readings | 29 | 0 | 27 | 0 | 3704 | 652 | 1008 | 0.0 | 0.0000 | 0 | 0 | 649118 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 1 | 26 | 0 | 2996 | 632 | 1260 | 0.0 | 0.0042 | 0 | 0 | 633949 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 6847 | 612 | 960 | 0.0 | 0.0040 | 0 | 0 | 642289 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 29 | 1 | 26 | 0 | 2850 | 576 | 976 | 0.0 | 0.0039 | 0 | 0 | 635374 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 3105 | 620 | 1048 | 0.0 | 0.0903 | 0 | 0 | 642318 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 29 | 1 | 26 | 0 | 2914 | 604 | 992 | 0.0 | 0.0095 | 0 | 0 | 634341 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 29 | 1 | 26 | 0 | 2945 | 616 | 1036 | 0.0 | 0.0040 | 0 | 0 | 633536 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 1 | 26 | 0 | 2826 | 608 | 1180 | 0.0 | 0.0039 | 0 | 0 | 634278 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5635 | 572 | 572 | 0.0 | 0.0000 | 0 | 0 | 643251 |
| High | admin | logged-in-admin | mobile | /vi/admin | 29 | 0 | 27 | 0 | 3254 | 540 | 1156 | 0.0 | 0.0000 | 0 | 0 | 647483 |
| High | admin | logged-in-admin | mobile | /vi/admin/deposits | 29 | 0 | 27 | 0 | 2776 | 556 | 880 | 0.0 | 0.0000 | 0 | 0 | 647916 |
| High | admin | logged-in-admin | mobile | /vi/admin/disputes | 29 | 0 | 27 | 0 | 2818 | 572 | 880 | 0.0 | 0.0000 | 0 | 0 | 646065 |
| High | admin | logged-in-admin | mobile | /vi/admin/reader-requests | 29 | 0 | 27 | 0 | 2806 | 552 | 876 | 0.0 | 0.0000 | 0 | 0 | 646772 |
| High | admin | logged-in-admin | mobile | /vi/admin/withdrawals | 29 | 0 | 27 | 0 | 2772 | 544 | 856 | 0.0 | 0.0000 | 0 | 0 | 646179 |
| High | reading | logged-in-reader | mobile | /vi/reading | 29 | 1 | 26 | 0 | 2924 | 552 | 916 | 0.0 | 0.0000 | 0 | 0 | 642823 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 5654 | 564 | 564 | 0.0 | 0.0000 | 0 | 0 | 642211 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 29 | 1 | 26 | 0 | 2832 | 552 | 876 | 0.0 | 0.0000 | 0 | 0 | 635167 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2683 | 588 | 952 | 0.0 | 0.0042 | 0 | 0 | 631711 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 3367 | 812 | 1076 | 0.0 | 0.0489 | 0 | 0 | 630936 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2789 | 624 | 1028 | 0.0 | 0.0042 | 0 | 0 | 631910 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 2831 | 620 | 1080 | 0.0 | 0.0042 | 0 | 0 | 634025 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2749 | 640 | 1040 | 0.0 | 0.0042 | 0 | 0 | 631713 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2693 | 632 | 1044 | 0.0 | 0.0047 | 0 | 0 | 632180 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2716 | 592 | 996 | 0.0 | 0.0042 | 0 | 0 | 632635 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 2744 | 608 | 1224 | 0.0 | 0.0042 | 0 | 0 | 632889 |
| High | admin | logged-in-admin | desktop | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2726 | 612 | 1000 | 0.0 | 0.0000 | 0 | 0 | 644763 |
| High | admin | logged-in-admin | desktop | /vi/admin/system-configs | 28 | 0 | 26 | 0 | 2810 | 648 | 1076 | 0.0 | 0.0000 | 0 | 0 | 656050 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/d3b73fcf-fe77-4fce-88b0-452d93bbfa7b | 28 | 0 | 26 | 0 | 2816 | 672 | 1132 | 2.0 | 0.0042 | 0 | 0 | 632496 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2746 | 620 | 1024 | 0.0 | 0.0042 | 0 | 0 | 630957 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2815 | 612 | 1008 | 0.0 | 0.0042 | 0 | 0 | 631399 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2819 | 644 | 1056 | 0.0 | 0.0042 | 0 | 0 | 631377 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 3060 | 724 | 1296 | 0.0 | 0.0042 | 0 | 0 | 633175 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2977 | 576 | 984 | 0.0 | 0.0039 | 0 | 0 | 632578 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2762 | 620 | 1004 | 0.0 | 0.0039 | 0 | 0 | 631681 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2666 | 604 | 1016 | 0.0 | 0.0039 | 0 | 0 | 631844 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 4741 | 580 | 940 | 0.0 | 0.0039 | 0 | 0 | 631914 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2935 | 1080 | 1080 | 0.0 | 0.0039 | 0 | 0 | 632792 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/f7987eb3-81ef-4f1b-8d95-19a85bdd6aae | 28 | 0 | 26 | 0 | 2769 | 728 | 1160 | 0.0 | 0.0039 | 0 | 0 | 632425 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 3574 | 584 | 996 | 0.0 | 0.0039 | 0 | 0 | 631427 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2801 | 620 | 984 | 0.0 | 0.0039 | 0 | 0 | 631450 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 3031 | 600 | 1080 | 0.0 | 0.0039 | 0 | 0 | 631285 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2750 | 552 | 884 | 0.0 | 0.0000 | 0 | 0 | 631820 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2840 | 560 | 900 | 0.0 | 0.0760 | 0 | 0 | 630976 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 28 | 0 | 26 | 0 | 2872 | 540 | 868 | 0.0 | 0.0000 | 0 | 0 | 634135 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2733 | 572 | 896 | 0.0 | 0.0000 | 0 | 0 | 631660 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 3421 | 1276 | 1612 | 0.0 | 0.0000 | 0 | 0 | 631676 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3136 | 568 | 908 | 0.0 | 0.0071 | 0 | 0 | 632531 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2805 | 656 | 1016 | 0.0 | 0.0071 | 0 | 0 | 631410 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 3368 | 528 | 864 | 0.0 | 0.0088 | 0 | 0 | 632126 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2803 | 552 | 876 | 0.0 | 0.0000 | 0 | 0 | 632510 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2990 | 560 | 936 | 0.0 | 0.0000 | 0 | 0 | 633016 |
| High | admin | logged-in-admin | mobile | /vi/admin/promotions | 28 | 0 | 26 | 0 | 2838 | 568 | 876 | 0.0 | 0.0000 | 0 | 0 | 644886 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/87c68e71-1ef8-4aa9-8a53-7ca6a3e1a353 | 28 | 0 | 26 | 0 | 2790 | 556 | 880 | 0.0 | 0.0000 | 0 | 0 | 632282 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2909 | 560 | 884 | 0.0 | 0.0000 | 0 | 0 | 631344 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2916 | 712 | 1028 | 0.0 | 0.0000 | 0 | 0 | 631211 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 3042 | 584 | 908 | 0.0 | 0.0000 | 0 | 0 | 631238 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2740 | 536 | 932 | 0.0 | 0.0000 | 0 | 0 | 632962 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 3195 | 540 | 888 | 0.0 | 0.0071 | 0 | 0 | 633179 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2968 | 988 | 988 | 0.0 | 0.0000 | 0 | 0 | 632018 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2805 | 552 | 1284 | 0.0 | 0.0000 | 0 | 0 | 632522 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 2767 | 676 | 1052 | 0.0 | 0.0000 | 0 | 0 | 631824 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2841 | 564 | 920 | 0.0 | 0.0330 | 0 | 0 | 633412 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 2791 | 552 | 884 | 0.0 | 0.0000 | 0 | 0 | 632376 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 3007 | 668 | 1000 | 0.0 | 0.0000 | 0 | 0 | 632637 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2756 | 572 | 924 | 0.0 | 0.0000 | 0 | 0 | 633127 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/6f456328-dcf6-424c-825e-9385d76fc19d | 28 | 0 | 26 | 0 | 2858 | 636 | 964 | 0.0 | 0.0000 | 0 | 0 | 632397 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2775 | 568 | 900 | 0.0 | 0.0000 | 0 | 0 | 631180 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 3317 | 564 | 908 | 0.0 | 0.0071 | 0 | 0 | 631494 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 3046 | 576 | 896 | 0.0 | 0.0071 | 0 | 0 | 631472 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 3177 | 560 | 900 | 0.0 | 0.0071 | 0 | 0 | 633047 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2777 | 564 | 904 | 0.0 | 0.0000 | 0 | 0 | 633478 |
| High | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 27 | 1 | 23 | 0 | 2940 | 536 | 844 | 0.0 | 0.0032 | 0 | 0 | 528059 |
| High | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 27 | 1 | 23 | 0 | 3269 | 652 | 956 | 0.0 | 0.0032 | 0 | 0 | 528201 |
| High | auth-public | logged-in-reader | desktop | /vi | 26 | 0 | 24 | 0 | 2698 | 656 | 1260 | 252.0 | 0.0033 | 0 | 0 | 537822 |
| High | auth-public | logged-in-admin | mobile | /vi | 26 | 0 | 24 | 0 | 2733 | 548 | 896 | 0.0 | 0.0032 | 0 | 0 | 537754 |
| High | auth-public | logged-in-reader | mobile | /vi | 26 | 0 | 24 | 0 | 2720 | 544 | 904 | 0.0 | 0.0032 | 0 | 0 | 537954 |
| High | auth-public | logged-out | desktop | /vi/register | 25 | 1 | 22 | 0 | 4446 | 600 | 600 | 0.0 | 0.0000 | 0 | 0 | 513690 |
| High | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 3399 | 628 | 628 | 0.0 | 0.0000 | 0 | 0 | 525834 |
| High | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 4612 | 604 | 936 | 0.0 | 0.0019 | 0 | 0 | 526305 |
| High | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 4224 | 636 | 940 | 0.0 | 0.0019 | 0 | 0 | 526436 |
| High | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 3163 | 564 | 564 | 0.0 | 0.0000 | 0 | 0 | 525982 |
| High | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 4080 | 696 | 696 | 0.0 | 0.0000 | 0 | 0 | 525988 |
| High | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3352 | 556 | 556 | 0.0 | 0.0000 | 0 | 0 | 526075 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3518 | 572 | 872 | 0.0 | 0.0055 | 0 | 0 | 526268 |
| High | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 3226 | 596 | 596 | 0.0 | 0.0000 | 0 | 0 | 512032 |
| High | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 3542 | 564 | 564 | 0.0 | 0.0000 | 0 | 0 | 512070 |
| High | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 3466 | 568 | 568 | 0.0 | 0.0000 | 0 | 0 | 512397 |
| High | auth-public | logged-out | mobile | /vi/register | 24 | 0 | 22 | 0 | 3278 | 588 | 588 | 0.0 | 0.0000 | 0 | 0 | 512880 |
| High | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 3331 | 644 | 644 | 0.0 | 0.0000 | 0 | 0 | 512072 |
| High | auth-public | logged-in-reader | mobile | /vi/register | 24 | 0 | 22 | 0 | 3225 | 548 | 912 | 0.0 | 0.0055 | 0 | 0 | 510964 |
| Medium | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2745 | 592 | 592 | 0.0 | 0.0000 | 0 | 0 | 526129 |
| Medium | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2996 | 572 | 572 | 0.0 | 0.0000 | 0 | 0 | 526041 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2701 | 600 | 908 | 0.0 | 0.0020 | 0 | 0 | 526255 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3000 | 620 | 984 | 0.0 | 0.0020 | 0 | 0 | 526461 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3117 | 580 | 908 | 0.0 | 0.0020 | 0 | 0 | 526466 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2789 | 616 | 952 | 0.0 | 0.0019 | 0 | 0 | 526243 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2685 | 548 | 856 | 0.0 | 0.0032 | 0 | 0 | 526344 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2718 | 568 | 888 | 0.0 | 0.0032 | 0 | 0 | 526435 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2702 | 536 | 844 | 0.0 | 0.0032 | 0 | 0 | 526348 |
| Medium | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 2729 | 584 | 584 | 0.0 | 0.0000 | 0 | 0 | 512435 |
| Medium | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 3059 | 580 | 580 | 0.0 | 0.0000 | 0 | 0 | 512230 |
| Medium | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 2656 | 540 | 540 | 0.0 | 0.0000 | 0 | 0 | 512039 |
| Medium | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2646 | 540 | 540 | 0.0 | 0.0000 | 0 | 0 | 512199 |

## Major Issues Found

- Critical: 19 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: 142 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 13 page(s) có request trong dải 400-800ms.
- Duplicate: 115 nhóm duplicate request cần kiểm tra over-fetch/cache key.
- Pending: 5 page(s) có pending request không phải websocket/eventsource.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 4281 | 3858 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | admin | logged-in-admin | desktop | /vi/admin/disputes | GET | 200 | 2519 | 512 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 2367 | 2105 | static | https://www.tarotnow.xyz/_next/static/chunks/0pd6iwgval8kp.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/legal/tos | GET | 200 | 2223 | 2077 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-out | desktop | /vi/register | GET | 200 | 2092 | 1467 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/33d0cbf1-f592-4e7f-a9ab-3513d4ab39f2 | GET | 200 | 2026 | 673 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | admin | logged-in-admin | desktop | /vi/admin/reader-requests | GET | 200 | 2023 | 569 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | GET | 200 | 2004 | 1041 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1853 | 281 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | GET | 200 | 1832 | 1816 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | GET | 200 | 1806 | 1256 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-out | mobile | /vi/legal/privacy | GET | 200 | 1735 | 807 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | GET | 200 | 1700 | 1005 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 1690 | 1633 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1670 | 106 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1669 | 263 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1640 | 263 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/gamification | GET | 200 | 1532 | 209 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1495 | 307 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | GET | 200 | 1432 | 1400 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1426 | 268 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/33d0cbf1-f592-4e7f-a9ab-3513d4ab39f2 | GET | 200 | 1391 | 1375 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | GET | 200 | 1382 | 1362 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1376 | 240 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1363 | 211 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1353 | 325 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | GET | 200 | 1345 | 331 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | GET | 200 | 1343 | 702 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1338 | 140 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1321 | 307 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | desktop | /vi/admin/readings | GET | 200 | 1302 | 1287 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1263 | 599 | html | https://www.tarotnow.xyz/vi |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1233 | 88 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1230 | 282 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F08_The_Chariot_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1218 | 140 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1216 | 190 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1212 | 218 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1207 | 1173 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1205 | 295 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | GET | 200 | 1196 | 1178 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 1170 | 334 | html | https://www.tarotnow.xyz/vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d |
| Critical | auth-public | logged-out | desktop | /vi/reset-password | GET | 200 | 1168 | 1157 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | GET | 200 | 1167 | 413 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1166 | 325 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1159 | 232 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/privacy | GET | 200 | 1154 | 1130 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1153 | 152 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1146 | 129 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin | GET | 200 | 1131 | 318 | html | https://www.tarotnow.xyz/vi/admin |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1125 | 163 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | GET | 200 | 1120 | 1103 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1100 | 219 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | GET | 200 | 1087 | 1069 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | reading | logged-in-reader | desktop | /vi/reading | GET | 200 | 1085 | 272 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 1072 | 1069 | static | https://www.tarotnow.xyz/_next/static/chunks/0vp4mbwt0gpbv.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 1072 | 177 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | mobile | /vi/login | GET | 200 | 1065 | 1040 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | GET | 200 | 1057 | 217 | static | https://www.tarotnow.xyz/_next/static/chunks/13onjvrcl5bfv.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | GET | 200 | 1056 | 541 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1056 | 211 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | mobile | /vi | GET | 200 | 1053 | 583 | html | https://www.tarotnow.xyz/vi |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 1052 | 562 | static | https://www.tarotnow.xyz/_next/static/chunks/04qliyftvi87..js |
| Critical | auth-public | logged-in-admin | desktop | /vi | GET | 200 | 1050 | 344 | html | https://www.tarotnow.xyz/vi |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1050 | 224 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1048 | 199 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1037 | 88 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 1035 | 215 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | GET | 200 | 1035 | 675 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1018 | 105 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1013 | 325 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F16_The_Devil_50_20260325_181357.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1011 | 281 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1010 | 140 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 1010 | 729 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 1008 | 177 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | GET | 200 | 1006 | 993 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | GET | 200 | 1005 | 517 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 996 | 750 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| Critical | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | GET | 200 | 995 | 982 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 994 | 177 | api | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-out | desktop | /vi/legal/tos | GET | 200 | 993 | 977 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/04qliyftvi87..js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0pd6iwgval8kp.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/04qliyftvi87..js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0pd6iwgval8kp.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-admin | mobile | /vi/login | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| High | auth-public | logged-in-admin | mobile | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | https://www.tarotnow.xyz/vi/gamification |

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

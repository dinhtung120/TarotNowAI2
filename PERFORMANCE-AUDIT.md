# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-07T17:33:50.794Z
- Benchmark generated at (UTC): 2026-05-07T17:33:44.713Z
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 190
- Critical pages: 24
- High pages: 135
- Medium pages: 31
- Slow requests >800ms: 291
- Slow requests 400-800ms: 1346

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2938 | 0 | 0 | yes |
| logged-in-admin | desktop | 48 | 29.6 | 3083 | 6 | 2 | yes |
| logged-in-reader | desktop | 38 | 29.1 | 3123 | 6 | 1 | yes |
| logged-out | mobile | 9 | 25.1 | 2946 | 0 | 1 | yes |
| logged-in-admin | mobile | 48 | 29.4 | 3656 | 15 | 1 | yes |
| logged-in-reader | mobile | 38 | 30.4 | 3493 | 1 | 2 | yes |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | 60 | 5 | 50 | 0 | 5032 | 1296 | 1296 | 23.0 | 0.0055 | 0 | 1 | 1115227 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | 53 | 0 | 49 | 0 | 4068 | 1388 | 1388 | 670.0 | 0.0033 | 0 | 1 | 1108129 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | 53 | 0 | 49 | 0 | 4222 | 920 | 920 | 6.0 | 0.0055 | 0 | 0 | 1118945 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | 50 | 0 | 46 | 0 | 3431 | 628 | 1300 | 338.0 | 0.0035 | 0 | 1 | 1037933 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 38 | 3 | 33 | 0 | 4009 | 660 | 1992 | 0.0 | 0.0042 | 0 | 1 | 779646 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 38 | 2 | 34 | 0 | 3715 | 1180 | 1180 | 22.0 | 0.0000 | 0 | 0 | 802128 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 38 | 2 | 34 | 0 | 3494 | 604 | 1004 | 4.0 | 0.0071 | 0 | 0 | 665440 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 36 | 1 | 33 | 0 | 3098 | 684 | 1104 | 0.0 | 0.0042 | 0 | 0 | 734496 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 36 | 1 | 33 | 0 | 3087 | 816 | 1192 | 0.0 | 0.0039 | 0 | 0 | 734746 |
| Critical | auth-public | logged-in-admin | mobile | /vi | 36 | 5 | 28 | 0 | 3494 | 812 | 1216 | 0.0 | 0.0032 | 1 | 0 | 615109 |
| Critical | auth-public | logged-in-reader | mobile | /vi | 36 | 5 | 28 | 0 | 3407 | 616 | 1036 | 9.0 | 0.0055 | 0 | 0 | 614822 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 35 | 0 | 33 | 0 | 4096 | 544 | 2064 | 0.0 | 0.0122 | 0 | 1 | 777003 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 33 | 3 | 27 | 0 | 3608 | 808 | 1192 | 0.0 | 0.0000 | 0 | 1 | 646627 |
| Critical | auth-public | logged-out | mobile | /vi | 31 | 2 | 27 | 0 | 3505 | 1148 | 1148 | 0.0 | 0.0000 | 0 | 1 | 603038 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 3068 | 612 | 992 | 0.0 | 0.0760 | 2 | 0 | 630706 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 3102 | 800 | 1140 | 8.0 | 0.0071 | 3 | 0 | 630928 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | 24 | 0 | 22 | 0 | 2728 | 664 | 1360 | 419.0 | 0.0035 | 3 | 0 | 511053 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2745 | 804 | 1576 | 386.0 | 0.0035 | 3 | 0 | 511157 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | 24 | 0 | 22 | 0 | 2738 | 1296 | 1296 | 374.0 | 0.0039 | 3 | 0 | 511167 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | 24 | 0 | 22 | 0 | 2806 | 700 | 1460 | 454.0 | 0.0033 | 3 | 0 | 511147 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | 24 | 0 | 22 | 0 | 2952 | 652 | 1060 | 0.0 | 0.0055 | 3 | 0 | 511197 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | 24 | 0 | 22 | 0 | 2817 | 712 | 1120 | 0.0 | 0.0055 | 3 | 0 | 511197 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 2878 | 1040 | 1040 | 0.0 | 0.0055 | 3 | 0 | 511180 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | 24 | 0 | 22 | 0 | 2975 | 680 | 1120 | 7.0 | 0.0055 | 1 | 0 | 511233 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/911a489a-96d1-48d1-9021-e8f1ef57c96b | 35 | 3 | 30 | 0 | 3741 | 916 | 3624 | 35.0 | 0.0001 | 0 | 0 | 696726 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/91e67450-1f09-4d11-a32b-9453dcc25b57 | 35 | 3 | 30 | 0 | 4374 | 1176 | 3968 | 29.0 | 0.0001 | 0 | 0 | 697041 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/51db55e6-0f53-4d34-a286-7b99161ba0c0 | 35 | 3 | 30 | 0 | 3352 | 748 | 3220 | 1.0 | 0.0000 | 0 | 0 | 696417 |
| High | admin | logged-in-admin | desktop | /vi/admin/gamification | 34 | 1 | 31 | 0 | 3361 | 776 | 1108 | 0.0 | 0.0022 | 0 | 0 | 700498 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 34 | 3 | 29 | 0 | 3207 | 540 | 1200 | 0.0 | 0.0000 | 0 | 0 | 649627 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 33 | 3 | 28 | 0 | 3329 | 744 | 1116 | 0.0 | 0.0180 | 0 | 0 | 652830 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 33 | 2 | 29 | 0 | 3262 | 728 | 1448 | 0.0 | 0.0000 | 0 | 0 | 730146 |
| High | admin | logged-in-admin | mobile | /vi/admin/gamification | 33 | 0 | 31 | 0 | 3942 | 744 | 744 | 7.0 | 0.0000 | 0 | 0 | 699395 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 33 | 2 | 29 | 0 | 3345 | 592 | 948 | 0.0 | 0.0071 | 0 | 0 | 728081 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 32 | 3 | 27 | 0 | 3089 | 660 | 1100 | 0.0 | 0.0543 | 0 | 0 | 639441 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 32 | 3 | 27 | 0 | 3194 | 628 | 1064 | 0.0 | 0.0192 | 0 | 0 | 646845 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 32 | 3 | 27 | 0 | 3037 | 644 | 1080 | 0.0 | 0.0039 | 0 | 0 | 635784 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 32 | 3 | 27 | 0 | 3231 | 668 | 1420 | 0.0 | 0.0095 | 0 | 0 | 637288 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 32 | 2 | 28 | 0 | 3608 | 824 | 1508 | 0.0 | 0.0000 | 0 | 0 | 650621 |
| High | admin | logged-in-admin | mobile | /vi/admin/users | 32 | 1 | 29 | 0 | 4217 | 976 | 1664 | 9.0 | 0.0000 | 0 | 0 | 651859 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 32 | 2 | 28 | 0 | 3820 | 640 | 1016 | 0.0 | 0.0000 | 0 | 0 | 647761 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 32 | 2 | 27 | 0 | 3319 | 744 | 1092 | 0.0 | 0.0330 | 0 | 0 | 638060 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 31 | 1 | 28 | 0 | 2937 | 724 | 1200 | 0.0 | 0.0042 | 0 | 0 | 645388 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 3043 | 696 | 1400 | 0.0 | 0.0042 | 0 | 0 | 726922 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 31 | 2 | 27 | 0 | 2895 | 736 | 1152 | 0.0 | 0.0489 | 0 | 0 | 634016 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 31 | 2 | 27 | 0 | 2912 | 800 | 1116 | 0.0 | 0.0042 | 0 | 0 | 635874 |
| High | admin | logged-in-admin | desktop | /vi/admin/users | 31 | 0 | 29 | 0 | 2781 | 584 | 1036 | 0.0 | 0.0000 | 0 | 0 | 651091 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/bac4d278-181b-4256-ad3b-81b7554f55a7 | 31 | 0 | 29 | 0 | 3373 | 784 | 1264 | 0.0 | 0.0042 | 0 | 0 | 713018 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | 2 | 27 | 0 | 2864 | 884 | 884 | 0.0 | 0.0042 | 0 | 0 | 634262 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 31 | 1 | 28 | 0 | 3025 | 928 | 1248 | 0.0 | 0.0039 | 0 | 0 | 645369 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 3137 | 668 | 1408 | 0.0 | 0.0039 | 0 | 0 | 725291 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 31 | 2 | 27 | 0 | 3602 | 832 | 1144 | 23.0 | 0.0726 | 0 | 0 | 637968 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 31 | 1 | 28 | 0 | 2781 | 664 | 1008 | 0.0 | 0.0177 | 0 | 0 | 650788 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 31 | 2 | 27 | 0 | 3288 | 868 | 868 | 0.0 | 0.0040 | 0 | 0 | 635271 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 31 | 2 | 27 | 0 | 3609 | 680 | 1404 | 0.0 | 0.0000 | 0 | 0 | 638392 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 31 | 2 | 27 | 0 | 3506 | 664 | 1052 | 4.0 | 0.0071 | 0 | 0 | 636464 |
| High | admin | logged-in-admin | mobile | /vi/admin/deposits | 31 | 1 | 28 | 0 | 4122 | 1448 | 1448 | 4.0 | 0.0000 | 0 | 0 | 649502 |
| High | admin | logged-in-admin | mobile | /vi/admin/readings | 31 | 1 | 28 | 0 | 5520 | 1468 | 1468 | 66.0 | 0.0000 | 0 | 0 | 650927 |
| High | admin | logged-in-admin | mobile | /vi/admin/system-configs | 31 | 0 | 29 | 0 | 4093 | 1280 | 1280 | 45.0 | 0.0000 | 0 | 0 | 690430 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 31 | 1 | 28 | 0 | 3379 | 748 | 1188 | 12.0 | 0.0000 | 0 | 0 | 727425 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 31 | 2 | 27 | 0 | 3386 | 764 | 764 | 0.0 | 0.0000 | 0 | 0 | 637114 |
| High | admin | logged-in-admin | desktop | /vi/admin | 30 | 0 | 28 | 0 | 3333 | 736 | 1100 | 0.0 | 0.0000 | 0 | 0 | 648698 |
| High | admin | logged-in-admin | desktop | /vi/admin/deposits | 30 | 0 | 28 | 0 | 2906 | 636 | 1116 | 0.0 | 0.0000 | 0 | 0 | 648737 |
| High | admin | logged-in-admin | desktop | /vi/admin/disputes | 30 | 0 | 28 | 0 | 3119 | 924 | 924 | 0.0 | 0.0000 | 0 | 0 | 647166 |
| High | admin | logged-in-admin | desktop | /vi/admin/reader-requests | 30 | 0 | 28 | 0 | 3717 | 1340 | 1704 | 0.0 | 0.0000 | 0 | 0 | 647803 |
| High | admin | logged-in-admin | desktop | /vi/admin/readings | 30 | 0 | 28 | 0 | 2847 | 620 | 1228 | 0.0 | 0.0000 | 0 | 0 | 650098 |
| High | admin | logged-in-admin | desktop | /vi/admin/withdrawals | 30 | 0 | 28 | 0 | 2825 | 792 | 792 | 0.0 | 0.0000 | 0 | 0 | 647429 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 30 | 1 | 27 | 0 | 2931 | 620 | 1016 | 0.0 | 0.0039 | 0 | 0 | 636788 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 30 | 0 | 28 | 0 | 3519 | 628 | 1876 | 0.0 | 0.0039 | 0 | 0 | 643184 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 2887 | 788 | 1204 | 0.0 | 0.0190 | 0 | 0 | 644748 |
| High | reading | logged-in-admin | mobile | /vi/reading | 30 | 2 | 26 | 0 | 3297 | 724 | 1092 | 0.0 | 0.0000 | 0 | 0 | 643844 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 3227 | 680 | 1020 | 0.0 | 0.0267 | 0 | 0 | 649802 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 30 | 2 | 26 | 0 | 3311 | 736 | 1112 | 0.0 | 0.0000 | 0 | 0 | 633549 |
| High | admin | logged-in-admin | mobile | /vi/admin | 30 | 0 | 28 | 0 | 3853 | 952 | 952 | 3.0 | 0.0000 | 0 | 0 | 648753 |
| High | admin | logged-in-admin | mobile | /vi/admin/disputes | 30 | 0 | 28 | 0 | 3355 | 700 | 1024 | 1.0 | 0.0000 | 0 | 0 | 647141 |
| High | admin | logged-in-admin | mobile | /vi/admin/promotions | 30 | 0 | 28 | 0 | 3637 | 652 | 1048 | 2.0 | 0.0000 | 0 | 0 | 647243 |
| High | admin | logged-in-admin | mobile | /vi/admin/reader-requests | 30 | 0 | 28 | 0 | 3273 | 700 | 1044 | 0.0 | 0.0000 | 0 | 0 | 647430 |
| High | admin | logged-in-admin | mobile | /vi/admin/withdrawals | 30 | 0 | 28 | 0 | 4014 | 916 | 916 | 3.0 | 0.0000 | 0 | 0 | 647242 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/6097dbe6-bd76-4d5f-bb2a-ab9f792dad25 | 30 | 0 | 28 | 0 | 4211 | 916 | 3888 | 19.0 | 0.0071 | 0 | 0 | 680672 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 3367 | 904 | 904 | 0.0 | 0.0196 | 0 | 0 | 649803 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 30 | 0 | 28 | 0 | 4052 | 604 | 1944 | 0.0 | 0.0051 | 0 | 0 | 643356 |
| High | auth-public | logged-out | desktop | /vi | 29 | 0 | 27 | 0 | 4035 | 1468 | 1468 | 407.0 | 0.0000 | 0 | 0 | 601137 |
| High | auth-public | logged-in-admin | desktop | /vi | 29 | 0 | 27 | 0 | 3410 | 752 | 1516 | 140.0 | 0.0035 | 0 | 0 | 607923 |
| High | reading | logged-in-admin | desktop | /vi/reading | 29 | 1 | 26 | 0 | 3054 | 736 | 1272 | 47.0 | 0.0042 | 0 | 0 | 642971 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 29 | 0 | 27 | 0 | 8145 | 724 | 724 | 432.0 | 0.0068 | 0 | 0 | 642668 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers | 29 | 1 | 26 | 0 | 2869 | 696 | 1168 | 0.0 | 0.0042 | 0 | 0 | 635012 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 29 | 1 | 26 | 0 | 2869 | 668 | 1344 | 0.0 | 0.0042 | 0 | 0 | 635243 |
| High | admin | logged-in-admin | desktop | /vi/admin/promotions | 29 | 0 | 27 | 0 | 2808 | 680 | 1076 | 0.0 | 0.0000 | 0 | 0 | 645651 |
| High | admin | logged-in-admin | desktop | /vi/admin/system-configs | 29 | 0 | 27 | 0 | 2769 | 684 | 1212 | 0.0 | 0.0000 | 0 | 0 | 656948 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/c71b0d22-3d30-482e-90bf-2aba6f525594 | 29 | 1 | 26 | 0 | 2822 | 668 | 1096 | 0.0 | 0.0042 | 0 | 0 | 633167 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/c1212558-1ee2-4535-8428-5bc9dee0ca63 | 29 | 1 | 26 | 0 | 2862 | 720 | 1144 | 0.0 | 0.0042 | 0 | 0 | 633117 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 1 | 26 | 0 | 2779 | 680 | 1360 | 0.0 | 0.0042 | 0 | 0 | 633618 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 29 | 1 | 26 | 0 | 2911 | 596 | 1220 | 0.0 | 0.0042 | 0 | 0 | 634019 |
| High | reading | logged-in-reader | desktop | /vi/reading | 29 | 1 | 26 | 0 | 2916 | 804 | 1276 | 0.0 | 0.0039 | 0 | 0 | 642872 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 7823 | 680 | 680 | 13.0 | 0.0040 | 0 | 0 | 642117 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 29 | 1 | 26 | 0 | 2888 | 648 | 1304 | 0.0 | 0.0039 | 0 | 0 | 635162 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 29 | 1 | 26 | 0 | 2931 | 788 | 1116 | 0.0 | 0.0039 | 0 | 0 | 632137 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 1 | 26 | 0 | 2919 | 680 | 1108 | 12.0 | 0.0039 | 0 | 0 | 632239 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 29 | 1 | 26 | 0 | 2836 | 676 | 1556 | 0.0 | 0.0039 | 0 | 0 | 633809 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 7704 | 640 | 640 | 170.0 | 0.0000 | 0 | 0 | 642894 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 3158 | 580 | 1128 | 0.0 | 0.0071 | 0 | 0 | 643512 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 7232 | 588 | 960 | 92.0 | 0.0071 | 0 | 0 | 642036 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 29 | 1 | 26 | 0 | 3088 | 536 | 1180 | 0.0 | 0.0000 | 0 | 0 | 635030 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 29 | 1 | 26 | 0 | 3762 | 792 | 1200 | 29.0 | 0.0000 | 0 | 0 | 635086 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 29 | 1 | 26 | 0 | 3021 | 524 | 896 | 0.0 | 0.0000 | 0 | 0 | 633474 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2933 | 956 | 956 | 0.0 | 0.0042 | 0 | 0 | 631370 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2772 | 816 | 816 | 0.0 | 0.0042 | 0 | 0 | 631603 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2819 | 892 | 892 | 0.0 | 0.0042 | 0 | 0 | 631556 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 2758 | 636 | 1196 | 0.0 | 0.0042 | 0 | 0 | 630827 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 2816 | 908 | 908 | 0.0 | 0.0042 | 0 | 0 | 632041 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 2909 | 996 | 996 | 0.0 | 0.0042 | 0 | 0 | 632463 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 3096 | 980 | 1244 | 6.0 | 0.0042 | 0 | 0 | 632985 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2813 | 784 | 784 | 0.0 | 0.0042 | 0 | 0 | 631298 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2763 | 684 | 1088 | 0.0 | 0.0042 | 0 | 0 | 631036 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2702 | 644 | 1312 | 0.0 | 0.0042 | 0 | 0 | 632909 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 2917 | 664 | 1124 | 0.0 | 0.0039 | 0 | 0 | 631908 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 2808 | 708 | 1616 | 0.0 | 0.0039 | 0 | 0 | 632467 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 2785 | 684 | 1152 | 0.0 | 0.0039 | 0 | 0 | 631553 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 2743 | 624 | 1100 | 0.0 | 0.0039 | 0 | 0 | 631497 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 3207 | 1000 | 1000 | 0.0 | 0.0039 | 0 | 0 | 632332 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 3579 | 1336 | 1452 | 9.0 | 0.0039 | 0 | 0 | 632966 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/05452963-7459-4a8a-abfb-b0786a8fdbc2 | 28 | 0 | 26 | 0 | 2829 | 732 | 1148 | 0.0 | 0.0039 | 0 | 0 | 632370 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/6721ff32-2612-4eda-85ea-6951d1e83c8e | 28 | 0 | 26 | 0 | 2789 | 704 | 1072 | 0.0 | 0.0039 | 0 | 0 | 632376 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/2f79c2f6-b14e-467d-8ed8-123d97e236f2 | 28 | 0 | 26 | 0 | 2786 | 684 | 1108 | 0.0 | 0.0039 | 0 | 0 | 632465 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2850 | 692 | 1076 | 0.0 | 0.0039 | 0 | 0 | 631099 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2809 | 668 | 1364 | 0.0 | 0.0039 | 0 | 0 | 633049 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2868 | 680 | 1384 | 0.0 | 0.0039 | 0 | 0 | 633140 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 3603 | 944 | 944 | 0.0 | 0.0000 | 0 | 0 | 631491 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 28 | 0 | 26 | 0 | 3015 | 708 | 1064 | 0.0 | 0.0000 | 0 | 0 | 634185 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 28 | 0 | 26 | 0 | 3127 | 572 | 920 | 0.0 | 0.0071 | 0 | 0 | 631712 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 3854 | 656 | 1012 | 1.0 | 0.0074 | 0 | 0 | 631990 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 3457 | 1076 | 1076 | 38.0 | 0.0000 | 0 | 0 | 632544 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 4196 | 1184 | 1184 | 19.0 | 0.0000 | 0 | 0 | 632990 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 4135 | 1252 | 1252 | 7.0 | 0.0000 | 0 | 0 | 631018 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 3142 | 912 | 912 | 9.0 | 0.0000 | 0 | 0 | 630991 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 3361 | 704 | 1072 | 4.0 | 0.0000 | 0 | 0 | 631020 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 3705 | 1012 | 1012 | 18.0 | 0.0071 | 0 | 0 | 632805 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 3411 | 832 | 1284 | 17.0 | 0.0000 | 0 | 0 | 632876 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 3321 | 976 | 976 | 21.0 | 0.0071 | 0 | 0 | 632996 |
| High | reading | logged-in-reader | mobile | /vi/reading | 28 | 0 | 26 | 0 | 3530 | 740 | 1100 | 0.0 | 0.0000 | 0 | 0 | 641814 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 3292 | 716 | 716 | 0.0 | 0.0000 | 0 | 0 | 631601 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 2968 | 552 | 916 | 0.0 | 0.0000 | 0 | 0 | 632277 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 3389 | 704 | 704 | 0.0 | 0.0000 | 0 | 0 | 631903 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 3300 | 1040 | 1040 | 0.0 | 0.0000 | 0 | 0 | 631714 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3291 | 768 | 768 | 0.0 | 0.0000 | 0 | 0 | 631858 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 3062 | 696 | 696 | 0.0 | 0.0000 | 0 | 0 | 632318 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 3105 | 616 | 1016 | 0.0 | 0.0071 | 0 | 0 | 633057 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/25415b3b-65fa-43b9-b954-7b93040a41de | 28 | 0 | 26 | 0 | 3142 | 560 | 928 | 0.0 | 0.0001 | 0 | 0 | 632560 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/21dcc838-2bcf-4b80-9ebb-6858c22eff45 | 28 | 0 | 26 | 0 | 2976 | 596 | 940 | 0.0 | 0.0000 | 0 | 0 | 632028 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 3726 | 764 | 764 | 0.0 | 0.0071 | 0 | 0 | 631071 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 3623 | 988 | 988 | 3.0 | 0.0000 | 0 | 0 | 631566 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 3132 | 552 | 880 | 0.0 | 0.0000 | 0 | 0 | 631309 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 3061 | 744 | 744 | 0.0 | 0.0000 | 0 | 0 | 632698 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 3211 | 756 | 756 | 1.0 | 0.0000 | 0 | 0 | 633307 |
| High | auth-public | logged-in-reader | desktop | /vi | 26 | 0 | 24 | 0 | 2983 | 780 | 1536 | 457.0 | 0.0038 | 0 | 0 | 537438 |
| High | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 3026 | 652 | 652 | 0.0 | 0.0000 | 0 | 0 | 525859 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 4037 | 1432 | 1432 | 6.0 | 0.0032 | 0 | 0 | 526135 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3514 | 716 | 1068 | 24.0 | 0.0055 | 0 | 0 | 526226 |
| High | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2864 | 780 | 780 | 0.0 | 0.0000 | 0 | 0 | 511769 |
| High | auth-public | logged-in-reader | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 3330 | 544 | 544 | 0.0 | 0.0000 | 0 | 0 | 512031 |
| Medium | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2817 | 656 | 656 | 0.0 | 0.0000 | 0 | 0 | 525771 |
| Medium | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2660 | 568 | 568 | 0.0 | 0.0000 | 0 | 0 | 525944 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2682 | 648 | 968 | 0.0 | 0.0020 | 0 | 0 | 526199 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2708 | 668 | 1028 | 0.0 | 0.0020 | 0 | 0 | 526246 |
| Medium | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2679 | 652 | 1000 | 0.0 | 0.0020 | 0 | 0 | 526350 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 2890 | 608 | 1008 | 0.0 | 0.0019 | 0 | 0 | 526359 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2753 | 672 | 1152 | 0.0 | 0.0019 | 0 | 0 | 526144 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2991 | 992 | 992 | 0.0 | 0.0019 | 0 | 0 | 526362 |
| Medium | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 2794 | 684 | 684 | 0.0 | 0.0000 | 0 | 0 | 525880 |
| Medium | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2817 | 660 | 660 | 0.0 | 0.0000 | 0 | 0 | 525753 |
| Medium | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2788 | 628 | 628 | 0.0 | 0.0000 | 0 | 0 | 525852 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3461 | 664 | 1016 | 6.0 | 0.0032 | 0 | 0 | 526355 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 3253 | 992 | 992 | 0.0 | 0.0032 | 0 | 0 | 526059 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3317 | 972 | 972 | 0.0 | 0.0032 | 0 | 0 | 526283 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3214 | 604 | 924 | 0.0 | 0.0032 | 0 | 0 | 526216 |
| Medium | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 2688 | 608 | 608 | 0.0 | 0.0000 | 0 | 0 | 512322 |
| Medium | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 2652 | 596 | 596 | 0.0 | 0.0000 | 0 | 0 | 512723 |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2848 | 784 | 784 | 0.0 | 0.0000 | 0 | 0 | 511864 |
| Medium | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2850 | 772 | 772 | 0.0 | 0.0000 | 0 | 0 | 511960 |
| Medium | auth-public | logged-in-admin | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2789 | 676 | 676 | 0.0 | 0.0000 | 0 | 0 | 511927 |
| Medium | auth-public | logged-in-admin | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2909 | 732 | 732 | 0.0 | 0.0000 | 0 | 0 | 512091 |
| Medium | auth-public | logged-in-reader | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2800 | 716 | 716 | 0.0 | 0.0000 | 0 | 0 | 511884 |
| Medium | auth-public | logged-in-reader | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 2824 | 844 | 844 | 0.0 | 0.0000 | 0 | 0 | 512042 |
| Medium | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 3116 | 752 | 752 | 0.0 | 0.0000 | 0 | 0 | 512199 |
| Medium | auth-public | logged-out | mobile | /vi/register | 24 | 0 | 22 | 0 | 2947 | 736 | 736 | 0.0 | 0.0000 | 0 | 0 | 512798 |
| Medium | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 2865 | 684 | 684 | 0.0 | 0.0000 | 0 | 0 | 511811 |
| Medium | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2812 | 616 | 616 | 0.0 | 0.0000 | 0 | 0 | 511926 |
| Medium | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2868 | 700 | 700 | 0.0 | 0.0000 | 0 | 0 | 511964 |
| Medium | auth-public | logged-in-admin | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 2808 | 616 | 616 | 0.0 | 0.0000 | 0 | 0 | 511994 |
| Medium | auth-public | logged-in-admin | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2984 | 724 | 724 | 0.0 | 0.0000 | 0 | 0 | 512118 |
| Medium | auth-public | logged-in-reader | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 2968 | 624 | 624 | 0.0 | 0.0000 | 0 | 0 | 512086 |

## Major Issues Found

- Critical: 24 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: 135 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 31 page(s) có request trong dải 400-800ms.
- Duplicate: 92 nhóm duplicate request cần kiểm tra over-fetch/cache key.
- Pending: 11 page(s) có pending request không phải websocket/eventsource.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 3784 | 778 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 2832 | 404 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 2726 | 430 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F08_The_Chariot_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2238 | 1545 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F16_The_Devil_50_20260325_181357.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 2119 | 721 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2103 | 899 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2100 | 954 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2094 | 866 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2082 | 1170 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 2070 | 1045 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 2070 | 721 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 2053 | 781 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 2023 | 764 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 2015 | 747 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F16_The_Devil_50_20260325_181357.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1977 | 327 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1948 | 337 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1908 | 422 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1905 | 928 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1897 | 405 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1878 | 180 | static | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1877 | 349 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1868 | 735 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1855 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1840 | 173 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1836 | 177 | static | https://www.tarotnow.xyz/_next/static/chunks/0w-sn531cssmu.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1834 | 535 | html | https://www.tarotnow.xyz/vi/admin/readings |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1828 | 74 | static | https://www.tarotnow.xyz/_next/static/chunks/08uelq136ram9.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1822 | 161 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1821 | 988 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F08_The_Chariot_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1819 | 977 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1819 | 163 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1818 | 718 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1817 | 409 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1813 | 1030 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1812 | 1004 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1811 | 119 | static | https://www.tarotnow.xyz/_next/static/chunks/07o~3chaybnsv.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1803 | 151 | static | https://www.tarotnow.xyz/_next/static/chunks/0c1ibrluv4zpr.js |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/911a489a-96d1-48d1-9021-e8f1ef57c96b | GET | 200 | 1794 | 1391 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| Critical | auth-public | logged-in-admin | mobile | /vi | POST | 200 | 1784 | 1755 | other | https://www.tarotnow.xyz/vi |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1777 | 146 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1774 | 151 | static | https://www.tarotnow.xyz/_next/static/chunks/0o8.vmbmcqfjl.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1769 | 159 | static | https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1766 | 730 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1763 | 117 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1758 | 728 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1745 | 927 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1745 | 669 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1731 | 147 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1689 | 325 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1669 | 647 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1630 | 149 | static | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1627 | 629 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1625 | 111 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1617 | 118 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1604 | 587 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1598 | 758 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1592 | 208 | static | https://www.tarotnow.xyz/_next/static/chunks/0b5a588g_0r8q.js |
| Critical | admin | logged-in-admin | desktop | /vi/admin/reader-requests | GET | 200 | 1588 | 992 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1579 | 317 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1577 | 133 | static | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1555 | 132 | static | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1546 | 691 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1495 | 323 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 1464 | 531 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | GET | 200 | 1462 | 363 | html | https://www.tarotnow.xyz/vi/wallet |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1425 | 116 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | GET | 200 | 1393 | 386 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1366 | 1356 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1364 | 382 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/users | GET | 200 | 1353 | 480 | html | https://www.tarotnow.xyz/vi/admin/users |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 1340 | 325 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | GET | 200 | 1334 | 359 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/tos | GET | 200 | 1297 | 431 | html | https://www.tarotnow.xyz/vi/legal/tos |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | GET | 200 | 1290 | 384 | html | https://www.tarotnow.xyz/vi/gacha |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1290 | 38 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | GET | 200 | 1282 | 360 | html | https://www.tarotnow.xyz/vi/profile |
| Critical | admin | logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1269 | 62 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 1269 | 302 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/deposits | GET | 200 | 1265 | 374 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | GET | 200 | 1261 | 329 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0u9nl9qa6hch7.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/08uelq136ram9.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0u9nl9qa6hch7.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/08uelq136ram9.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-reader | desktop | /vi/forgot-password | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| High | auth-public | logged-in-reader | mobile | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=384&q=75 |
| Critical | auth-public | logged-in-admin | mobile | /vi | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=1200&q=75 |

## Optimization Plan

1. Phase 1 complete: auth-public presence/session gating is covered by regression tests and post-deploy `auth-public` benchmark shows `0` pending requests.
2. Continue with Phase 2: collection image proxy latency remains the most visible feature bottleneck.
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

- Baseline benchmark: completed with `full-matrix` coverage across 190 pages before optimization.
- Hotspot fixed: collection card proxy images now bypass Next Image optimization through `shouldUseUnoptimizedImage()`.
- Local verification: `npx vitest run src/shared/http/assetUrl.test.ts` and `npm run lint` passed.
- GitHub Actions: `CD Fast Deploy` run `25510638702` completed successfully after push to `main`.
- Post-deploy full production benchmark: completed with `full-matrix` coverage across 190 pages.
- Post-deploy collection validation: benchmark artifact shows `0` wrapped `/_next/image?...collection%2Fcard-image...` requests, and slow collection proxy image entries in the report return `200` directly from `/api/collection/card-image`.
- Phase 1 shared auth/session/presence validation: commit `f5b922d8` added regression coverage for locale-aware realtime gating, `PresenceProvider`, and `usePresenceConnection` disabled/unauthenticated behavior.
- Phase 1 local verification: `npx vitest run src/shared/navigation/normalizePathname.test.ts src/app/_shared/providers/PresenceProvider.test.tsx src/app/_shared/hooks/usePresenceConnection.test.tsx` passed with 43 tests, and `npm run lint` passed.
- Phase 1 GitHub Actions: `CD Fast Deploy` run `25513544998` completed successfully after push to `main`.
- Phase 1 production benchmark: `BENCHMARK_FEATURE=auth-public` feature-matrix completed at `2026-05-07T18:37:06.354Z` with 54 auth-public pages and `0` pending requests.
- Phase 1 root-cause note: `/api/readers?page=1&pageSize=4` on logged-in auth routes is caused by expected redirect from auth entry pages to `/${locale}`, where the home page renders featured readers; no production code change was justified for that request.

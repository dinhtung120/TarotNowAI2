# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-07T16:44:26.552Z
- Benchmark generated at (UTC): 2026-05-07T16:43:54.710Z
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 190
- Critical pages: 29
- High pages: 140
- Medium pages: 21
- Slow requests >800ms: 263
- Slow requests 400-800ms: 1383

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.6 | 3611 | 1 | 0 | yes |
| logged-in-admin | desktop | 48 | 30.2 | 3466 | 9 | 3 | yes |
| logged-in-reader | desktop | 38 | 28.7 | 3816 | 7 | 1 | yes |
| logged-out | mobile | 9 | 25.0 | 3375 | 0 | 0 | yes |
| logged-in-admin | mobile | 48 | 29.1 | 3816 | 12 | 2 | yes |
| logged-in-reader | mobile | 38 | 29.6 | 3574 | 10 | 2 | yes |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | 61 | 5 | 50 | 0 | 5258 | 980 | 980 | 29.0 | 0.0055 | 0 | 1 | 1125991 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | 54 | 0 | 49 | 0 | 4084 | 680 | 1296 | 128.0 | 0.0035 | 0 | 1 | 1119015 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 39 | 2 | 35 | 0 | 4778 | 784 | 1896 | 0.0 | 0.0122 | 0 | 2 | 781208 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 38 | 3 | 33 | 0 | 4150 | 664 | 2356 | 0.0 | 0.0042 | 0 | 1 | 780024 |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 38 | 2 | 34 | 0 | 4556 | 860 | 1804 | 2.0 | 0.0039 | 0 | 1 | 779846 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 38 | 3 | 33 | 0 | 4335 | 596 | 992 | 16.0 | 0.0071 | 0 | 0 | 801045 |
| Critical | auth-public | logged-in-admin | desktop | /vi | 36 | 5 | 28 | 0 | 3374 | 740 | 1404 | 274.0 | 0.0040 | 0 | 1 | 612917 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 36 | 1 | 33 | 0 | 3274 | 796 | 1268 | 0.0 | 0.0042 | 0 | 0 | 652906 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 36 | 1 | 33 | 0 | 3126 | 668 | 1156 | 0.0 | 0.0042 | 0 | 0 | 734690 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/40c8682e-c4a6-4006-aeb5-f3a2848aa890 | 36 | 3 | 31 | 0 | 4394 | 1624 | 4384 | 0.0 | 0.0042 | 0 | 0 | 727608 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/5612dac7-3d73-410e-b123-448b564cbd29 | 36 | 3 | 31 | 0 | 3335 | 716 | 1136 | 0.0 | 0.0039 | 0 | 0 | 727548 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 36 | 1 | 33 | 0 | 3215 | 716 | 1044 | 3.0 | 0.0000 | 0 | 0 | 798720 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 35 | 0 | 33 | 0 | 4464 | 968 | 2028 | 19.0 | 0.0127 | 0 | 1 | 776846 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 30 | 2 | 26 | 0 | 3312 | 748 | 1108 | 0.0 | 0.0071 | 2 | 0 | 633015 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 3012 | 868 | 1196 | 0.0 | 0.0543 | 1 | 0 | 630769 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 3176 | 828 | 1072 | 0.0 | 0.0042 | 2 | 0 | 630706 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 3720 | 956 | 956 | 0.0 | 0.0000 | 1 | 0 | 630792 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 4170 | 1340 | 1340 | 1.0 | 0.0071 | 2 | 0 | 631082 |
| Critical | auth-public | logged-out | desktop | /vi | 26 | 0 | 24 | 0 | 7167 | 5120 | 5664 | 177.0 | 0.0000 | 1 | 0 | 536049 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | 24 | 0 | 22 | 0 | 2838 | 716 | 1436 | 397.0 | 0.0035 | 3 | 0 | 511159 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 3269 | 596 | 1348 | 358.0 | 0.0035 | 3 | 0 | 511192 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | 24 | 0 | 22 | 0 | 3189 | 1172 | 1172 | 511.0 | 0.0028 | 1 | 0 | 511215 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | 24 | 0 | 22 | 0 | 3977 | 1212 | 1212 | 338.0 | 0.0033 | 3 | 0 | 511054 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2956 | 1144 | 1144 | 204.0 | 0.0033 | 3 | 0 | 511132 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | 24 | 0 | 22 | 0 | 2915 | 732 | 1128 | 0.0 | 0.0055 | 3 | 0 | 511123 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | 24 | 0 | 22 | 0 | 2989 | 876 | 876 | 0.0 | 0.0055 | 3 | 0 | 511159 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 3143 | 876 | 876 | 0.0 | 0.0055 | 1 | 0 | 511274 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | 24 | 0 | 22 | 0 | 2991 | 724 | 724 | 0.0 | 0.0055 | 7 | 0 | 511194 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | 24 | 0 | 22 | 0 | 3211 | 1052 | 1052 | 15.0 | 0.0055 | 3 | 0 | 511220 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 35 | 0 | 33 | 0 | 4354 | 952 | 1224 | 0.0 | 0.0039 | 0 | 0 | 652014 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 35 | 0 | 33 | 0 | 4339 | 868 | 1184 | 0.0 | 0.0039 | 0 | 0 | 733525 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 35 | 4 | 29 | 0 | 4735 | 916 | 1176 | 0.0 | 0.0039 | 0 | 0 | 648982 |
| High | auth-public | logged-in-admin | mobile | /vi | 35 | 5 | 27 | 0 | 3278 | 624 | 996 | 0.0 | 0.0032 | 0 | 0 | 612908 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 35 | 0 | 33 | 0 | 3324 | 784 | 784 | 0.0 | 0.0071 | 0 | 0 | 661164 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 35 | 0 | 33 | 0 | 3863 | 928 | 1240 | 0.0 | 0.0071 | 0 | 0 | 661159 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 34 | 4 | 28 | 0 | 4240 | 580 | 904 | 0.0 | 0.0000 | 0 | 0 | 645307 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 33 | 3 | 28 | 0 | 6751 | 924 | 924 | 6.0 | 0.0043 | 0 | 0 | 646679 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 33 | 3 | 28 | 0 | 3431 | 652 | 972 | 0.0 | 0.0180 | 0 | 0 | 652525 |
| High | admin | logged-in-admin | desktop | /vi/admin/gamification | 33 | 0 | 31 | 0 | 3259 | 872 | 872 | 0.0 | 0.0022 | 0 | 0 | 699318 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 33 | 2 | 29 | 0 | 3624 | 832 | 1248 | 0.0 | 0.0177 | 0 | 0 | 652850 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 33 | 3 | 28 | 0 | 3393 | 592 | 1228 | 0.0 | 0.0689 | 0 | 0 | 651517 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 33 | 3 | 28 | 0 | 3782 | 668 | 1872 | 0.0 | 0.0071 | 0 | 0 | 648841 |
| High | admin | logged-in-admin | mobile | /vi/admin/gamification | 33 | 0 | 31 | 0 | 3817 | 860 | 860 | 0.0 | 0.0000 | 0 | 0 | 699470 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/dbf7bf79-8985-4f55-9cb8-7526f7d372d1 | 33 | 2 | 29 | 0 | 2990 | 564 | 900 | 0.0 | 0.0000 | 0 | 0 | 684771 |
| High | admin | logged-in-admin | desktop | /vi/admin/readings | 32 | 1 | 29 | 0 | 3658 | 624 | 1108 | 0.0 | 0.0000 | 0 | 0 | 652354 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 31 | 3 | 26 | 0 | 3058 | 648 | 1248 | 0.0 | 0.0042 | 0 | 0 | 636959 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 31 | 2 | 27 | 0 | 3245 | 688 | 1232 | 0.0 | 0.0042 | 0 | 0 | 635564 |
| High | admin | logged-in-admin | desktop | /vi/admin/deposits | 31 | 1 | 28 | 0 | 2940 | 704 | 1144 | 0.0 | 0.0000 | 0 | 0 | 649639 |
| High | admin | logged-in-admin | desktop | /vi/admin/system-configs | 31 | 0 | 29 | 0 | 3275 | 832 | 832 | 0.0 | 0.0000 | 0 | 0 | 690578 |
| High | admin | logged-in-admin | desktop | /vi/admin/users | 31 | 0 | 29 | 0 | 4425 | 748 | 1076 | 0.0 | 0.0000 | 0 | 0 | 650938 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/ba825483-debd-42e1-8276-0e7d409c3c79 | 31 | 0 | 29 | 0 | 4563 | 864 | 864 | 0.0 | 0.0042 | 0 | 0 | 712776 |
| High | reading | logged-in-reader | desktop | /vi/reading | 31 | 2 | 27 | 0 | 3485 | 888 | 1336 | 0.0 | 0.0039 | 0 | 0 | 644586 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/b64487ca-fd79-4151-8fab-bc500bfc1d5d | 31 | 0 | 29 | 0 | 4807 | 1148 | 1148 | 1.0 | 0.0039 | 0 | 0 | 712753 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/4f5fca30-31f1-4dbe-998d-e660c1d4f8ca | 31 | 0 | 29 | 0 | 3253 | 984 | 984 | 0.0 | 0.0039 | 0 | 0 | 712722 |
| High | admin | logged-in-admin | mobile | /vi/admin/readings | 31 | 1 | 28 | 0 | 3163 | 688 | 1012 | 0.0 | 0.0000 | 0 | 0 | 650760 |
| High | admin | logged-in-admin | mobile | /vi/admin/users | 31 | 0 | 29 | 0 | 4557 | 960 | 1280 | 104.0 | 0.0000 | 0 | 0 | 650824 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 31 | 3 | 26 | 0 | 3154 | 612 | 972 | 0.0 | 0.0000 | 0 | 0 | 637285 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 31 | 2 | 27 | 0 | 3454 | 732 | 732 | 0.0 | 0.0000 | 0 | 0 | 635553 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 31 | 2 | 27 | 0 | 3375 | 684 | 1032 | 0.0 | 0.0000 | 0 | 0 | 636275 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 30 | 0 | 28 | 0 | 2960 | 692 | 1380 | 0.0 | 0.0042 | 0 | 0 | 725864 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers | 30 | 1 | 27 | 0 | 3028 | 628 | 1036 | 0.0 | 0.0042 | 0 | 0 | 636275 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 30 | 1 | 27 | 0 | 3272 | 952 | 1184 | 0.0 | 0.0192 | 0 | 0 | 644540 |
| High | admin | logged-in-admin | desktop | /vi/admin | 30 | 0 | 28 | 0 | 5385 | 1116 | 1116 | 0.0 | 0.0000 | 0 | 0 | 648831 |
| High | admin | logged-in-admin | desktop | /vi/admin/disputes | 30 | 0 | 28 | 0 | 3502 | 732 | 1044 | 0.0 | 0.0000 | 0 | 0 | 647077 |
| High | admin | logged-in-admin | desktop | /vi/admin/reader-requests | 30 | 0 | 28 | 0 | 3660 | 656 | 1016 | 0.0 | 0.0000 | 0 | 0 | 647632 |
| High | admin | logged-in-admin | desktop | /vi/admin/withdrawals | 30 | 0 | 28 | 0 | 3093 | 756 | 1140 | 0.0 | 0.0000 | 0 | 0 | 647217 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 30 | 0 | 28 | 0 | 3378 | 908 | 1216 | 0.0 | 0.0039 | 0 | 0 | 724340 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 30 | 0 | 28 | 0 | 3477 | 744 | 1116 | 0.0 | 0.0000 | 0 | 0 | 725976 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 2986 | 588 | 956 | 0.0 | 0.0196 | 0 | 0 | 649599 |
| High | admin | logged-in-admin | mobile | /vi/admin | 30 | 0 | 28 | 0 | 3364 | 972 | 972 | 0.0 | 0.0000 | 0 | 0 | 648507 |
| High | admin | logged-in-admin | mobile | /vi/admin/deposits | 30 | 0 | 28 | 0 | 3504 | 852 | 852 | 0.0 | 0.0000 | 0 | 0 | 648631 |
| High | admin | logged-in-admin | mobile | /vi/admin/disputes | 30 | 0 | 28 | 0 | 4297 | 1108 | 1108 | 0.0 | 0.0000 | 0 | 0 | 646997 |
| High | admin | logged-in-admin | mobile | /vi/admin/reader-requests | 30 | 0 | 28 | 0 | 3289 | 704 | 704 | 0.0 | 0.0000 | 0 | 0 | 647466 |
| High | admin | logged-in-admin | mobile | /vi/admin/system-configs | 30 | 0 | 28 | 0 | 3893 | 1004 | 1004 | 11.0 | 0.0000 | 0 | 0 | 688721 |
| High | admin | logged-in-admin | mobile | /vi/admin/withdrawals | 30 | 0 | 28 | 0 | 3925 | 836 | 836 | 0.0 | 0.0000 | 0 | 0 | 647170 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/a2da1f5d-11d6-4f4f-ab62-4731bdc6c283 | 30 | 0 | 28 | 0 | 4105 | 780 | 780 | 0.0 | 0.0071 | 0 | 0 | 680738 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/578a906c-f1f9-48e8-85c9-151d5b2006bb | 30 | 0 | 28 | 0 | 3337 | 936 | 936 | 0.0 | 0.0000 | 0 | 0 | 680714 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/83ffd345-0a78-499a-a531-76aa85fd75fc | 30 | 0 | 28 | 0 | 3207 | 776 | 776 | 0.0 | 0.0000 | 0 | 0 | 680784 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 30 | 0 | 28 | 0 | 3174 | 680 | 1008 | 0.0 | 0.0071 | 0 | 0 | 724042 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 3692 | 840 | 840 | 3.0 | 0.0267 | 0 | 0 | 649841 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/1442aa76-4628-40db-8c84-154cb4ca96f2 | 30 | 0 | 28 | 0 | 4371 | 828 | 1164 | 0.0 | 0.0071 | 0 | 0 | 681255 |
| High | reading | logged-in-admin | desktop | /vi/reading | 29 | 1 | 26 | 0 | 2988 | 772 | 1196 | 0.0 | 0.0042 | 0 | 0 | 642602 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 29 | 0 | 27 | 0 | 3592 | 656 | 1084 | 0.0 | 0.0543 | 0 | 0 | 636044 |
| High | admin | logged-in-admin | desktop | /vi/admin/promotions | 29 | 0 | 27 | 0 | 3142 | 696 | 1004 | 0.0 | 0.0000 | 0 | 0 | 645695 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 7326 | 684 | 684 | 53.0 | 0.0040 | 0 | 0 | 642250 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 29 | 0 | 27 | 0 | 4119 | 652 | 1136 | 0.0 | 0.0740 | 0 | 0 | 636159 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 29 | 1 | 26 | 0 | 3104 | 716 | 1572 | 0.0 | 0.0039 | 0 | 0 | 633146 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 3487 | 844 | 1240 | 0.0 | 0.0190 | 0 | 0 | 643557 |
| High | auth-public | logged-out | mobile | /vi | 29 | 0 | 27 | 0 | 3516 | 1124 | 1124 | 15.0 | 0.0000 | 0 | 0 | 600990 |
| High | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 7493 | 716 | 716 | 75.0 | 0.0000 | 0 | 0 | 643035 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 29 | 0 | 27 | 0 | 4007 | 808 | 1132 | 0.0 | 0.0071 | 0 | 0 | 640642 |
| High | admin | logged-in-admin | mobile | /vi/admin/promotions | 29 | 0 | 27 | 0 | 5006 | 740 | 740 | 0.0 | 0.0000 | 0 | 0 | 645663 |
| High | auth-public | logged-in-reader | mobile | /vi | 29 | 0 | 27 | 0 | 3567 | 888 | 888 | 3.0 | 0.0055 | 0 | 0 | 607785 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 7588 | 684 | 684 | 159.0 | 0.0071 | 0 | 0 | 642073 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 29 | 0 | 27 | 0 | 3382 | 904 | 904 | 0.0 | 0.0892 | 0 | 0 | 643611 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 3448 | 780 | 1168 | 0.0 | 0.0071 | 0 | 0 | 643521 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 3961 | 1140 | 1140 | 0.0 | 0.0042 | 0 | 0 | 631443 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 3157 | 904 | 904 | 0.0 | 0.0042 | 0 | 0 | 631457 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 3254 | 676 | 1128 | 0.0 | 0.0042 | 0 | 0 | 631543 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 3722 | 676 | 1100 | 0.0 | 0.0047 | 0 | 0 | 631885 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 3494 | 652 | 1068 | 0.0 | 0.0042 | 0 | 0 | 632546 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 3171 | 732 | 1152 | 2.0 | 0.0042 | 0 | 0 | 632785 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/tos | 28 | 1 | 24 | 0 | 3304 | 772 | 772 | 0.0 | 0.0020 | 0 | 0 | 528948 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 28 | 1 | 24 | 0 | 2871 | 840 | 840 | 0.0 | 0.0020 | 0 | 0 | 528952 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 28 | 1 | 24 | 0 | 3350 | 644 | 960 | 0.0 | 0.0020 | 0 | 0 | 529013 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/f771ea6f-a728-470b-aeda-8ea9b9745849 | 28 | 0 | 26 | 0 | 2760 | 644 | 1068 | 0.0 | 0.0042 | 0 | 0 | 632171 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 3927 | 848 | 1228 | 0.0 | 0.0042 | 0 | 0 | 631012 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 2790 | 676 | 1068 | 0.0 | 0.0042 | 0 | 0 | 630886 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2901 | 640 | 1040 | 0.0 | 0.0042 | 0 | 0 | 630994 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 2868 | 584 | 1220 | 0.0 | 0.0042 | 0 | 0 | 632764 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 3014 | 648 | 1068 | 0.0 | 0.0042 | 0 | 0 | 632668 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 4015 | 608 | 1040 | 0.0 | 0.0042 | 0 | 0 | 632937 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 3532 | 684 | 1100 | 0.0 | 0.0039 | 0 | 0 | 631805 |
| High | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 3281 | 880 | 880 | 0.0 | 0.0039 | 0 | 0 | 631573 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 3988 | 1616 | 1804 | 16.0 | 0.0039 | 0 | 0 | 634146 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 3348 | 988 | 988 | 0.0 | 0.0039 | 0 | 0 | 631494 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 2969 | 704 | 1128 | 0.0 | 0.0039 | 0 | 0 | 631583 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 3343 | 676 | 1224 | 3.0 | 0.0095 | 0 | 0 | 633050 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 3345 | 1180 | 1180 | 25.0 | 0.0040 | 0 | 0 | 631941 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 3492 | 676 | 1076 | 0.0 | 0.0039 | 0 | 0 | 632461 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 9890 | 832 | 1320 | 0.0 | 0.0039 | 0 | 0 | 632900 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 3454 | 696 | 1084 | 0.0 | 0.0039 | 0 | 0 | 630945 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 3849 | 920 | 920 | 0.0 | 0.0039 | 0 | 0 | 631172 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 2972 | 820 | 820 | 0.0 | 0.0039 | 0 | 0 | 631223 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 3856 | 1284 | 1284 | 0.0 | 0.0039 | 0 | 0 | 632694 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 3435 | 1028 | 1028 | 17.0 | 0.0039 | 0 | 0 | 632969 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2985 | 848 | 848 | 0.0 | 0.0039 | 0 | 0 | 632979 |
| High | reading | logged-in-admin | mobile | /vi/reading | 28 | 0 | 26 | 0 | 3298 | 840 | 1208 | 0.0 | 0.0000 | 0 | 0 | 641704 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 3340 | 784 | 784 | 0.0 | 0.0071 | 0 | 0 | 631402 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 28 | 0 | 26 | 0 | 3079 | 612 | 948 | 0.0 | 0.0000 | 0 | 0 | 631716 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 3346 | 804 | 1128 | 0.0 | 0.0000 | 0 | 0 | 634049 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 3496 | 836 | 836 | 0.0 | 0.0071 | 0 | 0 | 631360 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 9859 | 736 | 736 | 0.0 | 0.0071 | 0 | 0 | 632018 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 3418 | 700 | 700 | 0.0 | 0.0074 | 0 | 0 | 631972 |
| High | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 3278 | 828 | 828 | 0.0 | 0.0000 | 0 | 0 | 632362 |
| High | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 3164 | 796 | 796 | 0.0 | 0.0000 | 0 | 0 | 633072 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 3339 | 676 | 676 | 5.0 | 0.0000 | 0 | 0 | 630905 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 3523 | 916 | 916 | 0.0 | 0.0000 | 0 | 0 | 631049 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 3686 | 796 | 796 | 0.0 | 0.0071 | 0 | 0 | 632702 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 4082 | 1132 | 1132 | 0.0 | 0.0071 | 0 | 0 | 632717 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 3522 | 816 | 816 | 0.0 | 0.0071 | 0 | 0 | 633088 |
| High | reading | logged-in-reader | mobile | /vi/reading | 28 | 0 | 26 | 0 | 3715 | 688 | 1048 | 0.0 | 0.0071 | 0 | 0 | 641727 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 3439 | 720 | 720 | 0.0 | 0.0071 | 0 | 0 | 631604 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 3700 | 656 | 1012 | 0.0 | 0.0071 | 0 | 0 | 631950 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 28 | 0 | 26 | 0 | 3722 | 1072 | 1484 | 0.0 | 0.0000 | 0 | 0 | 634241 |
| High | reader-chat | logged-in-reader | mobile | /vi/chat | 28 | 0 | 26 | 0 | 4103 | 964 | 964 | 0.0 | 0.0071 | 0 | 0 | 631587 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 3424 | 700 | 700 | 9.0 | 0.0071 | 0 | 0 | 631669 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 3130 | 596 | 1032 | 0.0 | 0.0401 | 0 | 0 | 632941 |
| High | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 2794 | 556 | 892 | 0.0 | 0.0000 | 0 | 0 | 632270 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 2929 | 652 | 992 | 0.0 | 0.0000 | 0 | 0 | 632996 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/b182f99a-0ec5-4c0d-bd79-2e079da622c3 | 28 | 0 | 26 | 0 | 2831 | 560 | 928 | 0.0 | 0.0001 | 0 | 0 | 632346 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 28 | 0 | 26 | 0 | 2770 | 548 | 868 | 0.0 | 0.0000 | 0 | 0 | 631266 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 3291 | 628 | 956 | 0.0 | 0.0071 | 0 | 0 | 631304 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 3243 | 588 | 968 | 0.0 | 0.0071 | 0 | 0 | 632808 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 2882 | 636 | 976 | 0.0 | 0.0000 | 0 | 0 | 632841 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 0 | 26 | 0 | 2816 | 596 | 944 | 0.0 | 0.0000 | 0 | 0 | 633052 |
| High | auth-public | logged-in-reader | desktop | /vi | 26 | 0 | 24 | 0 | 2957 | 740 | 1420 | 330.0 | 0.0033 | 0 | 0 | 537522 |
| High | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 3392 | 812 | 812 | 0.0 | 0.0000 | 0 | 0 | 525773 |
| High | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3326 | 888 | 888 | 0.0 | 0.0000 | 0 | 0 | 525743 |
| High | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3748 | 684 | 684 | 0.0 | 0.0000 | 0 | 0 | 525890 |
| High | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 3464 | 732 | 732 | 0.0 | 0.0000 | 0 | 0 | 525774 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 5298 | 832 | 832 | 0.0 | 0.0055 | 0 | 0 | 526127 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3336 | 708 | 708 | 0.0 | 0.0055 | 0 | 0 | 526210 |
| High | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 3265 | 612 | 612 | 0.0 | 0.0000 | 0 | 0 | 512025 |
| High | auth-public | logged-in-reader | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 3836 | 1008 | 1008 | 0.0 | 0.0000 | 0 | 0 | 512011 |
| High | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 3787 | 1040 | 1040 | 0.0 | 0.0000 | 0 | 0 | 512312 |
| High | auth-public | logged-in-admin | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 4025 | 660 | 660 | 0.0 | 0.0000 | 0 | 0 | 512054 |
| High | auth-public | logged-in-reader | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 4218 | 776 | 776 | 0.0 | 0.0000 | 0 | 0 | 511947 |
| High | auth-public | logged-in-reader | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 3604 | 928 | 928 | 0.0 | 0.0000 | 0 | 0 | 512039 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 3027 | 936 | 936 | 0.0 | 0.0019 | 0 | 0 | 526280 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3300 | 980 | 980 | 0.0 | 0.0019 | 0 | 0 | 526198 |
| Medium | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3169 | 808 | 808 | 0.0 | 0.0019 | 0 | 0 | 526220 |
| Medium | auth-public | logged-out | mobile | /vi/register | 25 | 1 | 22 | 0 | 3219 | 700 | 700 | 0.0 | 0.0000 | 0 | 0 | 513483 |
| Medium | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3068 | 684 | 684 | 0.0 | 0.0000 | 0 | 0 | 525692 |
| Medium | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3383 | 788 | 788 | 0.0 | 0.0000 | 0 | 0 | 525896 |
| Medium | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3253 | 884 | 884 | 0.0 | 0.0032 | 0 | 0 | 526182 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 3027 | 584 | 908 | 0.0 | 0.0032 | 0 | 0 | 526284 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2796 | 636 | 636 | 0.0 | 0.0032 | 0 | 0 | 526193 |
| Medium | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 2802 | 552 | 860 | 0.0 | 0.0032 | 0 | 0 | 526328 |
| Medium | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 3149 | 640 | 640 | 0.0 | 0.0000 | 0 | 0 | 512118 |
| Medium | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 2786 | 688 | 688 | 0.0 | 0.0000 | 0 | 0 | 512666 |
| Medium | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 2699 | 660 | 660 | 0.0 | 0.0000 | 0 | 0 | 511800 |
| Medium | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2970 | 592 | 592 | 0.0 | 0.0000 | 0 | 0 | 511920 |
| Medium | auth-public | logged-in-admin | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2854 | 844 | 844 | 0.0 | 0.0000 | 0 | 0 | 511933 |
| Medium | auth-public | logged-in-admin | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 3046 | 776 | 776 | 0.0 | 0.0000 | 0 | 0 | 511945 |
| Medium | auth-public | logged-in-reader | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 2966 | 672 | 672 | 0.0 | 0.0000 | 0 | 0 | 511914 |
| Medium | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 3101 | 728 | 728 | 0.0 | 0.0000 | 0 | 0 | 511905 |
| Medium | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 3345 | 936 | 936 | 0.0 | 0.0000 | 0 | 0 | 511923 |
| Medium | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 3492 | 1000 | 1000 | 0.0 | 0.0000 | 0 | 0 | 511959 |
| Medium | auth-public | logged-in-admin | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 3361 | 1172 | 1172 | 0.0 | 0.0000 | 0 | 0 | 512006 |

## Major Issues Found

- Critical: 29 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: 140 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 21 page(s) có request trong dải 400-800ms.
- Duplicate: 49 nhóm duplicate request cần kiểm tra over-fetch/cache key.
- Pending: 16 page(s) có pending request không phải websocket/eventsource.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | reading | logged-in-reader | desktop | /vi/reading/history | GET | 200 | 7299 | 7253 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | GET | 200 | 6875 | 6721 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 2993 | 2296 | html | https://www.tarotnow.xyz/vi |
| Critical | admin | logged-in-admin | desktop | /vi/admin | GET | 200 | 2582 | 2175 | static | https://www.tarotnow.xyz/_next/static/chunks/17_nn5bb0qiei.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/tos | GET | 200 | 2565 | 774 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 2359 | 230 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 2356 | 151 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 2308 | 233 | static | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 2280 | 229 | static | https://www.tarotnow.xyz/_next/static/chunks/142dx04-f3f2d.js |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers | GET | 200 | 2279 | 280 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 2269 | 232 | static | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 2269 | 290 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 2263 | 232 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 2217 | 292 | static | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 2133 | 234 | static | https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 2101 | 229 | static | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 2094 | 233 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-out | desktop | /vi | GET | 200 | 2086 | 782 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| Critical | admin | logged-in-admin | desktop | /vi/admin/users | GET | 200 | 1988 | 1688 | static | https://www.tarotnow.xyz/_next/static/chunks/17_nn5bb0qiei.js |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/ba825483-debd-42e1-8276-0e7d409c3c79 | GET | 200 | 1948 | 1928 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 1947 | 1670 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/b64487ca-fd79-4151-8fab-bc500bfc1d5d | GET | 200 | 1877 | 1833 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1849 | 1115 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 1843 | 1703 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1804 | 1623 | api | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 1765 | 1627 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 1753 | 1620 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/1442aa76-4628-40db-8c84-154cb4ca96f2 | GET | 200 | 1705 | 1328 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 1694 | 1641 | static | https://www.tarotnow.xyz/_next/static/chunks/17_nn5bb0qiei.js |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | GET | 200 | 1648 | 198 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1628 | 655 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1621 | 631 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | GET | 200 | 1604 | 1173 | static | https://www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/40c8682e-c4a6-4006-aeb5-f3a2848aa890 | GET | 200 | 1601 | 1213 | html | https://www.tarotnow.xyz/vi/reading/session/40c8682e-c4a6-4006-aeb5-f3a2848aa890 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1598 | 1460 | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1578 | 1460 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1558 | 638 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1552 | 894 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1542 | 657 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | GET | 200 | 1522 | 719 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | GET | 200 | 1515 | 657 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | GET | 200 | 1490 | 389 | html | https://www.tarotnow.xyz/vi/gamification |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | GET | 200 | 1477 | 992 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/ba825483-debd-42e1-8276-0e7d409c3c79 | GET | 200 | 1470 | 428 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1433 | 652 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | GET | 200 | 1430 | 759 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | GET | 200 | 1406 | 815 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/reset-password | GET | 200 | 1372 | 549 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1354 | 346 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1347 | 351 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | GET | 200 | 1340 | 787 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | GET | 200 | 1330 | 1311 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/a2da1f5d-11d6-4f4f-ab62-4731bdc6c283 | GET | 200 | 1324 | 1085 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | GET | 200 | 1302 | 234 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | GET | 200 | 1298 | 1248 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | GET | 200 | 1296 | 349 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | admin | logged-in-admin | desktop | /vi/admin/readings | GET | 200 | 1290 | 252 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | auth-public | logged-in-reader | mobile | /vi/reset-password | GET | 200 | 1286 | 1140 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 1284 | 914 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1268 | 298 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1262 | 307 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | GET | 200 | 1243 | 1212 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1243 | 1201 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | GET | 200 | 1240 | 302 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | GET | 200 | 1238 | 302 | api | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | GET | 200 | 1236 | 371 | html | https://www.tarotnow.xyz/vi/notifications |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/a2da1f5d-11d6-4f4f-ab62-4731bdc6c283 | GET | 200 | 1226 | 955 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | auth-public | logged-out | desktop | /vi/legal/tos | GET | 200 | 1217 | 417 | html | https://www.tarotnow.xyz/vi/legal/tos |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers | GET | 200 | 1208 | 222 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/chat | GET | 200 | 1199 | 1132 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | admin | logged-in-admin | desktop | /vi/admin/reader-requests | GET | 200 | 1197 | 794 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | admin | logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 1182 | 992 | static | https://www.tarotnow.xyz/_next/static/chunks/17_nn5bb0qiei.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1177 | 1159 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fdefense_booster_50_20260416_165452.avif&w=48&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | GET | 200 | 1177 | 205 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | auth-public | logged-out | mobile | /vi/login | GET | 200 | 1172 | 937 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | GET | 200 | 1149 | 930 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/b64487ca-fd79-4151-8fab-bc500bfc1d5d | GET | 200 | 1147 | 400 | static | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1146 | 1112 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Ffree_draw_ticket_50_20260416_165452.avif&w=48&q=75 |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | GET | 200 | 1146 | 901 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | GET | 200 | 1137 | 1111 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fexp_booster_50_20260416_165452.avif&w=48&q=75 |

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
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-admin | desktop | /vi/register | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 2 | GET https://www.tarotnow.xyz/_next/image?q=75&url=https://media.tarotnow.xyz/avatars/1bf7374304584c0488e06621bbc1454f.webp&w=128 |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/05smpou-mjs5j.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| High | auth-public | logged-in-reader | mobile | /vi/forgot-password | 2 | GET https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| Critical | auth-public | logged-out | desktop | /vi | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=384&q=75 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=1200&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0.6weo7lob0cs.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/api/wallet/balance |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/api/chat/unread-count |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/api/notifications/unread-count |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/vi |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

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

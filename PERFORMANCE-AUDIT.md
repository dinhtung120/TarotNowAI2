# PERFORMANCE AUDIT - TarotNow

## Executive Summary

- Generated at (UTC): 2026-05-07T10:26:33.496Z
- Benchmark generated at (UTC): 2026-05-07T10:26:27.420Z
- Base origin: https://www.tarotnow.xyz
- Locale prefix: /vi
- Benchmark mode: full-matrix
- Total scenarios: 6
- Total pages measured: 163
- Critical pages: 75
- High pages: 86
- Medium pages: 2
- Slow requests >800ms: 3481
- Slow requests 400-800ms: 1387

## Scenario Coverage

| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 25.1 | 4394 | 0 | 1 | yes |
| logged-in-admin | desktop | 39 | 29.0 | 5878 | 82 | 1 | yes |
| logged-in-reader | desktop | 36 | 29.1 | 5477 | 62 | 1 | yes |
| logged-out | mobile | 9 | 24.9 | 4441 | 1 | 0 | yes |
| logged-in-admin | mobile | 35 | 28.7 | 5563 | 88 | 1 | yes |
| logged-in-reader | mobile | 35 | 28.7 | 6976 | 110 | 1 | yes |

## Detailed Metrics Table

| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Critical | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 41 | 4 | 35 | 0 | 7030 | 1372 | 2936 | 0.0 | 0.0039 | 0 | 1 | 790364 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha | 39 | 3 | 34 | 0 | 8821 | 1636 | 1636 | 11.0 | 0.0042 | 0 | 0 | 738101 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/community | 38 | 3 | 33 | 0 | 7775 | 1540 | 7076 | 5.0 | 0.0042 | 0 | 1 | 770658 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | 37 | 2 | 33 | 0 | 5595 | 892 | 2560 | 0.0 | 0.0173 | 1 | 1 | 778749 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | 37 | 2 | 33 | 0 | 5904 | 1312 | 1648 | 0.0 | 0.0071 | 1 | 0 | 663697 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/inventory | 36 | 1 | 33 | 0 | 5482 | 1936 | 1936 | 0.0 | 0.0042 | 0 | 0 | 653271 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | 36 | 1 | 33 | 0 | 4828 | 1468 | 1468 | 12.0 | 0.0039 | 2 | 0 | 734914 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/inventory | 36 | 1 | 33 | 0 | 4457 | 1068 | 1396 | 0.0 | 0.0071 | 0 | 0 | 662457 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | 36 | 1 | 33 | 0 | 5745 | 1124 | 2340 | 0.0 | 0.0173 | 2 | 1 | 768963 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | 35 | 0 | 33 | 0 | 7564 | 2216 | 2356 | 0.0 | 0.0071 | 2 | 0 | 798049 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | 35 | 0 | 33 | 0 | 6906 | 1048 | 1384 | 0.0 | 0.0071 | 3 | 0 | 783556 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | 32 | 2 | 28 | 0 | 5091 | 1060 | 1060 | 0.0 | 0.0071 | 3 | 0 | 728133 |
| Critical | auth-public | logged-out | desktop | /vi | 31 | 2 | 27 | 0 | 5694 | 2744 | 2744 | 383.0 | 0.0024 | 0 | 1 | 602981 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | 31 | 2 | 27 | 0 | 4250 | 1028 | 1348 | 0.0 | 0.0000 | 2 | 0 | 633854 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/90a1dff0-637a-4c24-ae37-1f6d19808a82 | 31 | 0 | 29 | 0 | 5392 | 1556 | 1556 | 0.0 | 0.0042 | 2 | 0 | 705119 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/a234d7ae-09a6-4ea5-8183-2599752598ad | 31 | 0 | 29 | 0 | 6623 | 2728 | 2728 | 0.0 | 0.0042 | 3 | 0 | 705050 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 31 | 2 | 27 | 0 | 5285 | 1664 | 1664 | 0.0 | 0.0042 | 1 | 0 | 626692 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | 2 | 27 | 0 | 5387 | 1432 | 1432 | 3.0 | 0.0042 | 1 | 0 | 628673 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/b9f507fb-919b-488f-b8ca-2b02a8e9fc65 | 31 | 0 | 29 | 0 | 4377 | 944 | 944 | 0.0 | 0.0039 | 2 | 0 | 712909 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 31 | 2 | 27 | 0 | 3708 | 816 | 1140 | 0.0 | 0.0071 | 2 | 0 | 627696 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 31 | 2 | 27 | 0 | 5314 | 1208 | 1532 | 0.0 | 0.0071 | 2 | 0 | 627188 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | 30 | 1 | 27 | 0 | 6893 | 2104 | 2104 | 0.0 | 0.0043 | 16 | 0 | 643731 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | 30 | 2 | 26 | 0 | 4628 | 1308 | 1308 | 0.0 | 0.0039 | 3 | 0 | 633858 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/8dfcd983-416a-4565-870e-7ace00ba2e5d | 30 | 0 | 28 | 0 | 4446 | 712 | 1188 | 0.0 | 0.0072 | 1 | 0 | 673308 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/797fea9f-b1fc-4e17-8906-c4dfd78107b4 | 30 | 0 | 28 | 0 | 8107 | 2640 | 2980 | 0.0 | 0.0072 | 3 | 0 | 673330 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 30 | 2 | 26 | 0 | 5019 | 880 | 880 | 0.0 | 0.0071 | 2 | 0 | 625841 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/chat | 30 | 2 | 26 | 0 | 4425 | 1668 | 2008 | 0.0 | 0.0071 | 1 | 0 | 626453 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 4835 | 768 | 1100 | 0.0 | 0.0267 | 3 | 0 | 642416 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | 30 | 0 | 28 | 0 | 13745 | 1544 | 1544 | 0.0 | 0.0072 | 6 | 0 | 673396 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/a3c19aa3-0e8f-4421-8a74-0f5f2474dec8 | 30 | 0 | 28 | 0 | 8568 | 3908 | 4244 | 0.0 | 0.0072 | 2 | 0 | 673655 |
| Critical | auth-public | logged-in-admin | desktop | /vi | 29 | 0 | 27 | 0 | 4333 | 1256 | 1256 | 367.0 | 0.0040 | 1 | 0 | 607759 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | 29 | 0 | 27 | 0 | 4557 | 1768 | 1768 | 0.0 | 0.0042 | 2 | 0 | 635309 |
| Critical | auth-public | logged-in-reader | desktop | /vi | 29 | 0 | 27 | 0 | 4407 | 776 | 1452 | 925.0 | 0.0038 | 3 | 0 | 607933 |
| Critical | auth-public | logged-out | mobile | /vi | 29 | 0 | 27 | 0 | 6214 | 2648 | 2648 | 0.0 | 0.0000 | 1 | 0 | 604321 |
| Critical | auth-public | logged-in-admin | mobile | /vi | 29 | 0 | 27 | 0 | 8027 | 2740 | 2740 | 0.0 | 0.0055 | 2 | 0 | 607880 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | 29 | 0 | 27 | 0 | 6641 | 900 | 900 | 0.0 | 0.0071 | 15 | 0 | 643085 |
| Critical | auth-public | logged-in-reader | mobile | /vi | 29 | 0 | 27 | 0 | 5371 | 1460 | 1816 | 0.0 | 0.0024 | 6 | 0 | 607777 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | 29 | 0 | 27 | 0 | 4649 | 696 | 1016 | 0.0 | 0.0071 | 11 | 0 | 634868 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 5842 | 1280 | 1280 | 0.0 | 0.0042 | 2 | 0 | 624551 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 4735 | 1000 | 1304 | 0.0 | 0.0000 | 14 | 0 | 624120 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 11957 | 1808 | 1808 | 4.0 | 0.0042 | 2 | 0 | 624350 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/chat | 28 | 0 | 26 | 0 | 4048 | 1016 | 1016 | 0.0 | 0.0039 | 2 | 0 | 631752 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 5300 | 1744 | 2204 | 1.0 | 0.0039 | 1 | 0 | 634373 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 4664 | 1632 | 1632 | 0.0 | 0.0039 | 2 | 0 | 631874 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 4820 | 1812 | 1812 | 0.0 | 0.0095 | 2 | 0 | 633344 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 5150 | 1384 | 1384 | 0.0 | 0.0044 | 2 | 0 | 632285 |
| Critical | reading | logged-in-admin | mobile | /vi/reading | 28 | 0 | 26 | 0 | 4208 | 764 | 1088 | 0.0 | 0.0071 | 1 | 0 | 641618 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 5846 | 904 | 1256 | 0.0 | 0.0071 | 2 | 0 | 634219 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | 28 | 0 | 26 | 0 | 4128 | 1288 | 1288 | 0.0 | 0.0000 | 10 | 0 | 631219 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 5554 | 1596 | 1928 | 0.0 | 0.0074 | 3 | 0 | 631990 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 11128 | 7084 | 7084 | 0.0 | 0.0071 | 3 | 0 | 625769 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 5427 | 2344 | 2696 | 0.0 | 0.0071 | 4 | 0 | 625924 |
| Critical | reading | logged-in-reader | mobile | /vi/reading | 28 | 0 | 26 | 0 | 5200 | 976 | 976 | 0.0 | 0.0071 | 1 | 0 | 641470 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 4694 | 1184 | 1184 | 0.0 | 0.0071 | 2 | 0 | 624610 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | 28 | 0 | 26 | 0 | 4190 | 776 | 1088 | 0.0 | 0.0071 | 1 | 0 | 627147 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 7061 | 2104 | 2360 | 0.0 | 0.0071 | 2 | 0 | 624594 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | 28 | 0 | 26 | 0 | 4402 | 1484 | 1516 | 0.0 | 0.0074 | 2 | 0 | 625442 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | 28 | 0 | 26 | 0 | 8090 | 4860 | 5192 | 0.0 | 0.0071 | 1 | 0 | 625620 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 11807 | 7476 | 7796 | 0.0 | 0.0071 | 2 | 0 | 624291 |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 5829 | 2440 | 2748 | 0.0 | 0.0024 | 2 | 0 | 520133 |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 7446 | 3176 | 3492 | 0.0 | 0.0024 | 4 | 0 | 519820 |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 7045 | 3396 | 3712 | 0.0 | 0.0024 | 4 | 0 | 520060 |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 8497 | 5208 | 5524 | 0.0 | 0.0024 | 2 | 0 | 520003 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | 24 | 0 | 22 | 0 | 4277 | 1664 | 1664 | 241.0 | 0.0000 | 10 | 0 | 511180 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | 24 | 0 | 22 | 0 | 4061 | 1324 | 1324 | 296.0 | 0.0000 | 12 | 0 | 511190 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 4211 | 1744 | 1744 | 90.0 | 0.0000 | 14 | 0 | 511178 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | 24 | 0 | 22 | 0 | 4984 | - | - | 0.0 | 0.0024 | 23 | 0 | 511233 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | 24 | 0 | 22 | 0 | 5285 | 2124 | 2124 | 487.0 | 0.0028 | 8 | 0 | 511253 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 11642 | 2304 | 2304 | 0.0 | 0.0000 | 12 | 0 | 511220 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | 24 | 0 | 22 | 0 | 4603 | 1292 | 1292 | 0.0 | 0.0000 | 9 | 0 | 511197 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | 24 | 0 | 22 | 0 | 3564 | 648 | 1004 | 0.0 | 0.0024 | 4 | 0 | 511223 |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 3988 | - | - | 0.0 | 0.0000 | 19 | 0 | 511264 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | 24 | 0 | 22 | 0 | 5935 | 992 | 992 | 0.0 | 0.0384 | 11 | 0 | 511232 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | 24 | 0 | 22 | 0 | 8519 | - | - | 0.0 | 0.0000 | 22 | 0 | 511257 |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 5970 | - | - | 0.0 | 0.0000 | 19 | 0 | 511153 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/inventory | 35 | 0 | 33 | 0 | 3866 | 896 | 1164 | 0.0 | 0.0039 | 0 | 0 | 652458 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/e0ac9d62-58bd-41d2-9abb-fd7f5092131d | 34 | 2 | 30 | 0 | 6337 | 2176 | 2176 | 0.0 | 0.0039 | 0 | 0 | 715954 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/5d1c145a-5a71-4480-acf0-244f34f6ef76 | 34 | 2 | 30 | 0 | 7030 | 1868 | 1868 | 0.0 | 0.0039 | 0 | 0 | 716101 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 34 | 4 | 28 | 0 | 5570 | 1220 | 1220 | 0.0 | 0.0039 | 0 | 0 | 644702 |
| High | inventory-gacha-collection | logged-in-admin | desktop | /vi/gacha/history | 33 | 2 | 29 | 0 | 5555 | 2504 | 2504 | 0.0 | 0.0042 | 0 | 0 | 728962 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/leaderboard | 33 | 3 | 28 | 0 | 4575 | 728 | 1172 | 0.0 | 0.0180 | 0 | 0 | 644829 |
| High | reading | logged-in-reader | mobile | /vi/reading/session/37afc1bc-5feb-4b68-8c4b-da8ffc156f46 | 33 | 2 | 29 | 0 | 5163 | 1528 | 1528 | 0.0 | 0.0072 | 0 | 0 | 677129 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/leaderboard | 32 | 2 | 28 | 0 | 7875 | 4292 | 4292 | 0.0 | 0.0177 | 0 | 0 | 652053 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/d67a8827-530a-4358-ae07-5d5f6b08bd1d | 31 | 0 | 29 | 0 | 4171 | 1064 | 1064 | 0.0 | 0.0042 | 0 | 0 | 704807 |
| High | reading | logged-in-admin | desktop | /vi/reading/session/dedc486f-ae01-4175-8304-308508b8e6fd | 31 | 0 | 29 | 0 | 5395 | 1516 | 1516 | 0.0 | 0.0042 | 0 | 0 | 705535 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha/history | 31 | 1 | 28 | 0 | 6896 | 4056 | 4056 | 0.0 | 0.0039 | 0 | 0 | 724980 |
| High | reading | logged-in-reader | desktop | /vi/reading/history | 31 | 2 | 27 | 0 | 4207 | 1424 | 1424 | 0.0 | 0.0039 | 0 | 0 | 636145 |
| High | reading | logged-in-reader | desktop | /vi/reading/session/4cda2818-0afd-475d-9391-f8b697b5e44c | 31 | 0 | 29 | 0 | 8894 | 1688 | 1688 | 0.0 | 0.0039 | 0 | 0 | 712851 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile | 31 | 2 | 27 | 0 | 5094 | 1068 | 1400 | 0.0 | 0.0760 | 0 | 0 | 648373 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/withdraw | 31 | 2 | 27 | 0 | 5273 | 1512 | 1512 | 0.0 | 0.0401 | 0 | 0 | 629190 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | 2 | 27 | 0 | 4724 | 1116 | 1432 | 0.0 | 0.0071 | 0 | 0 | 627295 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/mfa | 30 | 2 | 26 | 0 | 5573 | 2716 | 2716 | 0.0 | 0.0042 | 0 | 0 | 633663 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/reader | 30 | 2 | 26 | 0 | 4873 | 860 | 1184 | 0.0 | 0.0000 | 0 | 0 | 632740 |
| High | reader-chat | logged-in-admin | mobile | /vi/chat | 30 | 2 | 26 | 0 | 4964 | 1156 | 1480 | 0.0 | 0.0071 | 0 | 0 | 633828 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/leaderboard | 30 | 0 | 28 | 0 | 5547 | 1160 | 1160 | 0.0 | 0.0267 | 0 | 0 | 649622 |
| High | community-leaderboard-quest | logged-in-admin | mobile | /vi/gamification | 30 | 1 | 27 | 0 | 6439 | 1232 | 1564 | 0.0 | 0.0071 | 0 | 0 | 644858 |
| High | reading | logged-in-admin | mobile | /vi/reading/session/37a5aa0f-5b91-4586-94a6-97ba219a8629 | 30 | 0 | 28 | 0 | 4885 | 928 | 1252 | 0.0 | 0.0072 | 0 | 0 | 673282 |
| High | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | 30 | 0 | 28 | 0 | 24110 | 4988 | 4988 | 0.0 | 0.0071 | 0 | 0 | 722191 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile | 29 | 0 | 27 | 0 | 6018 | 2552 | 2552 | 0.0 | 0.0543 | 0 | 0 | 636048 |
| High | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 13767 | 4680 | 4680 | 0.0 | 0.0192 | 0 | 0 | 636783 |
| High | inventory-gacha-collection | logged-in-reader | desktop | /vi/collection | 29 | 0 | 27 | 0 | 9153 | 2052 | 2052 | 32.0 | 0.0039 | 0 | 0 | 641943 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile | 29 | 0 | 27 | 0 | 4082 | 1172 | 1172 | 0.0 | 0.0740 | 0 | 0 | 636094 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers | 29 | 0 | 27 | 0 | 5004 | 2400 | 2400 | 0.0 | 0.0039 | 0 | 0 | 635531 |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/gamification | 29 | 0 | 27 | 0 | 5200 | 1792 | 1792 | 0.0 | 0.0190 | 0 | 0 | 643559 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers | 29 | 0 | 27 | 0 | 4406 | 800 | 1108 | 0.0 | 0.0071 | 0 | 0 | 640582 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile | 29 | 0 | 27 | 0 | 5089 | 1852 | 2184 | 0.0 | 0.0892 | 0 | 0 | 636515 |
| High | reader-chat | logged-in-reader | mobile | /vi/readers | 29 | 0 | 27 | 0 | 5492 | 2104 | 2348 | 0.0 | 0.0071 | 0 | 0 | 633637 |
| High | community-leaderboard-quest | logged-in-reader | mobile | /vi/gamification | 29 | 0 | 27 | 0 | 5169 | 1880 | 2356 | 0.0 | 0.0071 | 0 | 0 | 636445 |
| High | reading | logged-in-admin | desktop | /vi/reading | 28 | 0 | 26 | 0 | 5246 | 1980 | 1980 | 35.0 | 0.0042 | 0 | 0 | 641595 |
| High | reader-chat | logged-in-admin | desktop | /vi/chat | 28 | 0 | 26 | 0 | 11267 | 8184 | 8184 | 0.0 | 0.0042 | 0 | 0 | 624405 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet | 28 | 0 | 26 | 0 | 6081 | 1460 | 1460 | 3.0 | 0.0042 | 0 | 0 | 627272 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 6561 | 1108 | 1420 | 0.0 | 0.0042 | 0 | 0 | 625449 |
| High | profile-wallet-notifications | logged-in-admin | desktop | /vi/notifications | 28 | 0 | 26 | 0 | 4099 | 1012 | 1012 | 0.0 | 0.0042 | 0 | 0 | 624919 |
| High | reader-chat | logged-in-admin | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 4967 | 1088 | 1088 | 0.0 | 0.0042 | 0 | 0 | 625500 |
| High | reading | logged-in-admin | desktop | /vi/reading/history | 28 | 0 | 26 | 0 | 4887 | 1200 | 1200 | 1.0 | 0.0043 | 0 | 0 | 625801 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 6081 | 1468 | 1468 | 0.0 | 0.0042 | 0 | 0 | 624320 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 0 | 26 | 0 | 7158 | 1984 | 1984 | 2.0 | 0.0042 | 0 | 0 | 625858 |
| High | reader-chat | logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 0 | 26 | 0 | 6510 | 2788 | 2936 | 50.0 | 0.0042 | 0 | 0 | 625926 |
| High | reading | logged-in-reader | desktop | /vi/reading | 28 | 0 | 26 | 0 | 4188 | 824 | 1224 | 0.0 | 0.0039 | 0 | 0 | 641613 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/mfa | 28 | 0 | 26 | 0 | 4512 | 1432 | 1432 | 0.0 | 0.0039 | 0 | 0 | 631920 |
| High | profile-wallet-notifications | logged-in-reader | desktop | /vi/profile/reader | 28 | 0 | 26 | 0 | 4226 | 1508 | 1508 | 0.0 | 0.0039 | 0 | 0 | 632433 |
| High | reader-chat | logged-in-reader | desktop | /vi/reader/apply | 28 | 0 | 26 | 0 | 4639 | 1516 | 1516 | 0.0 | 0.0039 | 0 | 0 | 632474 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 5546 | 1724 | 1724 | 0.0 | 0.0039 | 0 | 0 | 631312 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 0 | 26 | 0 | 4847 | 1036 | 1036 | 0.0 | 0.0039 | 0 | 0 | 630988 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/profile/mfa | 28 | 0 | 26 | 0 | 4630 | 744 | 744 | 0.0 | 0.0071 | 0 | 0 | 631615 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit | 28 | 0 | 26 | 0 | 4336 | 892 | 1224 | 0.0 | 0.0071 | 0 | 0 | 631436 |
| High | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 4142 | 1160 | 1488 | 0.0 | 0.0071 | 0 | 0 | 632149 |
| High | reader-chat | logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 0 | 26 | 0 | 7369 | 4068 | 4388 | 0.0 | 0.0071 | 0 | 0 | 624322 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | 28 | 0 | 26 | 0 | 12575 | 9380 | 9380 | 0.0 | 0.0071 | 0 | 0 | 625295 |
| High | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit/history | 28 | 0 | 26 | 0 | 6050 | 2028 | 2028 | 0.0 | 0.0071 | 0 | 0 | 625035 |
| High | reading | logged-in-reader | mobile | /vi/reading/history | 28 | 0 | 26 | 0 | 6142 | 1004 | 1004 | 0.0 | 0.0071 | 0 | 0 | 626117 |
| High | auth-public | logged-out | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 9047 | 1004 | 1004 | 0.0 | 0.0000 | 0 | 0 | 525662 |
| High | auth-public | logged-out | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3539 | 1084 | 1084 | 0.0 | 0.0000 | 0 | 0 | 525900 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 4824 | 832 | 832 | 0.0 | 0.0020 | 0 | 0 | 520048 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3918 | 1140 | 1140 | 0.0 | 0.0020 | 0 | 0 | 520206 |
| High | auth-public | logged-in-admin | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 3583 | 876 | 876 | 0.0 | 0.0020 | 0 | 0 | 520003 |
| High | auth-public | logged-in-reader | desktop | /vi/legal/tos | 25 | 0 | 23 | 0 | 4711 | 1108 | 1108 | 0.0 | 0.0019 | 0 | 0 | 526163 |
| High | auth-public | logged-in-reader | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 5075 | 2080 | 2080 | 0.0 | 0.0019 | 0 | 0 | 526163 |
| High | auth-public | logged-in-reader | desktop | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 5607 | 2152 | 2152 | 0.0 | 0.0019 | 0 | 0 | 526249 |
| High | auth-public | logged-out | mobile | /vi/legal/tos | 25 | 0 | 23 | 0 | 3737 | 756 | 756 | 0.0 | 0.0000 | 0 | 0 | 525755 |
| High | auth-public | logged-out | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 3873 | 904 | 904 | 0.0 | 0.0000 | 0 | 0 | 525900 |
| High | auth-public | logged-out | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 4556 | 1412 | 1412 | 0.0 | 0.0000 | 0 | 0 | 526021 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/privacy | 25 | 0 | 23 | 0 | 5855 | 1632 | 1940 | 0.0 | 0.0055 | 0 | 0 | 519884 |
| High | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | 25 | 0 | 23 | 0 | 9214 | 4764 | 5080 | 0.0 | 0.0055 | 0 | 0 | 519982 |
| High | auth-public | logged-out | desktop | /vi/login | 24 | 0 | 22 | 0 | 3768 | 932 | 932 | 0.0 | 0.0000 | 0 | 0 | 512233 |
| High | auth-public | logged-out | desktop | /vi/register | 24 | 0 | 22 | 0 | 4361 | 1344 | 1344 | 0.0 | 0.0000 | 0 | 0 | 512759 |
| High | auth-public | logged-out | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 3708 | 908 | 908 | 0.0 | 0.0000 | 0 | 0 | 512045 |
| High | auth-public | logged-out | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 3457 | 988 | 988 | 0.0 | 0.0000 | 0 | 0 | 511978 |
| High | auth-public | logged-in-admin | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 4577 | 1200 | 1200 | 0.0 | 0.0000 | 0 | 0 | 511923 |
| High | auth-public | logged-in-admin | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 4553 | 1616 | 1616 | 0.0 | 0.0000 | 0 | 0 | 512020 |
| High | auth-public | logged-in-reader | desktop | /vi/reset-password | 24 | 0 | 22 | 0 | 4599 | 1824 | 1824 | 0.0 | 0.0001 | 0 | 0 | 511925 |
| High | auth-public | logged-in-reader | desktop | /vi/verify-email | 24 | 0 | 22 | 0 | 3959 | 1256 | 1256 | 0.0 | 0.0000 | 0 | 0 | 512221 |
| High | auth-public | logged-out | mobile | /vi/login | 24 | 0 | 22 | 0 | 4171 | 756 | 756 | 0.0 | 0.0000 | 0 | 0 | 512242 |
| High | auth-public | logged-out | mobile | /vi/register | 24 | 0 | 22 | 0 | 5148 | 1448 | 1448 | 0.0 | 0.0000 | 0 | 0 | 512727 |
| High | auth-public | logged-out | mobile | /vi/forgot-password | 24 | 0 | 22 | 0 | 3395 | 700 | 700 | 0.0 | 0.0000 | 0 | 0 | 511851 |
| High | auth-public | logged-out | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 5107 | 1924 | 1924 | 0.0 | 0.0000 | 0 | 0 | 511951 |
| High | auth-public | logged-out | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 3771 | 924 | 924 | 0.0 | 0.0000 | 0 | 0 | 512088 |
| High | auth-public | logged-in-admin | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 5163 | 1516 | 1516 | 0.0 | 0.0000 | 0 | 0 | 511894 |
| High | auth-public | logged-in-admin | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 4854 | 1356 | 1356 | 0.0 | 0.0000 | 0 | 0 | 512125 |
| High | auth-public | logged-in-reader | mobile | /vi/reset-password | 24 | 0 | 22 | 0 | 5101 | 1508 | 1508 | 0.0 | 0.0000 | 0 | 0 | 511931 |
| High | auth-public | logged-in-reader | mobile | /vi/verify-email | 24 | 0 | 22 | 0 | 4961 | 1348 | 1348 | 0.0 | 0.0000 | 0 | 0 | 512031 |
| Medium | auth-public | logged-out | desktop | /vi/legal/privacy | 25 | 0 | 23 | 0 | 2970 | 660 | 972 | 0.0 | 0.0000 | 0 | 0 | 525791 |
| Medium | auth-public | logged-out | desktop | /vi/forgot-password | 24 | 0 | 22 | 0 | 3003 | 872 | 872 | 0.0 | 0.0000 | 0 | 0 | 511847 |

## Major Issues Found

- Critical: 75 page(s) có request count >35, pending request, failed request, hoặc issue nghiêm trọng.
- High: 86 page(s) vượt ngưỡng >25 requests hoặc có request >800ms.
- Medium: 2 page(s) có request trong dải 400-800ms.
- Duplicate: 2 nhóm duplicate request cần kiểm tra over-fetch/cache key.
- Pending: 69 page(s) có pending request không phải websocket/eventsource.

### Slow Requests

| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 18462 | 2341 | static | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 18460 | 2341 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 18454 | 2341 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 18450 | 2341 | static | https://www.tarotnow.xyz/_next/static/chunks/13onjvrcl5bfv.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 18445 | 2254 | static | https://www.tarotnow.xyz/_next/static/chunks/0-7mtii.hadxk.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 18251 | 2341 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha/history | GET | 200 | 17834 | 2248 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 10970 | 582 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 10969 | 1060 | static | https://www.tarotnow.xyz/_next/static/chunks/06ujm1m6ocbc1.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 10966 | 1060 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 10543 | 885 | static | https://www.tarotnow.xyz/_next/static/chunks/06ujm1m6ocbc1.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 10540 | 910 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 10534 | 1965 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 10530 | 1944 | static | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 10500 | 896 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 10492 | 863 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 10478 | 1060 | static | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 10476 | 1060 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 10475 | 1060 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 10377 | 884 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 10167 | 1001 | static | https://www.tarotnow.xyz/_next/static/chunks/03kmcz0ax1e4~.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 10157 | 581 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 9645 | 1075 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 9644 | 1081 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 9638 | 1074 | static | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 9637 | 1058 | static | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 9622 | 1053 | static | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 9609 | 1049 | static | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 9533 | 54 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 9488 | 1060 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 9488 | 1060 | static | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 9482 | 2427 | static | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 9134 | 3912 | static | https://www.tarotnow.xyz/_next/static/chunks/06ujm1m6ocbc1.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 9117 | 2079 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 9113 | 3913 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 9072 | 862 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 8913 | 3914 | static | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 8904 | 3913 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 8801 | 3914 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 8593 | 8205 | static | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 8591 | 8205 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 8587 | 8205 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 8570 | 8209 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 8536 | 8174 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 8535 | 8192 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/reader | GET | 200 | 8492 | 8252 | html | https://www.tarotnow.xyz/vi/profile/reader |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | GET | 200 | 8286 | 7214 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 8203 | 8166 | static | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | GET | 200 | 8198 | 8178 | static | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 7975 | 1749 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 7964 | 1776 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 7962 | 1838 | static | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 7960 | 2395 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 7959 | 2394 | static | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 7958 | 2055 | static | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 7956 | 2394 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/chat | GET | 200 | 7829 | 655 | html | https://www.tarotnow.xyz/vi/chat |
| Critical | community-leaderboard-quest | logged-in-admin | desktop | /vi/gamification | GET | 200 | 7693 | 34 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7605 | 360 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7597 | 358 | static | https://www.tarotnow.xyz/_next/static/chunks/06ujm1m6ocbc1.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7596 | 363 | static | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7596 | 361 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7596 | 361 | static | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7593 | 358 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7587 | 162 | static | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7398 | 302 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7250 | 919 | static | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7248 | 919 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7247 | 917 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7246 | 915 | static | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7244 | 945 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7243 | 914 | static | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7239 | 945 | static | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7230 | 937 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7087 | 42 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7002 | 3921 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 6900 | 673 | static | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 6896 | 35 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/reader/apply | GET | 200 | 6700 | 831 | html | https://www.tarotnow.xyz/vi/reader/apply |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/ai-disclaimer | GET | 200 | 6578 | 1206 | static | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |

### Duplicate API / Request Candidates

| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | --- | --- | ---: | --- |
| High | community-leaderboard-quest | logged-in-reader | desktop | /vi/community | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| High | reader-chat | logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

### Pending Requests

| Severity | Feature | Scenario | Viewport | Route | URL |
| --- | --- | --- | --- | --- | --- |
| Critical | auth-public | logged-in-admin | desktop | /vi | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=384&q=75 |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/login | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/register | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-admin | desktop | /vi/forgot-password | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F08_The_Chariot_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F16_The_Devil_50_20260325_181357.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/06ujm1m6ocbc1.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | profile-wallet-notifications | logged-in-admin | desktop | /vi/wallet/withdraw | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/90a1dff0-637a-4c24-ae37-1f6d19808a82 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/90a1dff0-637a-4c24-ae37-1f6d19808a82 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/a234d7ae-09a6-4ea5-8183-2599752598ad | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/a234d7ae-09a6-4ea5-8183-2599752598ad | https://www.tarotnow.xyz/_next/static/chunks/09.78-sm.tf5_.js |
| Critical | reading | logged-in-admin | desktop | /vi/reading/session/a234d7ae-09a6-4ea5-8183-2599752598ad | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-reader | desktop | /vi | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/login | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/wallet/balance |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/chat/unread-count |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/notifications/unread-count |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/vi |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| Critical | auth-public | logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-reader | desktop | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| Critical | inventory-gacha-collection | logged-in-reader | desktop | /vi/gacha | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reader-chat | logged-in-reader | desktop | /vi/chat | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | desktop | /vi/chat | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/deposit/history | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | desktop | /vi/notifications | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/b9f507fb-919b-488f-b8ca-2b02a8e9fc65 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reading | logged-in-reader | desktop | /vi/reading/session/b9f507fb-919b-488f-b8ca-2b02a8e9fc65 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-out | mobile | /vi | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | auth-public | logged-in-admin | mobile | /vi | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | auth-public | logged-in-admin | mobile | /vi | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/login | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/wallet/balance |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/vi |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | auth-public | logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-admin | mobile | /vi/forgot-password | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | reading | logged-in-admin | mobile | /vi/reading | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/gacha/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F08_The_Chariot_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F16_The_Devil_50_20260325_181357.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | community-leaderboard-quest | logged-in-admin | mobile | /vi/community | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/13woiho5a~0-..js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/06ujm1m6ocbc1.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/wallet/withdraw | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-admin | mobile | /vi/notifications | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/reader/apply | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | reader-chat | logged-in-admin | mobile | /vi/reader/apply | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/reader/apply | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reading | logged-in-admin | mobile | /vi/reading/history | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | reading | logged-in-admin | mobile | /vi/reading/history | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | reading | logged-in-admin | mobile | /vi/reading/history | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reading | logged-in-admin | mobile | /vi/reading/history | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/tos | https://www.tarotnow.xyz/vi/legal/tos |
| Critical | auth-public | logged-in-admin | mobile | /vi/legal/tos | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/8dfcd983-416a-4565-870e-7ace00ba2e5d | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/797fea9f-b1fc-4e17-8906-c4dfd78107b4 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/797fea9f-b1fc-4e17-8906-c4dfd78107b4 | https://www.tarotnow.xyz/_next/static/chunks/09.78-sm.tf5_.js |
| Critical | reading | logged-in-admin | mobile | /vi/reading/session/797fea9f-b1fc-4e17-8906-c4dfd78107b4 | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| Critical | reader-chat | logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | auth-public | logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | auth-public | logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/api/wallet/balance |
| Critical | auth-public | logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | auth-public | logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | auth-public | logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/vi |
| Critical | auth-public | logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/login | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/125p1933fux_8.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/register | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/turbopack-04f4sychnhg.o.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/004blgc4x3yr..js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0m98db.6t5_en.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/03lv7o3p_.tam.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/174tgx6b~.hvn.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/18afy0ss_4u5l.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/16nwcytuy6f98.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0y1pmpupryerl.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0cclzt6w8o8v_.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/forgot-password | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| Critical | reading | logged-in-reader | mobile | /vi/reading | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/inventory | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=256&q=75 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/gacha | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F02_The_Magician_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F03_The_High+Priestess+_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | inventory-gacha-collection | logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/profile/mfa | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/chat | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=256&q=75 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/leaderboard | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=256&q=75 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | community-leaderboard-quest | logged-in-reader | mobile | /vi/community | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/wallet/deposit | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | profile-wallet-notifications | logged-in-reader | mobile | /vi/notifications | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/reader/apply | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/tos | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/tos | https://www.tarotnow.xyz/api/chat/unread-count |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/tos | https://www.tarotnow.xyz/vi/legal/tos |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/tos | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/privacy | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/privacy | https://www.tarotnow.xyz/api/chat/unread-count |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/privacy | https://www.tarotnow.xyz/vi/legal/privacy |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/privacy | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| Critical | auth-public | logged-in-reader | mobile | /vi/legal/ai-disclaimer | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | https://www.tarotnow.xyz/cdn-cgi/rum? |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | https://www.tarotnow.xyz/api/me/runtime-policies |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | https://www.tarotnow.xyz/api/reading/cards-catalog |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/0c7d37f7-e4f1-404c-9baf-bb83661d32da | https://www.tarotnow.xyz/_next/static/chunks/09.78-sm.tf5_.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/a3c19aa3-0e8f-4421-8a74-0f5f2474dec8 | https://www.tarotnow.xyz/_next/static/chunks/09.78-sm.tf5_.js |
| Critical | reading | logged-in-reader | mobile | /vi/reading/session/a3c19aa3-0e8f-4421-8a74-0f5f2474dec8 | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| Critical | reader-chat | logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |

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

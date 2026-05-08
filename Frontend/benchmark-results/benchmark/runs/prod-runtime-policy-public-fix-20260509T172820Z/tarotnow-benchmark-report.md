# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T17:43:12.201Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 7
- High pages (request count): 142
- High slow requests: 1770
- Medium slow requests: 660

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 3082 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 4717 | 1270 | 21 | 0 | 15 | 0 | yes |
| logged-in-reader | desktop | 33 | 3248 | 971 | 0 | 0 | 12 | 0 | yes |
| logged-out | mobile | 9 | 3240 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 6671 | 1252 | 65 | 0 | 4 | 0 | yes |
| logged-in-reader | mobile | 33 | 3145 | 1001 | 1 | 0 | 13 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.6 | 4815 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 4492 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 5423 | 1 |
| logged-in-admin | desktop | community | 1 | 37.0 | 7259 | 0 |
| logged-in-admin | desktop | gacha | 2 | 32.5 | 4139 | 2 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 4724 | 0 |
| logged-in-admin | desktop | home | 1 | 29.0 | 4973 | 0 |
| logged-in-admin | desktop | inventory | 1 | 32.0 | 3392 | 1 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 6130 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 4159 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 6511 | 1 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 4643 | 13 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 4350 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.6 | 4319 | 2 |
| logged-in-admin | desktop | reading | 5 | 31.8 | 4543 | 0 |
| logged-in-admin | desktop | wallet | 4 | 29.0 | 4946 | 1 |
| logged-in-admin | mobile | admin | 10 | 29.3 | 7130 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 6205 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 7702 | 13 |
| logged-in-admin | mobile | community | 1 | 34.0 | 9618 | 0 |
| logged-in-admin | mobile | gacha | 2 | 34.5 | 5060 | 1 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 7358 | 0 |
| logged-in-admin | mobile | home | 1 | 35.0 | 3965 | 0 |
| logged-in-admin | mobile | inventory | 1 | 35.0 | 4040 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 5420 | 1 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 6385 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 7924 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.3 | 5813 | 22 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 8531 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.0 | 5955 | 1 |
| logged-in-admin | mobile | reading | 5 | 29.2 | 6367 | 3 |
| logged-in-admin | mobile | wallet | 4 | 28.0 | 8643 | 24 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2778 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 7088 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3515 | 0 |
| logged-in-reader | desktop | gacha | 2 | 35.5 | 4057 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2915 | 0 |
| logged-in-reader | desktop | home | 1 | 35.0 | 3099 | 0 |
| logged-in-reader | desktop | inventory | 1 | 35.0 | 3040 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 3022 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2929 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2805 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.7 | 2857 | 0 |
| logged-in-reader | desktop | reader | 1 | 34.0 | 6241 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.0 | 2893 | 0 |
| logged-in-reader | desktop | reading | 5 | 28.6 | 2971 | 0 |
| logged-in-reader | desktop | wallet | 4 | 29.5 | 3031 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2824 | 0 |
| logged-in-reader | mobile | collection | 1 | 33.0 | 5764 | 0 |
| logged-in-reader | mobile | community | 1 | 37.0 | 3923 | 0 |
| logged-in-reader | mobile | gacha | 2 | 34.5 | 3227 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 3240 | 1 |
| logged-in-reader | mobile | home | 1 | 35.0 | 3058 | 0 |
| logged-in-reader | mobile | inventory | 1 | 36.0 | 3206 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2946 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2786 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 4535 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.3 | 2935 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2823 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.3 | 2891 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.2 | 2911 | 0 |
| logged-in-reader | mobile | wallet | 4 | 35.3 | 3266 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2897 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3969 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 3095 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2795 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 5409 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 3258 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3969 | 1359 | 1944 | 1080 | 1080 | 0.0000 | 492.0 | 601534 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3229 | 718 | 1217 | 764 | 764 | 0.0000 | 0.0 | 512370 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2865 | 848 | 856 | 688 | 688 | 0.0000 | 0.0 | 512972 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2778 | 772 | 772 | 612 | 612 | 0.0000 | 0.0 | 511972 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2818 | 806 | 806 | 644 | 644 | 0.0000 | 0.0 | 512185 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2797 | 790 | 790 | 612 | 612 | 0.0000 | 0.0 | 512198 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3534 | 689 | 1505 | 700 | 700 | 0.0000 | 0.0 | 525945 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2764 | 752 | 752 | 716 | 716 | 0.0000 | 0.0 | 526103 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2986 | 665 | 972 | 596 | 596 | 0.0000 | 0.0 | 526122 |
| logged-in-admin | desktop | auth-public | /vi | 29 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4973 | 1153 | 2966 | 1388 | 1388 | 0.0059 | 105.0 | 608117 |
| logged-in-admin | desktop | reading | /vi/reading | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3578 | 1344 | 1571 | 1052 | 1472 | 0.0041 | 0.0 | 644001 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 32 | 7 | high | 0 | 0 | 1 | 0 | 5 | 0 | 5 | 5 | 0 | 0 | 3392 | 1324 | 1383 | 1252 | 1560 | 0.0041 | 0.0 | 646946 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 35 | 6 | high | 0 | 0 | 0 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 4087 | 1387 | 2072 | 1496 | 1496 | 0.0041 | 0.0 | 732637 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4190 | 1908 | 2184 | 1004 | 1528 | 0.0041 | 0.0 | 726427 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5423 | 2120 | 2420 | 1208 | 1208 | 0.0041 | 0.0 | 643164 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4589 | 2057 | 2542 | 992 | 1632 | 0.0489 | 0.0 | 636882 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5100 | 1781 | 3093 | 1208 | 1572 | 0.0041 | 0.0 | 631868 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 34 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4240 | 0 | - | 1700 | 1700 | 0.0000 | 0.0 | 631108 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4096 | 1894 | 2090 | 1168 | 1472 | 0.0041 | 0.0 | 634075 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4492 | 1840 | 2449 | 1240 | 1240 | 0.0041 | 0.0 | 631878 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6130 | 3459 | 4121 | 2736 | 2736 | 0.0179 | 0.0 | 650841 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 37 | 3 | critical | 0 | 0 | 2 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 7259 | 3465 | 4532 | 2508 | 4636 | 0.0041 | 0.0 | 780064 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | 7 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4724 | 1885 | 2719 | 1288 | 1668 | 0.1240 | 0.0 | 643146 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5491 | 2866 | 3485 | 2036 | 2384 | 0.0041 | 0.0 | 634540 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4560 | 2233 | 2549 | 1736 | 1736 | 0.0041 | 0.0 | 633888 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5288 | 2104 | 3282 | 1284 | 1284 | 0.0041 | 0.0 | 632424 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 30 | 31 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4443 | 1775 | - | 992 | 1716 | 0.0000 | 0.0 | 633344 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6511 | 2965 | 4491 | 2428 | 2428 | 0.0041 | 0.0 | 632379 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4350 | 1960 | 2344 | 1512 | 1512 | 0.0041 | 0.0 | 633020 |
| logged-in-admin | desktop | reading | /vi/reading/history | 32 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5883 | 2756 | 3876 | 2376 | 2376 | 0.0041 | 0.0 | 645014 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4092 | 1676 | 2079 | 1124 | 1480 | 0.0020 | 0.0 | 526349 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4018 | 1739 | 2014 | 1148 | 1148 | 0.0020 | 0.0 | 526315 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4368 | 1432 | 2362 | 940 | 1284 | 0.0020 | 0.0 | 526255 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4540 | 1731 | 2534 | 972 | 1636 | 0.0000 | 0.0 | 647658 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5340 | 2059 | 3332 | 1568 | 1568 | 0.0000 | 0.0 | 647618 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3913 | 1599 | 1904 | 1032 | 1032 | 0.0000 | 0.0 | 646822 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4776 | 1791 | 2769 | 1392 | 1392 | 0.0022 | 0.0 | 698692 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4932 | 2090 | 2926 | 1444 | 1444 | 0.0000 | 0.0 | 644825 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4895 | 1900 | 2887 | 1332 | 1332 | 0.0000 | 0.0 | 648184 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5245 | 2639 | 3236 | 1980 | 1980 | 0.0000 | 0.0 | 648920 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4990 | 1987 | 2980 | 1248 | 1248 | 0.0000 | 0.0 | 687635 |
| logged-in-admin | desktop | admin | /vi/admin/users | 31 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4861 | 1806 | 2855 | 1468 | 1468 | 0.0000 | 0.0 | 651389 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4662 | 1630 | 2654 | 1048 | 1048 | 0.0000 | 0.0 | 646177 |
| logged-in-admin | desktop | reading | /vi/reading/session/f29132fb-58b1-4d20-b002-a8e012e93b02 | 33 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4215 | 1916 | 2202 | 936 | 1292 | 0.0041 | 0.0 | 715384 |
| logged-in-admin | desktop | reading | /vi/reading/session/c49d5f84-b4e2-4ee6-9c3c-7164c3ddd014 | 31 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4666 | 2410 | 2655 | 1032 | 1688 | 0.0041 | 0.0 | 712988 |
| logged-in-admin | desktop | reading | /vi/reading/session/67376be5-beb8-41b7-b505-312c52f5bc45 | 33 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4374 | 2156 | 2364 | 992 | 1504 | 0.0041 | 0.0 | 715028 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4840 | 2017 | 2830 | 964 | 1284 | 0.0041 | 0.0 | 633308 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4332 | 1868 | 2324 | 1368 | 1368 | 0.0041 | 0.0 | 631495 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4253 | 1862 | 2247 | 1040 | 1428 | 0.0041 | 0.0 | 631306 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4271 | 1794 | 2227 | 944 | 1300 | 0.0041 | 0.0 | 632787 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4053 | 1730 | 2041 | 1148 | 1148 | 0.0041 | 0.0 | 635368 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4389 | 1664 | 2379 | 976 | 1380 | 0.0041 | 0.0 | 633349 |
| logged-in-reader | desktop | auth-public | /vi | 35 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3099 | 835 | 1091 | 608 | 1176 | 0.0033 | 343.0 | 613249 |
| logged-in-reader | desktop | reading | /vi/reading | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2974 | 940 | 952 | 636 | 1072 | 0.0039 | 0.0 | 642019 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 35 | 5 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3040 | 907 | 1032 | 624 | 1068 | 0.0039 | 0.0 | 652373 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 36 | 6 | critical | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3158 | 849 | 1140 | 588 | 996 | 0.0039 | 0.0 | 733895 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 35 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4955 | 955 | 2943 | 672 | 1128 | 0.0039 | 0.0 | 737617 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 32 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7088 | 752 | 1413 | 612 | 1032 | 0.0040 | 0.0 | 642287 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3094 | 832 | 1078 | 668 | 1052 | 0.0726 | 0.0 | 639184 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2718 | 696 | 710 | 624 | 1012 | 0.0039 | 0.0 | 631926 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2758 | 735 | 748 | 584 | 1376 | 0.0039 | 0.0 | 632725 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3319 | 821 | 1251 | 608 | 1044 | 0.0039 | 0.0 | 634131 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2778 | 755 | 767 | 600 | 1028 | 0.0039 | 0.0 | 631689 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3022 | 719 | 1014 | 632 | 984 | 0.0177 | 0.0 | 649903 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3515 | 757 | 777 | 600 | 1772 | 0.0039 | 0.0 | 643338 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2915 | 898 | 898 | 616 | 1024 | 0.0430 | 0.0 | 642409 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2927 | 914 | 914 | 608 | 1120 | 0.0039 | 0.0 | 635468 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2772 | 750 | 758 | 592 | 968 | 0.0039 | 0.0 | 631948 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 32 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3587 | 726 | 1575 | 600 | 1040 | 0.0039 | 0.0 | 643928 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2839 | 828 | 832 | 572 | 1004 | 0.0095 | 0.0 | 634645 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2805 | 794 | 794 | 604 | 1012 | 0.0040 | 0.0 | 633548 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 34 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6241 | 667 | 4229 | 608 | 988 | 0.0039 | 0.0 | 646288 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2684 | 667 | 674 | 616 | 1176 | 0.0039 | 0.0 | 633118 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3356 | 618 | 1348 | 624 | 916 | 0.0019 | 0.0 | 526516 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2748 | 615 | 739 | 616 | 896 | 0.0019 | 0.0 | 526345 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2682 | 670 | 670 | 640 | 948 | 0.0019 | 0.0 | 526394 |
| logged-in-reader | desktop | reading | /vi/reading/session/d267e51d-acf5-45ec-bb78-4429f9e7cca1 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2798 | 781 | 781 | 608 | 1008 | 0.0052 | 0.0 | 632593 |
| logged-in-reader | desktop | reading | /vi/reading/session/b81fb938-c7b0-434f-81fa-5bb7022a00aa | 31 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3645 | 709 | 1639 | 584 | 988 | 0.0039 | 0.0 | 713024 |
| logged-in-reader | desktop | reading | /vi/reading/session/0a0104da-87a0-421d-a6a1-a071ff54c9ac | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2753 | 728 | 741 | 600 | 964 | 0.0039 | 0.0 | 632588 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3068 | 718 | 1047 | 620 | 960 | 0.0039 | 0.0 | 631745 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2773 | 752 | 765 | 684 | 1028 | 0.0039 | 0.0 | 631751 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2707 | 684 | 697 | 644 | 1020 | 0.0039 | 0.0 | 631569 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2686 | 661 | 675 | 584 | 1168 | 0.0039 | 0.0 | 632973 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2972 | 684 | 961 | 616 | 1164 | 0.0039 | 0.0 | 633228 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2724 | 699 | 711 | 620 | 1136 | 0.0039 | 0.0 | 633518 |
| logged-out | mobile | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5409 | 2860 | 3399 | 2848 | 3192 | 0.0000 | 0.0 | 601287 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3239 | 633 | 1232 | 588 | 588 | 0.0000 | 0.0 | 512441 |
| logged-out | mobile | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2740 | 681 | 730 | 576 | 576 | 0.0000 | 0.0 | 512906 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2703 | 694 | 694 | 552 | 552 | 0.0000 | 0.0 | 511924 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2634 | 626 | 626 | 548 | 548 | 0.0000 | 0.0 | 512040 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2659 | 652 | 652 | 608 | 608 | 0.0000 | 0.0 | 512208 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4049 | 793 | 2036 | 564 | 564 | 0.0000 | 0.0 | 525998 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2973 | 901 | 962 | 576 | 576 | 0.0000 | 0.0 | 526033 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2753 | 744 | 744 | 560 | 560 | 0.0000 | 0.0 | 526108 |
| logged-in-admin | mobile | auth-public | /vi | 35 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3965 | 1581 | 1957 | 1552 | 1612 | 0.0032 | 0.0 | 612999 |
| logged-in-admin | mobile | reading | /vi/reading | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3754 | 1744 | 1744 | 1728 | 2056 | 0.0000 | 0.0 | 641536 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 35 | 5 | high | 0 | 0 | 0 | 0 | 5 | 0 | 5 | 5 | 0 | 0 | 4040 | 1334 | 2030 | 1052 | 1388 | 0.0071 | 0.0 | 661494 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 37 | 4 | critical | 0 | 0 | 1 | 0 | 5 | 0 | 5 | 5 | 0 | 0 | 4487 | 2218 | 2479 | 1244 | 1556 | 0.0071 | 0.0 | 799256 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5633 | 2240 | 3625 | 1316 | 1672 | 0.0071 | 0.0 | 728586 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 32 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7702 | 2206 | 2933 | 1260 | 1260 | 0.0071 | 0.0 | 643298 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 29 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4937 | 2532 | 2924 | 1152 | 1452 | 0.0760 | 0.0 | 634467 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6331 | 3131 | 4321 | 1472 | 1472 | 0.0071 | 0.0 | 632014 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 34 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6171 | 0 | - | 1852 | 1852 | 0.0000 | 0.0 | 630988 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5573 | 2909 | 3564 | 1724 | 1724 | 0.0071 | 0.0 | 634223 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6205 | 3503 | 4197 | 1632 | 1632 | 0.0071 | 0.0 | 631742 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5420 | 2886 | 3410 | 1224 | 1224 | 0.0267 | 0.0 | 649811 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 34 | 6 | high | 0 | 0 | 0 | 0 | 2 | 0 | 2 | 2 | 0 | 0 | 9618 | 5311 | 6827 | 1648 | 5280 | 0.0173 | 0.0 | 776716 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7358 | 4407 | 5347 | 1468 | 1468 | 0.0071 | 0.0 | 642011 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 9090 | 3827 | 7061 | 1648 | 1688 | 0.0071 | 0.0 | 634410 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8114 | 4493 | 6103 | 1696 | 2020 | 0.0071 | 0.0 | 631793 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8698 | 5415 | 6689 | 2524 | 3060 | 0.0071 | 0.0 | 632528 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 32 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8668 | 0 | - | - | - | 0.0000 | 0.0 | 631306 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7924 | 4172 | 5914 | 1828 | 1860 | 0.0088 | 0.0 | 632175 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8531 | 5033 | 6519 | 2812 | 2812 | 0.0071 | 0.0 | 632650 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8006 | 4714 | 5995 | 1728 | 1728 | 0.0071 | 0.0 | 633223 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6232 | 3446 | 4221 | 1300 | 1300 | 0.0055 | 0.0 | 526359 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6530 | 3576 | 4521 | 1576 | 1576 | 0.0055 | 0.0 | 526308 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6394 | 3189 | 4383 | 1488 | 1488 | 0.0055 | 0.0 | 526430 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6115 | 3500 | 4104 | 1428 | 1428 | 0.0000 | 0.0 | 647740 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6802 | 4059 | 4792 | 1532 | 1868 | 0.0000 | 0.0 | 647518 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7565 | 4798 | 5554 | 1600 | 1904 | 0.0000 | 0.0 | 645943 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 31 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7274 | 4697 | 5263 | 1488 | 1488 | 0.0030 | 0.0 | 696953 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6393 | 3840 | 4378 | 1424 | 1732 | 0.0000 | 0.0 | 644679 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6224 | 3745 | 4214 | 1408 | 1408 | 0.0000 | 0.0 | 646798 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6208 | 3626 | 4199 | 1816 | 2136 | 0.0000 | 0.0 | 648917 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8703 | 4206 | 6691 | 2392 | 2720 | 0.0000 | 0.0 | 687649 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | 2 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7728 | 4726 | 5719 | 2276 | 2344 | 0.0000 | 0.0 | 650940 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8283 | 3937 | 6272 | 3948 | 3948 | 0.0000 | 0.0 | 646213 |
| logged-in-admin | mobile | reading | /vi/reading/session/ca321f82-2eba-4e3a-bc78-8609d34c77c5 | 30 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6884 | 3993 | 4872 | 1656 | 1988 | 0.0072 | 0.0 | 681219 |
| logged-in-admin | mobile | reading | /vi/reading/session/1eec4115-cebf-4b42-928f-cb69ac337f40 | 30 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6537 | 3112 | 4521 | 1272 | 1608 | 0.0072 | 0.0 | 681248 |
| logged-in-admin | mobile | reading | /vi/reading/session/07bb0c79-f9da-445b-9b57-05fd1022159a | 30 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6656 | 3560 | 4644 | 1588 | 1912 | 0.0072 | 0.0 | 680957 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6274 | 2902 | 4263 | 1664 | 2000 | 0.0071 | 0.0 | 631084 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5913 | 2865 | 3899 | 1404 | 1404 | 0.0071 | 0.0 | 631435 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5340 | 2861 | 3328 | 1328 | 1328 | 0.0071 | 0.0 | 631326 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6471 | 3968 | 4461 | 1708 | 1740 | 0.0071 | 0.0 | 632846 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5249 | 2221 | 3239 | 1256 | 1256 | 0.0071 | 0.0 | 633090 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6862 | 3926 | 4848 | 2584 | 2936 | 0.0071 | 0.0 | 633079 |
| logged-in-reader | mobile | auth-public | /vi | 35 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3058 | 866 | 1015 | 552 | 904 | 0.0032 | 0.0 | 613205 |
| logged-in-reader | mobile | reading | /vi/reading | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2919 | 886 | 908 | 572 | 900 | 0.0000 | 0.0 | 641914 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 36 | 5 | critical | 0 | 0 | 1 | 0 | 5 | 2 | 1 | 5 | 0 | 0 | 3206 | 1191 | 1191 | 620 | 940 | 0.0071 | 0.0 | 662442 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 38 | 4 | critical | 0 | 0 | 2 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 3462 | 839 | 1450 | 564 | 912 | 0.0071 | 0.0 | 800619 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2991 | 866 | 981 | 572 | 912 | 0.0000 | 0.0 | 725345 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 33 | 28 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5764 | 798 | 949 | 576 | 892 | 0.0000 | 0.0 | 646386 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3115 | 879 | 1102 | 548 | 888 | 0.0000 | 0.0 | 638035 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2811 | 776 | 799 | 600 | 936 | 0.0000 | 0.0 | 631991 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2880 | 865 | 870 | 624 | 1384 | 0.0000 | 0.0 | 632698 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2870 | 855 | 856 | 588 | 924 | 0.0000 | 0.0 | 634160 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2824 | 792 | 811 | 572 | 900 | 0.0000 | 0.0 | 632091 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2946 | 840 | 934 | 716 | 1036 | 0.0196 | 0.0 | 649884 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 37 | 3 | critical | 0 | 0 | 2 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3923 | 847 | 1188 | 608 | 1744 | 0.0051 | 0.0 | 780251 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3240 | 918 | 1227 | 612 | 956 | 0.0071 | 0.0 | 642268 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 57 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3828 | 1031 | 1031 | 896 | 1264 | 0.0000 | 0.0 | 1146284 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2881 | 851 | 866 | 624 | 956 | 0.0000 | 0.0 | 631773 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3450 | 836 | 1440 | 628 | 960 | 0.0071 | 0.0 | 631864 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2906 | 891 | 892 | 620 | 972 | 0.0330 | 0.0 | 633251 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4535 | 801 | 2523 | 640 | 988 | 0.0074 | 0.0 | 632474 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2823 | 784 | 806 | 564 | 900 | 0.0000 | 0.0 | 632685 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2850 | 813 | 836 | 664 | 1088 | 0.0000 | 0.0 | 633189 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2822 | 806 | 809 | 692 | 692 | 0.0000 | 0.0 | 526327 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2745 | 731 | 731 | 716 | 716 | 0.0000 | 0.0 | 526263 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2791 | 777 | 778 | 592 | 908 | 0.0032 | 0.0 | 526411 |
| logged-in-reader | mobile | reading | /vi/reading/session/886d9d2d-7300-42c9-9260-60c59b3255b9 | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2856 | 768 | 843 | 636 | 972 | 0.0000 | 0.0 | 632638 |
| logged-in-reader | mobile | reading | /vi/reading/session/80b5e443-990c-4191-929f-6efbaa05985e | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3033 | 844 | 1017 | 580 | 912 | 0.0000 | 0.0 | 695135 |
| logged-in-reader | mobile | reading | /vi/reading/session/9eb4ef2a-a6bd-4f2a-a880-24f3129f529a | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2899 | 862 | 884 | 624 | 972 | 0.0000 | 0.0 | 632757 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2876 | 798 | 820 | 580 | 920 | 0.0000 | 0.0 | 631344 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2869 | 839 | 852 | 652 | 976 | 0.0000 | 0.0 | 631958 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2838 | 793 | 817 | 572 | 912 | 0.0000 | 0.0 | 631624 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2847 | 833 | 833 | 572 | 980 | 0.0000 | 0.0 | 633273 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2940 | 908 | 928 | 648 | 1064 | 0.0000 | 0.0 | 633338 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3000 | 974 | 974 | 600 | 940 | 0.0000 | 0.0 | 635550 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | desktop | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | mobile | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 35 | high | 0 | 33 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 37 | critical | 3 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 32 | high | 3 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/f29132fb-58b1-4d20-b002-a8e012e93b02 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/c49d5f84-b4e2-4ee6-9c3c-7164c3ddd014 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/67376be5-beb8-41b7-b505-312c52f5bc45 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 35 | high | 5 | 27 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 36 | critical | 1 | 33 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 35 | high | 4 | 29 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 32 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 34 | high | 5 | 27 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/d267e51d-acf5-45ec-bb78-4429f9e7cca1 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/b81fb938-c7b0-434f-81fa-5bb7022a00aa | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/0a0104da-87a0-421d-a6a1-a071ff54c9ac | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 35 | high | 5 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 37 | critical | 2 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 34 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/ca321f82-2eba-4e3a-bc78-8609d34c77c5 | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/1eec4115-cebf-4b42-928f-cb69ac337f40 | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/07bb0c79-f9da-445b-9b57-05fd1022159a | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 35 | high | 5 | 27 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 36 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 38 | critical | 3 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 33 | high | 4 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 37 | critical | 3 | 32 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 57 | critical | 3 | 49 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/886d9d2d-7300-42c9-9260-60c59b3255b9 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/80b5e443-990c-4191-929f-6efbaa05985e | 34 | high | 3 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/9eb4ef2a-a6bd-4f2a-a880-24f3129f529a | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 30 | high | 2 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 6510 | 2878 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 6224 | 900 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 6216 | 355 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 6087 | 2463 | static | https://www.tarotnow.xyz/_next/static/chunks/02u6sgqed9rhs.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 5446 | 354 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 5381 | 365 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 5374 | 402 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 5331 | 832 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 5326 | 525 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 5326 | 778 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 5320 | 344 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 5313 | 575 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 5268 | 355 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 5176 | 345 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 5164 | 411 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 5164 | 762 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 5142 | 366 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 5114 | 904 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 5106 | 346 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 5099 | 336 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 5093 | 233 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 5086 | 742 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 5086 | 363 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 5041 | 371 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 5038 | 367 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 5017 | 394 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 4932 | 376 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 4905 | 383 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 4884 | 322 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 4866 | 392 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 4791 | 216 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 4779 | 373 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 4729 | 347 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 4719 | 374 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 4713 | 400 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 4702 | 604 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 4680 | 251 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 4664 | 919 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 4649 | 329 | static | https://www.tarotnow.xyz/_next/static/chunks/171h5pwft-uqj.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 4624 | 346 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 4624 | 310 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 4586 | 323 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 4561 | 304 | static | https://www.tarotnow.xyz/_next/static/chunks/146-97ymb.004.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 4504 | 354 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 4503 | 403 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 4498 | 359 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 4487 | 382 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 4477 | 243 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 4433 | 1037 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 4432 | 339 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 4410 | 371 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 4401 | 358 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 4388 | 287 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 4363 | 377 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 4351 | 356 | static | https://www.tarotnow.xyz/_next/static/chunks/02u6sgqed9rhs.js |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 4349 | 305 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 4294 | 317 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 4285 | 365 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 4261 | 382 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | reading | /vi/reading/session/ca321f82-2eba-4e3a-bc78-8609d34c77c5 | GET | 200 | 4261 | 264 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 4253 | 368 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | reading | /vi/reading/session/ca321f82-2eba-4e3a-bc78-8609d34c77c5 | GET | 200 | 4252 | 325 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 4246 | 345 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 4232 | 915 | static | https://www.tarotnow.xyz/_next/static/chunks/17ovlvq8jtm.c.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 4220 | 920 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | reading | /vi/reading/session/ca321f82-2eba-4e3a-bc78-8609d34c77c5 | GET | 200 | 4207 | 338 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 4200 | 366 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 4197 | 395 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 4196 | 322 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 4191 | 1056 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | reading | /vi/reading/session/ca321f82-2eba-4e3a-bc78-8609d34c77c5 | GET | 200 | 4191 | 564 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 4188 | 249 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 4179 | 354 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 4173 | 436 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 4142 | 336 | static | https://www.tarotnow.xyz/_next/static/chunks/17ovlvq8jtm.c.js |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 4138 | 393 | static | https://www.tarotnow.xyz/_next/static/chunks/17ovlvq8jtm.c.js |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 4108 | 307 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 4105 | 651 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 4091 | 326 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 4089 | 596 | static | https://www.tarotnow.xyz/_next/static/chunks/02u6sgqed9rhs.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 800 | 46 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 799 | 470 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 799 | 796 | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 799 | 354 | static | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-reader | mobile | reading | /vi/reading/session/9eb4ef2a-a6bd-4f2a-a880-24f3129f529a | GET | 200 | 799 | 314 | html | https://www.tarotnow.xyz/vi/reading/session/9eb4ef2a-a6bd-4f2a-a880-24f3129f529a |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 798 | 785 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 797 | 557 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 797 | 325 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | reading | /vi/reading/session/80b5e443-990c-4191-929f-6efbaa05985e | GET | 200 | 797 | 317 | html | https://www.tarotnow.xyz/vi/reading/session/80b5e443-990c-4191-929f-6efbaa05985e |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 796 | 771 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 795 | 180 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 795 | 328 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 794 | 321 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 793 | 334 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 792 | 164 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 792 | 325 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 792 | 308 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 791 | 321 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 791 | 337 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 788 | 475 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | desktop | reading | /vi/reading/session/f29132fb-58b1-4d20-b002-a8e012e93b02 | GET | 200 | 787 | 187 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 785 | 322 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 785 | 333 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 784 | 574 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | admin | /vi/admin/readings | GET | 200 | 784 | 49 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 784 | 207 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | reading | /vi/reading/session/67376be5-beb8-41b7-b505-312c52f5bc45 | GET | 200 | 783 | 46 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-out | desktop | auth-public | /vi | GET | 200 | 782 | 547 | static | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 777 | 759 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 777 | 756 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 777 | 335 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 777 | 327 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 775 | 332 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 773 | 403 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 773 | 312 | html | https://www.tarotnow.xyz/vi/readers |
| logged-out | desktop | auth-public | /vi | GET | 200 | 772 | 559 | static | https://www.tarotnow.xyz/_next/static/chunks/0u2eu2n2o4n4q.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 772 | 313 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 771 | 366 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 771 | 306 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | reading | /vi/reading/session/ca321f82-2eba-4e3a-bc78-8609d34c77c5 | GET | 200 | 771 | 354 | static | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 770 | 322 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 770 | 311 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 769 | 331 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 769 | 352 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 769 | 316 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 768 | 315 | static | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 768 | 345 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 767 | 380 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 766 | 346 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | desktop | auth-public | /vi | GET | 200 | 765 | 748 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 763 | 745 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 763 | 326 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 762 | 714 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 762 | 721 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | reading | /vi/reading/session/1eec4115-cebf-4b42-928f-cb69ac337f40 | GET | 200 | 762 | 318 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 760 | 343 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 759 | 314 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 759 | 573 | static | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| logged-in-admin | desktop | auth-public | /vi | GET | 200 | 758 | 576 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | admin | /vi/admin/users | GET | 200 | 757 | 371 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 757 | 306 | static | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-out | mobile | auth-public | /vi/legal/privacy | GET | 200 | 756 | 328 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 756 | 749 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 754 | 733 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | reading | /vi/reading/session/1eec4115-cebf-4b42-928f-cb69ac337f40 | GET | 200 | 754 | 310 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | desktop | auth-public | /vi | GET | 200 | 753 | 323 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | auth-public | /vi | GET | 200 | 752 | 618 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 752 | 590 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | auth-public | /vi | GET | 200 | 751 | 646 | static | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | desktop | auth-public | /vi | GET | 200 | 750 | 730 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 749 | 322 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | desktop | auth-public | /vi | GET | 200 | 748 | 302 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | auth-public | /vi | GET | 200 | 748 | 339 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | GET | 200 | 748 | 188 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 748 | 53 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 748 | 329 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 747 | 333 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | GET | 200 | 746 | 681 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 746 | 706 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | GET | 200 | 746 | 340 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-admin | desktop | /vi/inventory | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-admin | desktop | /vi/gacha | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | desktop | /vi/gacha/history | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/v1/reading/cards-catalog/chunks/0?v=81a3d9698977fda2 |
| logged-in-admin | desktop | /vi/profile | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | desktop | /vi/profile/reader | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | desktop | /vi/notifications | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-admin | mobile | /vi/reading | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | /vi/gacha/history | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F07_The_Lovers_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F08_The_Chariot_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F12_Justice_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F13_The_Hanged_Man_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F16_The_Devil_50_20260325_181357.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dfull&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fui-avatars.com%2Fapi%2F%3Fbackground%3D111%26color%3Dfff%26name%3DLucifer&w=384&q=75 |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | mobile | /vi/profile/reader | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | mobile | /vi/leaderboard | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-admin | mobile | /vi/wallet/deposit | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | mobile | /vi/reading/session/ca321f82-2eba-4e3a-bc78-8609d34c77c5 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | /vi/reading/session/1eec4115-cebf-4b42-928f-cb69ac337f40 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/gamification | https://www.tarotnow.xyz/vi/gamification |

## Coverage Notes
| Scenario | Viewport | Note |
| --- | --- | --- |
| logged-out | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-out | desktop | dynamic-routes: skipped for logged-out scenario. |
| logged-out | desktop | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | desktop | reading.init.daily_1: blocked (400). |
| logged-in-admin | desktop | reading.init.spread_3: created f29132fb-58b1-4d20-b002-a8e012e93b02. |
| logged-in-admin | desktop | reading.init.spread_5: created c49d5f84-b4e2-4ee6-9c3c-7164c3ddd014. |
| logged-in-admin | desktop | reading.init.spread_10: created 67376be5-beb8-41b7-b505-312c52f5bc45. |
| logged-in-admin | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | desktop | reader-detail:ui-discovery-empty |
| logged-in-admin | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-admin | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-admin | desktop | community-posts:api-discovery-1 |
| logged-in-admin | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | desktop | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | desktop | reading.init.daily_1: blocked (400). |
| logged-in-reader | desktop | reading.init.spread_3: created d267e51d-acf5-45ec-bb78-4429f9e7cca1. |
| logged-in-reader | desktop | reading.init.spread_5: created b81fb938-c7b0-434f-81fa-5bb7022a00aa. |
| logged-in-reader | desktop | reading.init.spread_10: created 0a0104da-87a0-421d-a6a1-a071ff54c9ac. |
| logged-in-reader | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | desktop | reader-detail:ui-discovery-empty |
| logged-in-reader | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-reader | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-reader | desktop | community-posts:api-discovery-1 |
| logged-in-reader | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | desktop | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |
| logged-out | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-out | mobile | dynamic-routes: skipped for logged-out scenario. |
| logged-out | mobile | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | mobile | reading.init.daily_1: blocked (400). |
| logged-in-admin | mobile | reading.init.spread_3: created ca321f82-2eba-4e3a-bc78-8609d34c77c5. |
| logged-in-admin | mobile | reading.init.spread_5: created 1eec4115-cebf-4b42-928f-cb69ac337f40. |
| logged-in-admin | mobile | reading.init.spread_10: created 07bb0c79-f9da-445b-9b57-05fd1022159a. |
| logged-in-admin | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | mobile | reader-detail:ui-discovery-empty |
| logged-in-admin | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-admin | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-admin | mobile | community-posts:api-discovery-1 |
| logged-in-admin | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | mobile | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | mobile | reading.init.daily_1: blocked (400). |
| logged-in-reader | mobile | reading.init.spread_3: created 886d9d2d-7300-42c9-9260-60c59b3255b9. |
| logged-in-reader | mobile | reading.init.spread_5: created 80b5e443-990c-4191-929f-6efbaa05985e. |
| logged-in-reader | mobile | reading.init.spread_10: created 9eb4ef2a-a6bd-4f2a-a880-24f3129f529a. |
| logged-in-reader | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | mobile | reader-detail:ui-discovery-empty |
| logged-in-reader | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-reader | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-reader | mobile | community-posts:api-discovery-1 |
| logged-in-reader | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | mobile | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |

## Login Bootstrap Notes
### logged-in-admin / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login bootstrap succeeded.

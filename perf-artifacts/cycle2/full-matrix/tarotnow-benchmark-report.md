# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-04-29T20:56:04.704Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 77
- High pages (request count): 182
- High slow requests: 512
- Medium slow requests: 1336

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2935 | 247 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 48 | 3275 | 1841 | 2 | 0 | 11 | 4 | yes |
| logged-in-reader | desktop | 38 | 3487 | 1425 | 14 | 0 | 12 | 6 | yes |
| logged-out | mobile | 9 | 2946 | 247 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 48 | 3439 | 1784 | 0 | 0 | 9 | 5 | yes |
| logged-in-reader | mobile | 38 | 3703 | 1366 | 0 | 0 | 8 | 5 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 35.3 | 3059 | 0 |
| logged-in-admin | desktop | auth | 5 | 38.0 | 3852 | 0 |
| logged-in-admin | desktop | chat | 1 | 38.0 | 3269 | 0 |
| logged-in-admin | desktop | collection | 1 | 105.0 | 4953 | 0 |
| logged-in-admin | desktop | community | 1 | 43.0 | 5185 | 0 |
| logged-in-admin | desktop | gacha | 2 | 38.0 | 3214 | 0 |
| logged-in-admin | desktop | gamification | 1 | 35.0 | 3794 | 0 |
| logged-in-admin | desktop | home | 1 | 37.0 | 3733 | 0 |
| logged-in-admin | desktop | inventory | 1 | 40.0 | 3029 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 39.0 | 3934 | 0 |
| logged-in-admin | desktop | legal | 3 | 31.0 | 2900 | 0 |
| logged-in-admin | desktop | notifications | 1 | 39.0 | 2902 | 0 |
| logged-in-admin | desktop | profile | 3 | 45.0 | 2966 | 0 |
| logged-in-admin | desktop | reader | 1 | 34.0 | 2917 | 0 |
| logged-in-admin | desktop | readers | 7 | 33.4 | 3081 | 0 |
| logged-in-admin | desktop | reading | 5 | 36.4 | 3141 | 0 |
| logged-in-admin | desktop | wallet | 4 | 42.0 | 3085 | 2 |
| logged-in-admin | mobile | admin | 10 | 35.4 | 3639 | 0 |
| logged-in-admin | mobile | auth | 5 | 38.0 | 3922 | 0 |
| logged-in-admin | mobile | chat | 1 | 38.0 | 3710 | 0 |
| logged-in-admin | mobile | collection | 1 | 53.0 | 3643 | 0 |
| logged-in-admin | mobile | community | 1 | 43.0 | 4457 | 0 |
| logged-in-admin | mobile | gacha | 2 | 37.5 | 3217 | 0 |
| logged-in-admin | mobile | gamification | 1 | 35.0 | 3169 | 0 |
| logged-in-admin | mobile | home | 1 | 37.0 | 3425 | 0 |
| logged-in-admin | mobile | inventory | 1 | 40.0 | 3160 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 38.0 | 3561 | 0 |
| logged-in-admin | mobile | legal | 3 | 31.0 | 2959 | 0 |
| logged-in-admin | mobile | notifications | 1 | 39.0 | 3067 | 0 |
| logged-in-admin | mobile | profile | 3 | 45.0 | 3061 | 0 |
| logged-in-admin | mobile | reader | 1 | 34.0 | 3036 | 0 |
| logged-in-admin | mobile | readers | 7 | 33.4 | 3621 | 0 |
| logged-in-admin | mobile | reading | 5 | 35.8 | 3096 | 0 |
| logged-in-admin | mobile | wallet | 4 | 41.8 | 3136 | 0 |
| logged-in-reader | desktop | auth | 5 | 38.0 | 4092 | 0 |
| logged-in-reader | desktop | chat | 1 | 38.0 | 3017 | 0 |
| logged-in-reader | desktop | collection | 1 | 105.0 | 4387 | 14 |
| logged-in-reader | desktop | community | 1 | 43.0 | 4271 | 0 |
| logged-in-reader | desktop | gacha | 2 | 38.0 | 3020 | 0 |
| logged-in-reader | desktop | gamification | 1 | 35.0 | 3442 | 0 |
| logged-in-reader | desktop | home | 1 | 37.0 | 5741 | 0 |
| logged-in-reader | desktop | inventory | 1 | 40.0 | 3089 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 37.0 | 3034 | 0 |
| logged-in-reader | desktop | legal | 3 | 31.0 | 2941 | 0 |
| logged-in-reader | desktop | notifications | 1 | 39.0 | 3184 | 0 |
| logged-in-reader | desktop | profile | 3 | 35.3 | 3241 | 0 |
| logged-in-reader | desktop | reader | 1 | 34.0 | 3281 | 0 |
| logged-in-reader | desktop | readers | 7 | 33.6 | 3525 | 0 |
| logged-in-reader | desktop | reading | 5 | 36.6 | 3381 | 0 |
| logged-in-reader | desktop | wallet | 4 | 33.5 | 3110 | 0 |
| logged-in-reader | mobile | auth | 5 | 38.0 | 3731 | 0 |
| logged-in-reader | mobile | chat | 1 | 38.0 | 3174 | 0 |
| logged-in-reader | mobile | collection | 1 | 53.0 | 4079 | 0 |
| logged-in-reader | mobile | community | 1 | 43.0 | 4545 | 0 |
| logged-in-reader | mobile | gacha | 2 | 37.5 | 3260 | 0 |
| logged-in-reader | mobile | gamification | 1 | 35.0 | 4690 | 0 |
| logged-in-reader | mobile | home | 1 | 37.0 | 3810 | 0 |
| logged-in-reader | mobile | inventory | 1 | 39.0 | 3687 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 37.0 | 3465 | 0 |
| logged-in-reader | mobile | legal | 3 | 31.0 | 3103 | 0 |
| logged-in-reader | mobile | notifications | 1 | 40.0 | 3869 | 0 |
| logged-in-reader | mobile | profile | 3 | 35.3 | 3199 | 0 |
| logged-in-reader | mobile | reader | 1 | 34.0 | 3502 | 0 |
| logged-in-reader | mobile | readers | 7 | 33.6 | 3742 | 0 |
| logged-in-reader | mobile | reading | 5 | 35.4 | 3860 | 0 |
| logged-in-reader | mobile | wallet | 4 | 33.5 | 4081 | 0 |
| logged-out | desktop | auth | 5 | 25.2 | 2860 | 0 |
| logged-out | desktop | home | 1 | 34.0 | 3669 | 0 |
| logged-out | desktop | legal | 3 | 29.0 | 2817 | 0 |
| logged-out | mobile | auth | 5 | 25.2 | 2857 | 0 |
| logged-out | mobile | home | 1 | 34.0 | 3310 | 0 |
| logged-out | mobile | legal | 3 | 29.0 | 2973 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Route | Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | /vi | 34 | high | 0 | 0 | 0 | 0 | 3669 | 1639 | 1639 | 1608 | 2352 | 0.0000 | 32.0 | 556350 |
| logged-out | desktop | /vi/login | 25 | none | 0 | 0 | 0 | 0 | 2864 | 845 | 845 | 884 | 884 | 0.0000 | 0.0 | 432501 |
| logged-out | desktop | /vi/register | 26 | high | 0 | 0 | 0 | 0 | 2867 | 848 | 848 | 880 | 880 | 0.0000 | 0.0 | 433681 |
| logged-out | desktop | /vi/forgot-password | 25 | none | 0 | 0 | 0 | 0 | 2833 | 788 | 788 | 836 | 836 | 0.0000 | 0.0 | 432080 |
| logged-out | desktop | /vi/reset-password | 25 | none | 0 | 0 | 0 | 0 | 2908 | 868 | 869 | 876 | 876 | 0.0000 | 0.0 | 432268 |
| logged-out | desktop | /vi/verify-email | 25 | none | 0 | 0 | 0 | 0 | 2826 | 794 | 794 | 804 | 804 | 0.0000 | 0.0 | 432303 |
| logged-out | desktop | /vi/legal/tos | 29 | high | 0 | 0 | 0 | 0 | 2872 | 823 | 824 | 844 | 844 | 0.0000 | 6.0 | 469056 |
| logged-out | desktop | /vi/legal/privacy | 29 | high | 0 | 0 | 0 | 0 | 2787 | 756 | 756 | 768 | 768 | 0.0000 | 0.0 | 469156 |
| logged-out | desktop | /vi/legal/ai-disclaimer | 29 | high | 0 | 0 | 0 | 0 | 2791 | 765 | 766 | 784 | 784 | 0.0000 | 0.0 | 469244 |
| logged-in-admin | desktop | /vi | 37 | critical | 0 | 0 | 0 | 0 | 3733 | 911 | 1444 | 1600 | 1600 | 0.0030 | 827.0 | 566622 |
| logged-in-admin | desktop | /vi/login | 38 | critical | 0 | 0 | 0 | 0 | 3788 | 1235 | 1749 | 1936 | 1936 | 0.0024 | 261.0 | 566609 |
| logged-in-admin | desktop | /vi/register | 38 | critical | 0 | 0 | 0 | 0 | 3762 | 1161 | 1702 | 1856 | 1856 | 0.0024 | 786.0 | 566574 |
| logged-in-admin | desktop | /vi/forgot-password | 38 | critical | 0 | 0 | 0 | 0 | 3854 | 1212 | 1763 | 1944 | 1944 | 0.0024 | 811.0 | 566640 |
| logged-in-admin | desktop | /vi/reset-password | 38 | critical | 0 | 0 | 0 | 0 | 3837 | 1262 | 1806 | 1992 | 1992 | 0.0024 | 826.0 | 566659 |
| logged-in-admin | desktop | /vi/verify-email | 38 | critical | 0 | 0 | 0 | 0 | 4020 | 1388 | 1980 | 2160 | 2160 | 0.0024 | 893.0 | 566896 |
| logged-in-admin | desktop | /vi/reading | 34 | high | 0 | 0 | 1 | 0 | 2957 | 871 | 871 | 940 | 1440 | 0.0029 | 2.0 | 590442 |
| logged-in-admin | desktop | /vi/inventory | 40 | critical | 0 | 0 | 0 | 0 | 3029 | 843 | 865 | 1144 | 1144 | 0.0029 | 0.0 | 599685 |
| logged-in-admin | desktop | /vi/gacha | 41 | critical | 0 | 0 | 1 | 0 | 3392 | 1006 | 1006 | 1396 | 1396 | 0.0029 | 137.0 | 1010417 |
| logged-in-admin | desktop | /vi/gacha/history | 35 | high | 0 | 0 | 0 | 0 | 3035 | 926 | 963 | 1296 | 1600 | 0.0029 | 49.0 | 674004 |
| logged-in-admin | desktop | /vi/collection | 105 | critical | 0 | 0 | 1 | 0 | 4953 | 912 | 2859 | 880 | 2232 | 0.0029 | 551.0 | 12619035 |
| logged-in-admin | desktop | /vi/profile | 34 | high | 0 | 0 | 0 | 0 | 3045 | 845 | 1015 | 1164 | 1164 | 0.0477 | 10.0 | 583713 |
| logged-in-admin | desktop | /vi/profile/mfa | 33 | high | 0 | 0 | 0 | 0 | 2872 | 789 | 807 | 1076 | 1076 | 0.0029 | 24.0 | 579095 |
| logged-in-admin | desktop | /vi/profile/reader | 68 | critical | 0 | 0 | 0 | 2 | 2980 | 860 | 1080 | 1100 | 1100 | 0.0477 | 11.0 | 1161196 |
| logged-in-admin | desktop | /vi/readers | 35 | high | 0 | 0 | 0 | 0 | 2970 | 847 | 867 | 1156 | 1156 | 0.0029 | 10.0 | 584631 |
| logged-in-admin | desktop | /vi/chat | 38 | critical | 0 | 0 | 1 | 0 | 3269 | 868 | 1001 | 1228 | 1228 | 0.0029 | 10.0 | 591923 |
| logged-in-admin | desktop | /vi/leaderboard | 39 | critical | 0 | 0 | 1 | 0 | 3934 | 859 | 881 | 1180 | 1180 | 0.0167 | 78.0 | 672416 |
| logged-in-admin | desktop | /vi/community | 43 | critical | 0 | 0 | 1 | 1 | 5185 | 1194 | 1839 | 1536 | 2868 | 0.0029 | 120.0 | 735311 |
| logged-in-admin | desktop | /vi/gamification | 35 | high | 0 | 0 | 0 | 0 | 3794 | 1219 | 1225 | 1248 | 1632 | 0.0180 | 6.0 | 592563 |
| logged-in-admin | desktop | /vi/wallet | 33 | high | 0 | 0 | 0 | 0 | 3121 | 980 | 1005 | 1264 | 1620 | 0.0029 | 26.0 | 581464 |
| logged-in-admin | desktop | /vi/wallet/deposit | 34 | high | 0 | 0 | 1 | 0 | 3279 | 1017 | 1046 | 1360 | 1360 | 0.0029 | 60.0 | 579935 |
| logged-in-admin | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 0 | 0 | 0 | 2965 | 831 | 858 | 1112 | 1468 | 0.0029 | 17.0 | 579805 |
| logged-in-admin | desktop | /vi/wallet/withdraw | 68 | critical | 0 | 0 | 1 | 1 | 2975 | 1327 | 1447 | 1728 | 1728 | 0.0029 | 25.0 | 1158914 |
| logged-in-admin | desktop | /vi/notifications | 39 | critical | 0 | 0 | 1 | 0 | 2902 | 832 | 850 | 1164 | 1164 | 0.0029 | 4.0 | 595632 |
| logged-in-admin | desktop | /vi/reader/apply | 34 | high | 0 | 0 | 0 | 0 | 2917 | 805 | 824 | 1056 | 1056 | 0.0029 | 41.0 | 581432 |
| logged-in-admin | desktop | /vi/reading/history | 33 | high | 0 | 0 | 0 | 0 | 3019 | 900 | 919 | 1212 | 1416 | 0.0029 | 31.0 | 580167 |
| logged-in-admin | desktop | /vi/legal/tos | 31 | high | 0 | 0 | 0 | 0 | 2900 | 831 | 831 | 944 | 944 | 0.0012 | 0.0 | 472227 |
| logged-in-admin | desktop | /vi/legal/privacy | 31 | high | 0 | 0 | 0 | 0 | 2886 | 782 | 783 | 912 | 912 | 0.0012 | 0.0 | 472494 |
| logged-in-admin | desktop | /vi/legal/ai-disclaimer | 31 | high | 0 | 0 | 0 | 0 | 2913 | 837 | 837 | 916 | 916 | 0.0012 | 0.0 | 472427 |
| logged-in-admin | desktop | /vi/admin | 35 | high | 0 | 0 | 0 | 0 | 3050 | 895 | 918 | 1108 | 1108 | 0.0000 | 17.0 | 597424 |
| logged-in-admin | desktop | /vi/admin/deposits | 35 | high | 0 | 0 | 0 | 0 | 2953 | 789 | 812 | 972 | 1128 | 0.0000 | 0.0 | 597389 |
| logged-in-admin | desktop | /vi/admin/disputes | 36 | critical | 0 | 0 | 0 | 0 | 3295 | 1036 | 1064 | 1240 | 1240 | 0.0000 | 3.0 | 597268 |
| logged-in-admin | desktop | /vi/admin/gamification | 37 | critical | 0 | 0 | 0 | 0 | 3002 | 880 | 942 | 968 | 968 | 0.0022 | 4.0 | 639667 |
| logged-in-admin | desktop | /vi/admin/promotions | 34 | high | 0 | 0 | 0 | 0 | 2953 | 828 | 861 | 1032 | 1032 | 0.0000 | 0.0 | 594662 |
| logged-in-admin | desktop | /vi/admin/reader-requests | 35 | high | 0 | 0 | 0 | 0 | 3084 | 951 | 951 | 1096 | 1096 | 0.0000 | 9.0 | 596476 |
| logged-in-admin | desktop | /vi/admin/readings | 35 | high | 0 | 0 | 0 | 0 | 3036 | 883 | 899 | 1052 | 1052 | 0.0000 | 21.0 | 598870 |
| logged-in-admin | desktop | /vi/admin/system-configs | 35 | high | 0 | 0 | 0 | 0 | 3080 | 845 | 1017 | 1052 | 1296 | 0.0000 | 20.0 | 636583 |
| logged-in-admin | desktop | /vi/admin/users | 36 | critical | 0 | 0 | 0 | 0 | 3255 | 953 | 953 | 1072 | 1624 | 0.0000 | 26.0 | 599747 |
| logged-in-admin | desktop | /vi/admin/withdrawals | 35 | high | 0 | 0 | 0 | 0 | 2882 | 840 | 840 | 988 | 988 | 0.0000 | 7.0 | 596084 |
| logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 33 | high | 0 | 0 | 0 | 0 | 3305 | 1033 | 1053 | 1248 | 1484 | 0.0029 | 30.0 | 580306 |
| logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 34 | high | 0 | 0 | 1 | 0 | 3260 | 982 | 982 | 1384 | 1692 | 0.0029 | 103.0 | 581450 |
| logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 33 | high | 0 | 0 | 0 | 0 | 3066 | 877 | 909 | 1240 | 1552 | 0.0029 | 56.0 | 580110 |
| logged-in-admin | desktop | /vi/reading/session/e133c6d0-0bd3-4695-8a84-92717ae0582f | 38 | critical | 0 | 0 | 0 | 0 | 3070 | 923 | 1037 | 1192 | 1192 | 0.0029 | 31.0 | 671892 |
| logged-in-admin | desktop | /vi/reading/session/6d346279-a2ca-4c46-b7c5-925a1cee1557 | 39 | critical | 0 | 0 | 1 | 0 | 3523 | 904 | 1255 | 1300 | 1300 | 0.0039 | 83.0 | 673101 |
| logged-in-admin | desktop | /vi/reading/session/99cbb8c0-4747-40ab-8d37-34959c33d792 | 38 | critical | 0 | 0 | 0 | 0 | 3138 | 850 | 1020 | 1152 | 1152 | 0.0029 | 20.0 | 672079 |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 0 | 0 | 0 | 3052 | 882 | 882 | 1212 | 1212 | 0.0029 | 18.0 | 578534 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 2975 | 880 | 880 | 1140 | 1140 | 0.0029 | 2.0 | 578468 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 0 | 0 | 0 | 2942 | 804 | 848 | 1040 | 1040 | 0.0029 | 6.0 | 578614 |
| logged-in-reader | desktop | /vi | 37 | critical | 0 | 0 | 0 | 0 | 5741 | 1102 | 1999 | 2032 | 2032 | 0.0022 | 1091.0 | 566484 |
| logged-in-reader | desktop | /vi/login | 38 | critical | 0 | 0 | 0 | 1 | 4364 | 1447 | 2134 | 2324 | 2324 | 0.0022 | 993.0 | 565348 |
| logged-in-reader | desktop | /vi/register | 38 | critical | 0 | 0 | 0 | 0 | 4143 | 1437 | 1996 | 2216 | 2216 | 0.0022 | 461.0 | 566461 |
| logged-in-reader | desktop | /vi/forgot-password | 38 | critical | 0 | 0 | 0 | 0 | 4250 | 1322 | 2120 | 2180 | 2180 | 0.0022 | 1590.0 | 566790 |
| logged-in-reader | desktop | /vi/reset-password | 38 | critical | 0 | 0 | 0 | 0 | 3834 | 1194 | 1797 | 2008 | 2008 | 0.0022 | 1079.0 | 566595 |
| logged-in-reader | desktop | /vi/verify-email | 38 | critical | 0 | 0 | 0 | 0 | 3869 | 1282 | 1836 | 2028 | 2028 | 0.0022 | 272.0 | 566386 |
| logged-in-reader | desktop | /vi/reading | 34 | high | 0 | 0 | 1 | 0 | 2969 | 933 | 948 | 916 | 1544 | 0.0026 | 1.0 | 590086 |
| logged-in-reader | desktop | /vi/inventory | 40 | critical | 0 | 0 | 1 | 0 | 3089 | 790 | 813 | 1136 | 1136 | 0.0026 | 103.0 | 598802 |
| logged-in-reader | desktop | /vi/gacha | 41 | critical | 0 | 0 | 1 | 0 | 3045 | 984 | 984 | 1064 | 1328 | 0.0026 | 53.0 | 1010472 |
| logged-in-reader | desktop | /vi/gacha/history | 35 | high | 0 | 0 | 0 | 0 | 2995 | 856 | 882 | 1180 | 1472 | 0.0026 | 16.0 | 671979 |
| logged-in-reader | desktop | /vi/collection | 105 | critical | 0 | 0 | 1 | 0 | 4387 | 1012 | 1509 | 976 | 1936 | 0.0026 | 66.0 | 10514157 |
| logged-in-reader | desktop | /vi/profile | 38 | critical | 0 | 0 | 1 | 1 | 3068 | 961 | 983 | 1448 | 1448 | 0.0731 | 155.0 | 587333 |
| logged-in-reader | desktop | /vi/profile/mfa | 33 | high | 0 | 0 | 0 | 0 | 3669 | 1006 | 1006 | 1332 | 1332 | 0.0026 | 22.0 | 579377 |
| logged-in-reader | desktop | /vi/profile/reader | 35 | high | 0 | 0 | 0 | 1 | 2987 | 926 | 926 | 1240 | 1864 | 0.0026 | 38.0 | 581392 |
| logged-in-reader | desktop | /vi/readers | 35 | high | 0 | 0 | 0 | 0 | 2931 | 834 | 853 | 1136 | 1136 | 0.0026 | 0.0 | 584657 |
| logged-in-reader | desktop | /vi/chat | 38 | critical | 0 | 0 | 1 | 0 | 3017 | 938 | 938 | 1276 | 1276 | 0.0026 | 30.0 | 592094 |
| logged-in-reader | desktop | /vi/leaderboard | 37 | critical | 0 | 0 | 0 | 0 | 3034 | 817 | 832 | 1072 | 1072 | 0.0164 | 41.0 | 670077 |
| logged-in-reader | desktop | /vi/community | 43 | critical | 0 | 0 | 1 | 1 | 4271 | 948 | 1122 | 1232 | 2032 | 0.0026 | 48.0 | 735515 |
| logged-in-reader | desktop | /vi/gamification | 35 | high | 0 | 0 | 0 | 0 | 3442 | 989 | 989 | 1384 | 1384 | 0.0177 | 3.0 | 593045 |
| logged-in-reader | desktop | /vi/wallet | 33 | high | 0 | 0 | 0 | 0 | 3149 | 944 | 961 | 1204 | 1568 | 0.0026 | 15.0 | 581819 |
| logged-in-reader | desktop | /vi/wallet/deposit | 33 | high | 0 | 0 | 0 | 0 | 3048 | 951 | 951 | 1336 | 1336 | 0.0026 | 64.0 | 579209 |
| logged-in-reader | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 0 | 0 | 0 | 2973 | 831 | 844 | 1036 | 1036 | 0.0026 | 2.0 | 579514 |
| logged-in-reader | desktop | /vi/wallet/withdraw | 35 | high | 0 | 0 | 0 | 1 | 3268 | 872 | 918 | 1188 | 1188 | 0.0026 | 18.0 | 581901 |
| logged-in-reader | desktop | /vi/notifications | 39 | critical | 0 | 0 | 1 | 0 | 3184 | 887 | 1013 | 1296 | 1296 | 0.0026 | 30.0 | 595621 |
| logged-in-reader | desktop | /vi/reader/apply | 34 | high | 0 | 0 | 0 | 0 | 3281 | 936 | 959 | 1268 | 1268 | 0.0026 | 21.0 | 581320 |
| logged-in-reader | desktop | /vi/reading/history | 34 | high | 0 | 0 | 1 | 0 | 3392 | 978 | 1008 | 1316 | 1696 | 0.0026 | 74.0 | 581574 |
| logged-in-reader | desktop | /vi/legal/tos | 31 | high | 0 | 0 | 0 | 0 | 2976 | 819 | 819 | 916 | 916 | 0.0010 | 6.0 | 472417 |
| logged-in-reader | desktop | /vi/legal/privacy | 31 | high | 0 | 0 | 0 | 0 | 2921 | 804 | 804 | 896 | 896 | 0.0010 | 1.0 | 472182 |
| logged-in-reader | desktop | /vi/legal/ai-disclaimer | 31 | high | 0 | 0 | 0 | 1 | 2927 | 796 | 796 | 908 | 908 | 0.0010 | 0.0 | 471216 |
| logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 34 | high | 0 | 0 | 1 | 0 | 3133 | 953 | 953 | 1344 | 1620 | 0.0026 | 82.0 | 581739 |
| logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 33 | high | 0 | 0 | 0 | 0 | 3089 | 842 | 873 | 1152 | 1480 | 0.0026 | 21.0 | 580831 |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 34 | high | 0 | 0 | 1 | 0 | 3245 | 901 | 947 | 1348 | 1556 | 0.0026 | 59.0 | 581162 |
| logged-in-reader | desktop | /vi/reading/session/1c1ba523-21bc-420d-be45-2437f5d266bc | 39 | critical | 0 | 0 | 1 | 0 | 3333 | 1009 | 1237 | 1272 | 1272 | 0.0036 | 69.0 | 673221 |
| logged-in-reader | desktop | /vi/reading/session/3bbbde1f-26dc-4d7e-8436-b4095381e746 | 38 | critical | 0 | 0 | 0 | 0 | 3112 | 840 | 1005 | 1140 | 1140 | 0.0026 | 44.0 | 672158 |
| logged-in-reader | desktop | /vi/reading/session/d63a9d0a-2fd1-4b06-8f4b-e1d4b3960d8a | 38 | critical | 0 | 0 | 0 | 0 | 4099 | 1249 | 1943 | 1356 | 3964 | 0.0026 | 35.0 | 672444 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 0 | 0 | 0 | 3758 | 1292 | 1312 | 1568 | 1568 | 0.0026 | 51.0 | 578752 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 5248 | 2265 | 2291 | 1284 | 1284 | 0.0026 | 18.0 | 578991 |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 0 | 0 | 0 | 3273 | 995 | 1058 | 1072 | 1072 | 0.0026 | 13.0 | 578929 |
| logged-out | mobile | /vi | 34 | high | 0 | 0 | 0 | 0 | 3310 | 1071 | 1071 | 1032 | 1468 | 0.0000 | 52.0 | 567760 |
| logged-out | mobile | /vi/login | 25 | none | 0 | 0 | 0 | 0 | 2893 | 858 | 858 | 848 | 848 | 0.0000 | 0.0 | 432389 |
| logged-out | mobile | /vi/register | 26 | high | 0 | 0 | 0 | 0 | 2914 | 846 | 846 | 868 | 868 | 0.0000 | 0.0 | 433675 |
| logged-out | mobile | /vi/forgot-password | 25 | none | 0 | 0 | 0 | 0 | 2786 | 735 | 735 | 760 | 760 | 0.0000 | 0.0 | 432017 |
| logged-out | mobile | /vi/reset-password | 25 | none | 0 | 0 | 0 | 0 | 2816 | 762 | 762 | 752 | 752 | 0.0000 | 0.0 | 432196 |
| logged-out | mobile | /vi/verify-email | 25 | none | 0 | 0 | 0 | 0 | 2877 | 800 | 800 | 788 | 788 | 0.0000 | 0.0 | 432384 |
| logged-out | mobile | /vi/legal/tos | 29 | high | 0 | 0 | 0 | 0 | 3017 | 793 | 794 | 792 | 792 | 0.0000 | 0.0 | 469232 |
| logged-out | mobile | /vi/legal/privacy | 29 | high | 0 | 0 | 0 | 0 | 2959 | 809 | 809 | 804 | 804 | 0.0000 | 0.0 | 469190 |
| logged-out | mobile | /vi/legal/ai-disclaimer | 29 | high | 0 | 0 | 0 | 0 | 2942 | 878 | 878 | 884 | 884 | 0.0000 | 0.0 | 469330 |
| logged-in-admin | mobile | /vi | 37 | critical | 0 | 0 | 0 | 0 | 3425 | 898 | 1274 | 1068 | 1068 | 0.0025 | 73.0 | 575741 |
| logged-in-admin | mobile | /vi/login | 38 | critical | 0 | 0 | 0 | 1 | 3822 | 1341 | 1566 | 1460 | 1460 | 0.0025 | 66.0 | 574295 |
| logged-in-admin | mobile | /vi/register | 38 | critical | 0 | 0 | 0 | 0 | 3634 | 1297 | 1466 | 1384 | 1384 | 0.0025 | 33.0 | 575652 |
| logged-in-admin | mobile | /vi/forgot-password | 38 | critical | 0 | 0 | 0 | 0 | 3783 | 1273 | 1273 | 1248 | 1684 | 0.0025 | 47.0 | 575604 |
| logged-in-admin | mobile | /vi/reset-password | 38 | critical | 0 | 0 | 0 | 0 | 4724 | 2169 | 2360 | 1624 | 1624 | 0.0031 | 57.0 | 575580 |
| logged-in-admin | mobile | /vi/verify-email | 38 | critical | 0 | 0 | 0 | 0 | 3648 | 1255 | 1400 | 1320 | 1320 | 0.0025 | 31.0 | 575479 |
| logged-in-admin | mobile | /vi/reading | 34 | high | 0 | 0 | 1 | 0 | 3054 | 971 | 972 | 824 | 1176 | 0.0000 | 7.0 | 591093 |
| logged-in-admin | mobile | /vi/inventory | 40 | critical | 0 | 0 | 0 | 0 | 3160 | 928 | 928 | 988 | 988 | 0.0000 | 4.0 | 609898 |
| logged-in-admin | mobile | /vi/gacha | 40 | critical | 0 | 0 | 0 | 0 | 3138 | 925 | 925 | 848 | 1224 | 0.0000 | 3.0 | 1010576 |
| logged-in-admin | mobile | /vi/gacha/history | 35 | high | 0 | 0 | 0 | 0 | 3296 | 1017 | 1042 | 1080 | 1080 | 0.0000 | 13.0 | 675222 |
| logged-in-admin | mobile | /vi/collection | 53 | critical | 0 | 0 | 1 | 0 | 3643 | 1009 | 1555 | 780 | 1420 | 0.0000 | 65.0 | 4096097 |
| logged-in-admin | mobile | /vi/profile | 34 | high | 0 | 0 | 0 | 0 | 3133 | 898 | 1091 | 972 | 972 | 0.0000 | 7.0 | 594747 |
| logged-in-admin | mobile | /vi/profile/mfa | 33 | high | 0 | 0 | 0 | 0 | 3013 | 910 | 943 | 888 | 888 | 0.0000 | 4.0 | 580033 |
| logged-in-admin | mobile | /vi/profile/reader | 68 | critical | 0 | 0 | 0 | 1 | 3037 | 767 | 990 | 856 | 1076 | 0.0719 | 0.0 | 1173930 |
| logged-in-admin | mobile | /vi/readers | 36 | critical | 0 | 0 | 1 | 0 | 3138 | 820 | 866 | 776 | 1196 | 0.0000 | 4.0 | 591549 |
| logged-in-admin | mobile | /vi/chat | 38 | critical | 0 | 0 | 1 | 0 | 3710 | 1072 | 1199 | 1188 | 1188 | 0.0000 | 14.0 | 592820 |
| logged-in-admin | mobile | /vi/leaderboard | 38 | critical | 0 | 0 | 0 | 0 | 3561 | 1047 | 1066 | 1088 | 1088 | 0.0196 | 9.0 | 678193 |
| logged-in-admin | mobile | /vi/community | 43 | critical | 0 | 0 | 1 | 1 | 4457 | 933 | 1220 | 932 | 2120 | 0.0051 | 8.0 | 736099 |
| logged-in-admin | mobile | /vi/gamification | 35 | high | 0 | 0 | 0 | 0 | 3169 | 1003 | 1003 | 896 | 1264 | 0.0000 | 1.0 | 593805 |
| logged-in-admin | mobile | /vi/wallet | 33 | high | 0 | 0 | 0 | 0 | 3305 | 983 | 1020 | 1040 | 1040 | 0.0000 | 21.0 | 582168 |
| logged-in-admin | mobile | /vi/wallet/deposit | 34 | high | 0 | 0 | 1 | 0 | 3032 | 862 | 913 | 824 | 1252 | 0.0000 | 0.0 | 580947 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | 33 | high | 0 | 0 | 0 | 0 | 3299 | 1122 | 1140 | 1188 | 1188 | 0.0000 | 6.0 | 580956 |
| logged-in-admin | mobile | /vi/wallet/withdraw | 67 | critical | 0 | 0 | 0 | 2 | 2909 | 806 | 809 | 936 | 1044 | 0.0030 | 0.0 | 1159777 |
| logged-in-admin | mobile | /vi/notifications | 39 | critical | 0 | 0 | 1 | 0 | 3067 | 856 | 914 | 944 | 944 | 0.0000 | 3.0 | 596440 |
| logged-in-admin | mobile | /vi/reader/apply | 34 | high | 0 | 0 | 0 | 0 | 3036 | 882 | 918 | 928 | 928 | 0.0000 | 1.0 | 582177 |
| logged-in-admin | mobile | /vi/reading/history | 33 | high | 0 | 0 | 0 | 0 | 2992 | 877 | 902 | 880 | 880 | 0.0000 | 16.0 | 581230 |
| logged-in-admin | mobile | /vi/legal/tos | 31 | high | 0 | 0 | 0 | 0 | 3023 | 866 | 866 | 888 | 888 | 0.0025 | 0.0 | 473416 |
| logged-in-admin | mobile | /vi/legal/privacy | 31 | high | 0 | 0 | 0 | 0 | 2918 | 825 | 825 | 788 | 1124 | 0.0025 | 0.0 | 473558 |
| logged-in-admin | mobile | /vi/legal/ai-disclaimer | 31 | high | 0 | 0 | 0 | 0 | 2936 | 842 | 842 | 876 | 876 | 0.0025 | 0.0 | 473394 |
| logged-in-admin | mobile | /vi/admin | 35 | high | 0 | 0 | 0 | 0 | 3494 | 986 | 1017 | 1024 | 1024 | 0.0000 | 10.0 | 597588 |
| logged-in-admin | mobile | /vi/admin/deposits | 35 | high | 0 | 0 | 0 | 0 | 4912 | 2244 | 2258 | 1472 | 1472 | 0.0000 | 26.0 | 597720 |
| logged-in-admin | mobile | /vi/admin/disputes | 36 | critical | 0 | 0 | 0 | 0 | 4708 | 1616 | 1900 | 1108 | 1108 | 0.0000 | 4.0 | 597421 |
| logged-in-admin | mobile | /vi/admin/gamification | 37 | critical | 0 | 0 | 0 | 0 | 3780 | 1243 | 1643 | 1276 | 1276 | 0.0000 | 8.0 | 639561 |
| logged-in-admin | mobile | /vi/admin/promotions | 35 | high | 0 | 0 | 1 | 0 | 3055 | 947 | 961 | 904 | 1268 | 0.0000 | 0.0 | 595452 |
| logged-in-admin | mobile | /vi/admin/reader-requests | 35 | high | 0 | 0 | 0 | 0 | 3154 | 840 | 862 | 828 | 828 | 0.0000 | 9.0 | 596415 |
| logged-in-admin | mobile | /vi/admin/readings | 35 | high | 0 | 0 | 0 | 0 | 3114 | 962 | 1003 | 976 | 976 | 0.0000 | 5.0 | 598749 |
| logged-in-admin | mobile | /vi/admin/system-configs | 35 | high | 0 | 0 | 0 | 0 | 3297 | 915 | 1127 | 968 | 968 | 0.0000 | 13.0 | 636775 |
| logged-in-admin | mobile | /vi/admin/users | 36 | critical | 0 | 0 | 0 | 0 | 3497 | 919 | 921 | 848 | 1236 | 0.0000 | 8.0 | 599903 |
| logged-in-admin | mobile | /vi/admin/withdrawals | 35 | high | 0 | 0 | 0 | 0 | 3375 | 1098 | 1208 | 1184 | 1184 | 0.0000 | 16.0 | 596151 |
| logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 33 | high | 0 | 0 | 0 | 0 | 3398 | 948 | 1033 | 1104 | 1104 | 0.0000 | 13.0 | 581377 |
| logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 33 | high | 0 | 0 | 0 | 0 | 3253 | 945 | 974 | 1008 | 1008 | 0.0000 | 5.0 | 581413 |
| logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 33 | high | 0 | 0 | 0 | 0 | 3368 | 1048 | 1069 | 1020 | 1020 | 0.0000 | 12.0 | 581208 |
| logged-in-admin | mobile | /vi/reading/session/43116b7f-80b3-47ff-bb27-8a18d1fdd778 | 37 | critical | 0 | 0 | 0 | 0 | 3176 | 913 | 1088 | 1004 | 1004 | 0.0000 | 15.0 | 641080 |
| logged-in-admin | mobile | /vi/reading/session/62007c5e-90c4-4096-b410-ae1a30379000 | 37 | critical | 0 | 0 | 0 | 0 | 3093 | 823 | 985 | 872 | 872 | 0.0000 | 3.0 | 641316 |
| logged-in-admin | mobile | /vi/reading/session/8f8c12fc-54f8-4cd0-ae23-9696736073ec | 38 | critical | 0 | 0 | 1 | 0 | 3166 | 814 | 843 | 740 | 1132 | 0.0000 | 3.0 | 642237 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 0 | 0 | 0 | 4290 | 1643 | 1696 | 932 | 932 | 0.0030 | 2.0 | 580010 |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 3630 | 1007 | 1066 | 1032 | 1032 | 0.0000 | 18.0 | 579772 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 0 | 0 | 0 | 4272 | 1755 | 1802 | 1416 | 1416 | 0.0030 | 9.0 | 579474 |
| logged-in-reader | mobile | /vi | 37 | critical | 0 | 0 | 0 | 0 | 3810 | 1038 | 1428 | 1144 | 1144 | 0.0020 | 66.0 | 575102 |
| logged-in-reader | mobile | /vi/login | 38 | critical | 0 | 0 | 0 | 0 | 4132 | 1270 | 1457 | 1328 | 1328 | 0.0020 | 43.0 | 575123 |
| logged-in-reader | mobile | /vi/register | 38 | critical | 0 | 0 | 0 | 0 | 3728 | 1350 | 1517 | 1444 | 1444 | 0.0020 | 29.0 | 574937 |
| logged-in-reader | mobile | /vi/forgot-password | 38 | critical | 0 | 0 | 0 | 0 | 3267 | 1110 | 1110 | 1072 | 1596 | 0.0020 | 43.0 | 574977 |
| logged-in-reader | mobile | /vi/reset-password | 38 | critical | 0 | 0 | 0 | 0 | 3626 | 1363 | 1518 | 1432 | 1432 | 0.0020 | 23.0 | 575051 |
| logged-in-reader | mobile | /vi/verify-email | 38 | critical | 0 | 0 | 0 | 0 | 3901 | 1380 | 1775 | 1476 | 1476 | 0.0020 | 37.0 | 575107 |
| logged-in-reader | mobile | /vi/reading | 33 | high | 0 | 0 | 0 | 0 | 3187 | 1044 | 1045 | 1024 | 1024 | 0.0000 | 3.0 | 589998 |
| logged-in-reader | mobile | /vi/inventory | 39 | critical | 0 | 0 | 0 | 0 | 3687 | 993 | 1074 | 1112 | 1112 | 0.0000 | 0.0 | 606590 |
| logged-in-reader | mobile | /vi/gacha | 40 | critical | 0 | 0 | 0 | 0 | 3247 | 1033 | 1034 | 832 | 996 | 0.0000 | 22.0 | 1010353 |
| logged-in-reader | mobile | /vi/gacha/history | 35 | high | 0 | 0 | 0 | 0 | 3273 | 935 | 988 | 1008 | 1008 | 0.0000 | 0.0 | 672764 |
| logged-in-reader | mobile | /vi/collection | 53 | critical | 0 | 0 | 1 | 0 | 4079 | 1209 | 1907 | 876 | 1912 | 0.0000 | 55.0 | 4094656 |
| logged-in-reader | mobile | /vi/profile | 38 | critical | 0 | 0 | 1 | 1 | 3182 | 945 | 1051 | 908 | 1296 | 0.0821 | 10.0 | 595685 |
| logged-in-reader | mobile | /vi/profile/mfa | 33 | high | 0 | 0 | 0 | 0 | 3148 | 880 | 938 | 960 | 960 | 0.0000 | 0.0 | 580145 |
| logged-in-reader | mobile | /vi/profile/reader | 35 | high | 0 | 0 | 0 | 1 | 3268 | 874 | 894 | 904 | 904 | 0.0000 | 2.0 | 581981 |
| logged-in-reader | mobile | /vi/readers | 35 | high | 0 | 0 | 0 | 0 | 4489 | 1930 | 1978 | 1100 | 1100 | 0.0029 | 25.0 | 590482 |
| logged-in-reader | mobile | /vi/chat | 38 | critical | 0 | 0 | 1 | 0 | 3174 | 863 | 881 | 880 | 880 | 0.0000 | 5.0 | 592918 |
| logged-in-reader | mobile | /vi/leaderboard | 37 | critical | 0 | 0 | 0 | 0 | 3465 | 904 | 963 | 984 | 984 | 0.0196 | 0.0 | 670887 |
| logged-in-reader | mobile | /vi/community | 43 | critical | 0 | 0 | 1 | 1 | 4545 | 953 | 1277 | 1008 | 2124 | 0.0051 | 13.0 | 736042 |
| logged-in-reader | mobile | /vi/gamification | 35 | high | 0 | 0 | 0 | 0 | 4690 | 2007 | 2007 | 1448 | 1448 | 0.0029 | 40.0 | 593666 |
| logged-in-reader | mobile | /vi/wallet | 33 | high | 0 | 0 | 0 | 0 | 3687 | 1281 | 1281 | 1128 | 1128 | 0.0000 | 34.0 | 582612 |
| logged-in-reader | mobile | /vi/wallet/deposit | 33 | high | 0 | 0 | 0 | 0 | 3877 | 1226 | 1247 | 1212 | 1212 | 0.0000 | 40.0 | 580214 |
| logged-in-reader | mobile | /vi/wallet/deposit/history | 33 | high | 0 | 0 | 0 | 0 | 4333 | 1416 | 1436 | 1184 | 1184 | 0.0000 | 31.0 | 580119 |
| logged-in-reader | mobile | /vi/wallet/withdraw | 35 | high | 0 | 0 | 0 | 0 | 4425 | 1825 | 1830 | 1240 | 1240 | 0.0029 | 63.0 | 583947 |
| logged-in-reader | mobile | /vi/notifications | 40 | critical | 0 | 0 | 2 | 0 | 3869 | 1310 | 1336 | 1148 | 1148 | 0.0000 | 33.0 | 597338 |
| logged-in-reader | mobile | /vi/reader/apply | 34 | high | 0 | 0 | 0 | 0 | 3502 | 988 | 1022 | 1036 | 1036 | 0.0000 | 26.0 | 582448 |
| logged-in-reader | mobile | /vi/reading/history | 33 | high | 0 | 0 | 0 | 0 | 3671 | 1208 | 1238 | 1252 | 1252 | 0.0000 | 22.0 | 581385 |
| logged-in-reader | mobile | /vi/legal/tos | 31 | high | 0 | 0 | 0 | 1 | 3160 | 880 | 880 | 896 | 896 | 0.0020 | 0.0 | 472003 |
| logged-in-reader | mobile | /vi/legal/privacy | 31 | high | 0 | 0 | 0 | 1 | 3046 | 794 | 794 | 752 | 1084 | 0.0020 | 1.0 | 471934 |
| logged-in-reader | mobile | /vi/legal/ai-disclaimer | 31 | high | 0 | 0 | 0 | 0 | 3102 | 852 | 852 | 908 | 908 | 0.0020 | 0.0 | 473204 |
| logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 33 | high | 0 | 0 | 0 | 0 | 3373 | 983 | 1004 | 1012 | 1012 | 0.0000 | 8.0 | 581515 |
| logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 33 | high | 0 | 0 | 0 | 0 | 3465 | 904 | 927 | 1080 | 1080 | 0.0000 | 0.0 | 581643 |
| logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 34 | high | 0 | 0 | 1 | 0 | 4273 | 1207 | 1285 | 936 | 1368 | 0.0000 | 19.0 | 582108 |
| logged-in-reader | mobile | /vi/reading/session/26cdad91-d22f-4381-a94d-e8719e43cffe | 37 | critical | 0 | 0 | 0 | 0 | 4605 | 2019 | 2153 | 1024 | 4500 | 0.0030 | 72.0 | 641342 |
| logged-in-reader | mobile | /vi/reading/session/468c3cab-d180-4700-b032-523807112e92 | 37 | critical | 0 | 0 | 0 | 0 | 4521 | 979 | 2404 | 996 | 3492 | 0.0000 | 19.0 | 641296 |
| logged-in-reader | mobile | /vi/reading/session/7e5e6c5a-77e6-4a97-b3dd-79aae6dd01af | 37 | critical | 0 | 0 | 0 | 0 | 3318 | 953 | 1175 | 976 | 976 | 0.0000 | 7.0 | 641226 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 34 | high | 0 | 0 | 1 | 0 | 3350 | 865 | 892 | 812 | 1176 | 0.0000 | 10.0 | 580437 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 3486 | 961 | 978 | 992 | 992 | 0.0000 | 8.0 | 579576 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 0 | 0 | 0 | 3757 | 1171 | 1203 | 1128 | 1128 | 0.0000 | 18.0 | 579602 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | /vi | 34 | high | 0 | 31 | 0 |
| logged-out | desktop | /vi/register | 26 | high | 1 | 22 | 0 |
| logged-out | desktop | /vi/legal/tos | 29 | high | 0 | 26 | 0 |
| logged-out | desktop | /vi/legal/privacy | 29 | high | 0 | 26 | 0 |
| logged-out | desktop | /vi/legal/ai-disclaimer | 29 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | /vi | 37 | critical | 1 | 32 | 0 |
| logged-in-admin | desktop | /vi/login | 38 | critical | 1 | 32 | 0 |
| logged-in-admin | desktop | /vi/register | 38 | critical | 1 | 32 | 0 |
| logged-in-admin | desktop | /vi/forgot-password | 38 | critical | 1 | 32 | 0 |
| logged-in-admin | desktop | /vi/reset-password | 38 | critical | 1 | 32 | 0 |
| logged-in-admin | desktop | /vi/verify-email | 38 | critical | 1 | 32 | 0 |
| logged-in-admin | desktop | /vi/reading | 34 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | /vi/inventory | 40 | critical | 0 | 37 | 0 |
| logged-in-admin | desktop | /vi/gacha | 41 | critical | 1 | 32 | 5 |
| logged-in-admin | desktop | /vi/gacha/history | 35 | high | 0 | 32 | 0 |
| logged-in-admin | desktop | /vi/collection | 105 | critical | 1 | 31 | 70 |
| logged-in-admin | desktop | /vi/profile | 34 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/profile/mfa | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/profile/reader | 68 | critical | 0 | 61 | 0 |
| logged-in-admin | desktop | /vi/readers | 35 | high | 1 | 31 | 0 |
| logged-in-admin | desktop | /vi/chat | 38 | critical | 4 | 31 | 0 |
| logged-in-admin | desktop | /vi/leaderboard | 39 | critical | 2 | 33 | 1 |
| logged-in-admin | desktop | /vi/community | 43 | critical | 3 | 37 | 0 |
| logged-in-admin | desktop | /vi/gamification | 35 | high | 1 | 31 | 0 |
| logged-in-admin | desktop | /vi/wallet | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/wallet/deposit | 34 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/wallet/withdraw | 68 | critical | 1 | 60 | 0 |
| logged-in-admin | desktop | /vi/notifications | 39 | critical | 4 | 32 | 0 |
| logged-in-admin | desktop | /vi/reader/apply | 34 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | /vi/reading/history | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/legal/tos | 31 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | /vi/legal/privacy | 31 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | /vi/legal/ai-disclaimer | 31 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | /vi/admin | 35 | high | 0 | 32 | 0 |
| logged-in-admin | desktop | /vi/admin/deposits | 35 | high | 0 | 32 | 0 |
| logged-in-admin | desktop | /vi/admin/disputes | 36 | critical | 1 | 32 | 0 |
| logged-in-admin | desktop | /vi/admin/gamification | 37 | critical | 0 | 34 | 0 |
| logged-in-admin | desktop | /vi/admin/promotions | 34 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/admin/reader-requests | 35 | high | 0 | 32 | 0 |
| logged-in-admin | desktop | /vi/admin/readings | 35 | high | 0 | 32 | 0 |
| logged-in-admin | desktop | /vi/admin/system-configs | 35 | high | 0 | 32 | 0 |
| logged-in-admin | desktop | /vi/admin/users | 36 | critical | 0 | 33 | 0 |
| logged-in-admin | desktop | /vi/admin/withdrawals | 35 | high | 0 | 32 | 0 |
| logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 34 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/reading/session/e133c6d0-0bd3-4695-8a84-92717ae0582f | 38 | critical | 1 | 34 | 0 |
| logged-in-admin | desktop | /vi/reading/session/6d346279-a2ca-4c46-b7c5-925a1cee1557 | 39 | critical | 2 | 34 | 0 |
| logged-in-admin | desktop | /vi/reading/session/99cbb8c0-4747-40ab-8d37-34959c33d792 | 38 | critical | 1 | 34 | 0 |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi | 37 | critical | 1 | 32 | 0 |
| logged-in-reader | desktop | /vi/login | 38 | critical | 1 | 32 | 0 |
| logged-in-reader | desktop | /vi/register | 38 | critical | 1 | 32 | 0 |
| logged-in-reader | desktop | /vi/forgot-password | 38 | critical | 1 | 32 | 0 |
| logged-in-reader | desktop | /vi/reset-password | 38 | critical | 1 | 32 | 0 |
| logged-in-reader | desktop | /vi/verify-email | 38 | critical | 1 | 32 | 0 |
| logged-in-reader | desktop | /vi/reading | 34 | high | 1 | 30 | 0 |
| logged-in-reader | desktop | /vi/inventory | 40 | critical | 1 | 36 | 0 |
| logged-in-reader | desktop | /vi/gacha | 41 | critical | 1 | 32 | 5 |
| logged-in-reader | desktop | /vi/gacha/history | 35 | high | 0 | 32 | 0 |
| logged-in-reader | desktop | /vi/collection | 105 | critical | 1 | 31 | 70 |
| logged-in-reader | desktop | /vi/profile | 38 | critical | 2 | 31 | 0 |
| logged-in-reader | desktop | /vi/profile/mfa | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/profile/reader | 35 | high | 1 | 30 | 0 |
| logged-in-reader | desktop | /vi/readers | 35 | high | 1 | 31 | 0 |
| logged-in-reader | desktop | /vi/chat | 38 | critical | 4 | 31 | 0 |
| logged-in-reader | desktop | /vi/leaderboard | 37 | critical | 1 | 32 | 1 |
| logged-in-reader | desktop | /vi/community | 43 | critical | 3 | 37 | 0 |
| logged-in-reader | desktop | /vi/gamification | 35 | high | 1 | 31 | 0 |
| logged-in-reader | desktop | /vi/wallet | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/wallet/deposit | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/wallet/withdraw | 35 | high | 1 | 30 | 0 |
| logged-in-reader | desktop | /vi/notifications | 39 | critical | 4 | 32 | 0 |
| logged-in-reader | desktop | /vi/reader/apply | 34 | high | 1 | 30 | 0 |
| logged-in-reader | desktop | /vi/reading/history | 34 | high | 1 | 30 | 0 |
| logged-in-reader | desktop | /vi/legal/tos | 31 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | /vi/legal/privacy | 31 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | /vi/legal/ai-disclaimer | 31 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 34 | high | 1 | 30 | 0 |
| logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 34 | high | 1 | 30 | 0 |
| logged-in-reader | desktop | /vi/reading/session/1c1ba523-21bc-420d-be45-2437f5d266bc | 39 | critical | 2 | 34 | 0 |
| logged-in-reader | desktop | /vi/reading/session/3bbbde1f-26dc-4d7e-8436-b4095381e746 | 38 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/reading/session/d63a9d0a-2fd1-4b06-8f4b-e1d4b3960d8a | 38 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 30 | 0 |
| logged-out | mobile | /vi | 34 | high | 0 | 31 | 0 |
| logged-out | mobile | /vi/register | 26 | high | 1 | 22 | 0 |
| logged-out | mobile | /vi/legal/tos | 29 | high | 0 | 26 | 0 |
| logged-out | mobile | /vi/legal/privacy | 29 | high | 0 | 26 | 0 |
| logged-out | mobile | /vi/legal/ai-disclaimer | 29 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | /vi | 37 | critical | 1 | 32 | 0 |
| logged-in-admin | mobile | /vi/login | 38 | critical | 1 | 32 | 0 |
| logged-in-admin | mobile | /vi/register | 38 | critical | 1 | 32 | 0 |
| logged-in-admin | mobile | /vi/forgot-password | 38 | critical | 1 | 32 | 0 |
| logged-in-admin | mobile | /vi/reset-password | 38 | critical | 1 | 32 | 0 |
| logged-in-admin | mobile | /vi/verify-email | 38 | critical | 1 | 32 | 0 |
| logged-in-admin | mobile | /vi/reading | 34 | high | 1 | 30 | 0 |
| logged-in-admin | mobile | /vi/inventory | 40 | critical | 0 | 37 | 0 |
| logged-in-admin | mobile | /vi/gacha | 40 | critical | 0 | 32 | 5 |
| logged-in-admin | mobile | /vi/gacha/history | 35 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/collection | 53 | critical | 1 | 31 | 18 |
| logged-in-admin | mobile | /vi/profile | 34 | high | 0 | 31 | 0 |
| logged-in-admin | mobile | /vi/profile/mfa | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/profile/reader | 68 | critical | 0 | 61 | 0 |
| logged-in-admin | mobile | /vi/readers | 36 | critical | 2 | 31 | 0 |
| logged-in-admin | mobile | /vi/chat | 38 | critical | 4 | 31 | 0 |
| logged-in-admin | mobile | /vi/leaderboard | 38 | critical | 1 | 33 | 1 |
| logged-in-admin | mobile | /vi/community | 43 | critical | 3 | 37 | 0 |
| logged-in-admin | mobile | /vi/gamification | 35 | high | 1 | 31 | 0 |
| logged-in-admin | mobile | /vi/wallet | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/wallet/deposit | 34 | high | 1 | 30 | 0 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/wallet/withdraw | 67 | critical | 0 | 60 | 0 |
| logged-in-admin | mobile | /vi/notifications | 39 | critical | 4 | 32 | 0 |
| logged-in-admin | mobile | /vi/reader/apply | 34 | high | 1 | 30 | 0 |
| logged-in-admin | mobile | /vi/reading/history | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/legal/tos | 31 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | /vi/legal/privacy | 31 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | /vi/legal/ai-disclaimer | 31 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | /vi/admin | 35 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/admin/deposits | 35 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/admin/disputes | 36 | critical | 1 | 32 | 0 |
| logged-in-admin | mobile | /vi/admin/gamification | 37 | critical | 0 | 34 | 0 |
| logged-in-admin | mobile | /vi/admin/promotions | 35 | high | 1 | 31 | 0 |
| logged-in-admin | mobile | /vi/admin/reader-requests | 35 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/admin/readings | 35 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/admin/system-configs | 35 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/admin/users | 36 | critical | 0 | 33 | 0 |
| logged-in-admin | mobile | /vi/admin/withdrawals | 35 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/reading/session/43116b7f-80b3-47ff-bb27-8a18d1fdd778 | 37 | critical | 1 | 33 | 0 |
| logged-in-admin | mobile | /vi/reading/session/62007c5e-90c4-4096-b410-ae1a30379000 | 37 | critical | 1 | 33 | 0 |
| logged-in-admin | mobile | /vi/reading/session/8f8c12fc-54f8-4cd0-ae23-9696736073ec | 38 | critical | 2 | 33 | 0 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi | 37 | critical | 1 | 32 | 0 |
| logged-in-reader | mobile | /vi/login | 38 | critical | 1 | 32 | 0 |
| logged-in-reader | mobile | /vi/register | 38 | critical | 1 | 32 | 0 |
| logged-in-reader | mobile | /vi/forgot-password | 38 | critical | 1 | 32 | 0 |
| logged-in-reader | mobile | /vi/reset-password | 38 | critical | 1 | 32 | 0 |
| logged-in-reader | mobile | /vi/verify-email | 38 | critical | 1 | 32 | 0 |
| logged-in-reader | mobile | /vi/reading | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/inventory | 39 | critical | 0 | 36 | 0 |
| logged-in-reader | mobile | /vi/gacha | 40 | critical | 0 | 32 | 5 |
| logged-in-reader | mobile | /vi/gacha/history | 35 | high | 0 | 32 | 0 |
| logged-in-reader | mobile | /vi/collection | 53 | critical | 1 | 31 | 18 |
| logged-in-reader | mobile | /vi/profile | 38 | critical | 2 | 31 | 0 |
| logged-in-reader | mobile | /vi/profile/mfa | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/profile/reader | 35 | high | 1 | 30 | 0 |
| logged-in-reader | mobile | /vi/readers | 35 | high | 1 | 31 | 0 |
| logged-in-reader | mobile | /vi/chat | 38 | critical | 4 | 31 | 0 |
| logged-in-reader | mobile | /vi/leaderboard | 37 | critical | 1 | 32 | 1 |
| logged-in-reader | mobile | /vi/community | 43 | critical | 3 | 37 | 0 |
| logged-in-reader | mobile | /vi/gamification | 35 | high | 1 | 31 | 0 |
| logged-in-reader | mobile | /vi/wallet | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/wallet/deposit | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/wallet/deposit/history | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/wallet/withdraw | 35 | high | 1 | 30 | 0 |
| logged-in-reader | mobile | /vi/notifications | 40 | critical | 5 | 32 | 0 |
| logged-in-reader | mobile | /vi/reader/apply | 34 | high | 1 | 30 | 0 |
| logged-in-reader | mobile | /vi/reading/history | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/legal/tos | 31 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | /vi/legal/privacy | 31 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | /vi/legal/ai-disclaimer | 31 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 34 | high | 1 | 30 | 0 |
| logged-in-reader | mobile | /vi/reading/session/26cdad91-d22f-4381-a94d-e8719e43cffe | 37 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | /vi/reading/session/468c3cab-d180-4700-b032-523807112e92 | 37 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | /vi/reading/session/7e5e6c5a-77e6-4a97-b3dd-79aae6dd01af | 37 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 34 | high | 1 | 30 | 0 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 30 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2934 | 171 | third-party | https://img.tarotnow.xyz/light-god-50/42_Six_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2884 | 136 | third-party | https://img.tarotnow.xyz/light-god-50/41_Five_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2824 | 135 | third-party | https://img.tarotnow.xyz/light-god-50/40_Four_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2796 | 128 | third-party | https://img.tarotnow.xyz/light-god-50/39_Three_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2751 | 130 | third-party | https://img.tarotnow.xyz/light-god-50/37_Ace_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2749 | 146 | third-party | https://img.tarotnow.xyz/light-god-50/38_Two_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2701 | 131 | third-party | https://img.tarotnow.xyz/light-god-50/64_King_of_Swords_50_20260325_181428.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2618 | 112 | third-party | https://img.tarotnow.xyz/light-god-50/63_Queen_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2572 | 109 | third-party | https://img.tarotnow.xyz/light-god-50/62_Knight_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2551 | 113 | third-party | https://img.tarotnow.xyz/light-god-50/61_Page_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2536 | 136 | third-party | https://img.tarotnow.xyz/light-god-50/59_Nine_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2535 | 151 | third-party | https://img.tarotnow.xyz/light-god-50/58_Eight_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2534 | 128 | third-party | https://img.tarotnow.xyz/light-god-50/57_Seven_of_Swords_50_20260325_181424.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2534 | 126 | third-party | https://img.tarotnow.xyz/light-god-50/60_Ten_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2507 | 165 | third-party | https://img.tarotnow.xyz/light-god-50/56_Six_of_Swords_50_20260325_181424.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2497 | 185 | third-party | https://img.tarotnow.xyz/light-god-50/55_Five_of_Swords_50_20260325_181424.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2461 | 187 | third-party | https://img.tarotnow.xyz/light-god-50/54_Four_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2431 | 280 | third-party | https://img.tarotnow.xyz/light-god-50/53_Three_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2402 | 314 | third-party | https://img.tarotnow.xyz/light-god-50/52_Two_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2358 | 295 | third-party | https://img.tarotnow.xyz/light-god-50/51_Ace_of_Swords_50_20260325_181419.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2286 | 256 | third-party | https://img.tarotnow.xyz/light-god-50/36_King_of_Cups_50_20260325_181408.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2242 | 256 | third-party | https://img.tarotnow.xyz/light-god-50/35_Queen_of_Cups_50_20260325_181408.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2228 | 303 | third-party | https://img.tarotnow.xyz/light-god-50/51_Ace_of_Swords_50_20260325_181419.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2192 | 273 | third-party | https://img.tarotnow.xyz/light-god-50/34_Knight_of_Cups_50_20260325_181408.avif |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 2170 | 345 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | /vi/admin/deposits | GET | 200 | 2149 | 375 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2138 | 338 | third-party | https://img.tarotnow.xyz/light-god-50/36_King_of_Cups_50_20260325_181408.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2137 | 314 | third-party | https://img.tarotnow.xyz/light-god-50/35_Queen_of_Cups_50_20260325_181408.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2135 | 190 | third-party | https://img.tarotnow.xyz/light-god-50/56_Six_of_Swords_50_20260325_181424.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2126 | 184 | third-party | https://img.tarotnow.xyz/light-god-50/55_Five_of_Swords_50_20260325_181424.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2091 | 219 | third-party | https://img.tarotnow.xyz/light-god-50/33_Page_of_Cups_50_20260325_181406.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2080 | 259 | third-party | https://img.tarotnow.xyz/light-god-50/52_Two_of_Swords_50_20260325_181422.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2080 | 253 | third-party | https://img.tarotnow.xyz/light-god-50/54_Four_of_Swords_50_20260325_181422.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2068 | 241 | third-party | https://img.tarotnow.xyz/light-god-50/53_Three_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2034 | 190 | third-party | https://img.tarotnow.xyz/light-god-50/32_Ten_of_Cups_50_20260325_181406.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2010 | 224 | third-party | https://img.tarotnow.xyz/light-god-50/34_Knight_of_Cups_50_20260325_181408.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1973 | 272 | third-party | https://img.tarotnow.xyz/light-god-50/33_Page_of_Cups_50_20260325_181406.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1963 | 252 | third-party | https://img.tarotnow.xyz/light-god-50/32_Ten_of_Cups_50_20260325_181406.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1925 | 198 | third-party | https://img.tarotnow.xyz/light-god-50/31_Nine_of_Cups_50_20260325_181406.avif |
| logged-in-reader | mobile | /vi/reading/session/26cdad91-d22f-4381-a94d-e8719e43cffe | GET | 200 | 1876 | 312 | html | https://www.tarotnow.xyz/vi/reading/session/26cdad91-d22f-4381-a94d-e8719e43cffe |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1864 | 216 | third-party | https://img.tarotnow.xyz/light-god-50/29_Seven_of_Cups_50_20260325_181405.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1863 | 257 | third-party | https://img.tarotnow.xyz/light-god-50/30_Eight_of_Cups_50_20260325_181405.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1863 | 236 | third-party | https://img.tarotnow.xyz/light-god-50/31_Nine_of_Cups_50_20260325_181406.avif |
| logged-in-reader | mobile | /vi/gamification | GET | 200 | 1852 | 364 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | /vi/readers | GET | 200 | 1846 | 330 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1837 | 341 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1818 | 138 | third-party | https://img.tarotnow.xyz/light-god-50/28_Six_of_Cups_50_20260325_181405.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1796 | 158 | third-party | https://img.tarotnow.xyz/light-god-50/29_Seven_of_Cups_50_20260325_181405.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1791 | 199 | third-party | https://img.tarotnow.xyz/light-god-50/30_Eight_of_Cups_50_20260325_181405.avif |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1762 | 77 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1750 | 147 | third-party | https://img.tarotnow.xyz/light-god-50/26_Four_of_Cups_50_20260325_181402.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1745 | 126 | third-party | https://img.tarotnow.xyz/light-god-50/27_Five_of_Cups_50_20260325_181402.avif |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1726 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1720 | 158 | third-party | https://img.tarotnow.xyz/light-god-50/28_Six_of_Cups_50_20260325_181405.avif |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1720 | 75 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1720 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1718 | 91 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1714 | 76 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg6ntv_3jdd4.js |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1714 | 68 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1713 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/0zl0xwyc461q_.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1705 | 426 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 1705 | 330 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1680 | 224 | third-party | https://img.tarotnow.xyz/light-god-50/25_Three_of_Cups_50_20260325_181402.avif |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1677 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | /vi/admin/deposits | GET | 200 | 1677 | 66 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1670 | 89 | static | https://www.tarotnow.xyz/_next/static/chunks/0fqgq6m2b-440.css |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1660 | 143 | third-party | https://img.tarotnow.xyz/light-god-50/27_Five_of_Cups_50_20260325_181402.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1647 | 153 | third-party | https://img.tarotnow.xyz/light-god-50/26_Four_of_Cups_50_20260325_181402.avif |
| logged-in-reader | mobile | /vi/readers | GET | 200 | 1611 | 306 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | /vi/admin/deposits | GET | 200 | 1610 | 317 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1601 | 142 | third-party | https://img.tarotnow.xyz/light-god-50/25_Three_of_Cups_50_20260325_181402.avif |
| logged-in-admin | mobile | /vi/reset-password | GET | 200 | 1585 | 327 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1578 | 246 | third-party | https://img.tarotnow.xyz/light-god-50/24_Two_of_Cups_50_20260325_181401.avif |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1575 | 81 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1567 | 350 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | /vi/reading/session/26cdad91-d22f-4381-a94d-e8719e43cffe | GET | 200 | 1564 | 301 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1545 | 89 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1539 | 131 | third-party | https://img.tarotnow.xyz/light-god-50/24_Two_of_Cups_50_20260325_181401.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1513 | 240 | third-party | https://img.tarotnow.xyz/light-god-50/23_Ace_of_Cups_50_20260325_181401.avif |
| logged-in-reader | mobile | /vi/readers | GET | 200 | 1502 | 56 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | desktop | /vi/forgot-password | GET | 200 | 800 | 400 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | /vi/forgot-password | GET | 200 | 800 | 306 | api | https://www.tarotnow.xyz/api/wallet/balance |
| logged-in-reader | desktop | /vi/reading/session/d63a9d0a-2fd1-4b06-8f4b-e1d4b3960d8a | GET | 200 | 800 | 68 | static | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 800 | 153 | static | https://www.tarotnow.xyz/_next/static/chunks/08q.cp_si1m5q.js |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 800 | 77 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | /vi/community | GET | 200 | 799 | 150 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 799 | 70 | static | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 799 | 69 | static | https://www.tarotnow.xyz/_next/static/chunks/167n5~xbg2bxe.js |
| logged-in-reader | desktop | /vi/wallet | GET | 200 | 798 | 422 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 798 | 317 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 798 | 69 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | desktop | /vi/wallet/deposit | GET | 200 | 797 | 426 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 797 | 64 | static | https://www.tarotnow.xyz/_next/static/chunks/0nrdqsqvy_hhn.js |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 796 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/04bi685.wk-4b.js |
| logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 796 | 74 | static | https://www.tarotnow.xyz/_next/static/chunks/08nnjyw~vjmez.js |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 796 | 74 | static | https://www.tarotnow.xyz/_next/static/chunks/0hf3ddbnzc0yj.js |
| logged-in-admin | desktop | /vi/community | GET | 404 | 794 | 644 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F1bf7374304584c0488e06621bbc1454f.webp&w=48&q=75 |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 794 | 104 | static | https://www.tarotnow.xyz/_next/static/chunks/04bi685.wk-4b.js |
| logged-in-admin | desktop | /vi/reading | GET | 200 | 793 | 327 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 793 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/04wgpsc4kh~3w.js |
| logged-in-admin | desktop | /vi/gacha | GET | 200 | 792 | 417 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | /vi/profile/reader | GET | 200 | 791 | 365 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 791 | 84 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 791 | 68 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 791 | 77 | static | https://www.tarotnow.xyz/_next/static/chunks/0.npsys3-p5.f.js |
| logged-in-admin | desktop | /vi/community | GET | 200 | 790 | 120 | static | https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-reader | desktop | /vi/community | GET | 200 | 790 | 302 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | desktop | /vi/community | GET | 200 | 788 | 117 | static | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-reader | mobile | /vi/leaderboard | GET | 200 | 787 | 400 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 786 | 106 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg6ntv_3jdd4.js |
| logged-in-reader | desktop | /vi | GET | 200 | 782 | 86 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 782 | 76 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 782 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 782 | 64 | static | https://www.tarotnow.xyz/_next/static/chunks/0.npsys3-p5.f.js |
| logged-in-admin | mobile | /vi/forgot-password | GET | 200 | 781 | 358 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | /vi/leaderboard | GET | 200 | 780 | 343 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 780 | 75 | static | https://www.tarotnow.xyz/_next/static/chunks/0.npsys3-p5.f.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 779 | 119 | static | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-reader | mobile | /vi/legal/tos | GET | 200 | 779 | 313 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-reader | mobile | /vi/reading/session/468c3cab-d180-4700-b032-523807112e92 | GET | 200 | 779 | 302 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 778 | 311 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | /vi/reading/session/d63a9d0a-2fd1-4b06-8f4b-e1d4b3960d8a | GET | 200 | 776 | 45 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 775 | 416 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 774 | 69 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 774 | 60 | static | https://www.tarotnow.xyz/_next/static/chunks/07tk2ft0d9n3x.js |
| logged-in-reader | desktop | /vi/reading/session/d63a9d0a-2fd1-4b06-8f4b-e1d4b3960d8a | GET | 200 | 772 | 93 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 772 | 112 | static | https://www.tarotnow.xyz/_next/static/chunks/0zl0xwyc461q_.js |
| logged-in-reader | mobile | /vi/reading/session/7e5e6c5a-77e6-4a97-b3dd-79aae6dd01af | GET | 200 | 769 | 375 | html | https://www.tarotnow.xyz/vi/reading/session/7e5e6c5a-77e6-4a97-b3dd-79aae6dd01af |
| logged-in-admin | desktop | /vi/community | GET | 200 | 768 | 108 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | desktop | /vi/profile/mfa | GET | 200 | 768 | 432 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 768 | 182 | third-party | https://img.tarotnow.xyz/light-god-50/10_The_Hermit_50_20260325_181353.avif |
| logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 768 | 170 | static | https://www.tarotnow.xyz/_next/static/chunks/167n5~xbg2bxe.js |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 768 | 105 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 767 | 246 | third-party | https://img.tarotnow.xyz/light-god-50/18_The_Star_50_20260325_181357.avif |
| logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 766 | 304 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | desktop | /vi | GET | 200 | 764 | 311 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=384&q=75 |
| logged-in-admin | mobile | /vi/admin/withdrawals | GET | 200 | 764 | 598 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 764 | 68 | static | https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-reader | desktop | /vi/reading/session/d63a9d0a-2fd1-4b06-8f4b-e1d4b3960d8a | GET | 200 | 763 | 91 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | /vi/verify-email | GET | 200 | 763 | 345 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 763 | 115 | third-party | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 761 | 72 | static | https://www.tarotnow.xyz/_next/static/chunks/083_c9ltii8mv.js |
| logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 761 | 72 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 760 | 325 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 760 | 58 | static | https://www.tarotnow.xyz/_next/static/chunks/04wgpsc4kh~3w.js |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 758 | 323 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 758 | 76 | static | https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 758 | 387 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | /vi/login | GET | 200 | 756 | 327 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 756 | 86 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 755 | 72 | static | https://www.tarotnow.xyz/_next/static/chunks/0nrdqsqvy_hhn.js |
| logged-in-reader | mobile | /vi/reader/apply | GET | 200 | 755 | 298 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | /vi/reset-password | GET | 200 | 754 | 439 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | /vi | GET | 200 | 754 | 379 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 754 | 118 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | /vi/community | GET | 200 | 753 | 132 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-admin | mobile | /vi/admin/withdrawals | GET | 200 | 751 | 449 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | /vi/reading/session/d63a9d0a-2fd1-4b06-8f4b-e1d4b3960d8a | GET | 200 | 749 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | /vi/wallet | GET | 200 | 749 | 406 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 748 | 237 | third-party | https://img.tarotnow.xyz/light-god-50/10_The_Hermit_50_20260325_181353.avif |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/57_Seven_of_Swords_50_20260325_181424.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/58_Eight_of_Swords_50_20260325_181426.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/59_Nine_of_Swords_50_20260325_181426.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/60_Ten_of_Swords_50_20260325_181426.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/61_Page_of_Swords_50_20260325_181427.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/62_Knight_of_Swords_50_20260325_181427.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/63_Queen_of_Swords_50_20260325_181427.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/64_King_of_Swords_50_20260325_181428.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/37_Ace_of_Pentacles_50_20260325_181411.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/38_Two_of_Pentacles_50_20260325_181411.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/39_Three_of_Pentacles_50_20260325_181411.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/40_Four_of_Pentacles_50_20260325_181413.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/41_Five_of_Pentacles_50_20260325_181413.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/42_Six_of_Pentacles_50_20260325_181413.avif |

## Coverage Notes
| Scenario | Viewport | Note |
| --- | --- | --- |
| logged-out | desktop | origin-discovery:/sitemap.xml:routes-13 |
| logged-out | desktop | origin-discovery:/robots.txt:routes-13 |
| logged-out | desktop | dynamic-routes: skipped for logged-out scenario. |
| logged-out | desktop | scenario-filter:logged-out-protected-routes-skipped=33 |
| logged-in-admin | desktop | origin-discovery:/sitemap.xml:routes-13 |
| logged-in-admin | desktop | origin-discovery:/robots.txt:routes-13 |
| logged-in-admin | desktop | reading.init.daily_1: blocked (400). |
| logged-in-admin | desktop | reading.init.spread_3: created e133c6d0-0bd3-4695-8a84-92717ae0582f. |
| logged-in-admin | desktop | reading.init.spread_5: created 6d346279-a2ca-4c46-b7c5-925a1cee1557. |
| logged-in-admin | desktop | reading.init.spread_10: created 99cbb8c0-4747-40ab-8d37-34959c33d792. |
| logged-in-admin | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | desktop | reader-detail:ui-discovery-empty |
| logged-in-admin | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-admin | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-admin | desktop | community-posts:api-discovery-1 |
| logged-in-admin | desktop | community-post-detail:69db54fc297f66f734421a3c:200 |
| logged-in-reader | desktop | origin-discovery:/sitemap.xml:routes-13 |
| logged-in-reader | desktop | origin-discovery:/robots.txt:routes-13 |
| logged-in-reader | desktop | reading.init.daily_1: blocked (400). |
| logged-in-reader | desktop | reading.init.spread_3: created 1c1ba523-21bc-420d-be45-2437f5d266bc. |
| logged-in-reader | desktop | reading.init.spread_5: created 3bbbde1f-26dc-4d7e-8436-b4095381e746. |
| logged-in-reader | desktop | reading.init.spread_10: created d63a9d0a-2fd1-4b06-8f4b-e1d4b3960d8a. |
| logged-in-reader | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | desktop | reader-detail:ui-discovery-empty |
| logged-in-reader | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-reader | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-reader | desktop | community-posts:api-discovery-1 |
| logged-in-reader | desktop | community-post-detail:69db54fc297f66f734421a3c:200 |
| logged-in-reader | desktop | scenario-filter:reader-admin-routes-skipped=10 |
| logged-out | mobile | origin-discovery:/sitemap.xml:routes-13 |
| logged-out | mobile | origin-discovery:/robots.txt:routes-13 |
| logged-out | mobile | dynamic-routes: skipped for logged-out scenario. |
| logged-out | mobile | scenario-filter:logged-out-protected-routes-skipped=33 |
| logged-in-admin | mobile | origin-discovery:/sitemap.xml:routes-13 |
| logged-in-admin | mobile | origin-discovery:/robots.txt:routes-13 |
| logged-in-admin | mobile | reading.init.daily_1: blocked (400). |
| logged-in-admin | mobile | reading.init.spread_3: created 43116b7f-80b3-47ff-bb27-8a18d1fdd778. |
| logged-in-admin | mobile | reading.init.spread_5: created 62007c5e-90c4-4096-b410-ae1a30379000. |
| logged-in-admin | mobile | reading.init.spread_10: created 8f8c12fc-54f8-4cd0-ae23-9696736073ec. |
| logged-in-admin | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | mobile | reader-detail:ui-discovery-empty |
| logged-in-admin | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-admin | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-admin | mobile | community-posts:api-discovery-1 |
| logged-in-admin | mobile | community-post-detail:69db54fc297f66f734421a3c:200 |
| logged-in-reader | mobile | origin-discovery:/sitemap.xml:routes-13 |
| logged-in-reader | mobile | origin-discovery:/robots.txt:routes-13 |
| logged-in-reader | mobile | reading.init.daily_1: blocked (400). |
| logged-in-reader | mobile | reading.init.spread_3: created 26cdad91-d22f-4381-a94d-e8719e43cffe. |
| logged-in-reader | mobile | reading.init.spread_5: created 468c3cab-d180-4700-b032-523807112e92. |
| logged-in-reader | mobile | reading.init.spread_10: created 7e5e6c5a-77e6-4a97-b3dd-79aae6dd01af. |
| logged-in-reader | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | mobile | reader-detail:ui-discovery-empty |
| logged-in-reader | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-reader | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-reader | mobile | community-posts:api-discovery-1 |
| logged-in-reader | mobile | community-post-detail:69db54fc297f66f734421a3c:200 |
| logged-in-reader | mobile | scenario-filter:reader-admin-routes-skipped=10 |

## Login Bootstrap Notes
### logged-in-admin / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login bootstrap succeeded.

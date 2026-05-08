# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T15:28:23.010Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 15
- High pages (request count): 126
- High slow requests: 1158
- Medium slow requests: 652

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 5285 | 224 | 1 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 38 | 3463 | 1144 | 3 | 0 | 19 | 0 | yes |
| logged-in-reader | desktop | 38 | 3349 | 1155 | 3 | 0 | 13 | 0 | yes |
| logged-out | mobile | 9 | 8953 | 227 | 3 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 35 | 11089 | 1177 | 130 | 0 | 39 | 0 | yes |
| logged-in-reader | mobile | 35 | 6555 | 994 | 91 | 0 | 3 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | auth | 5 | 29.8 | 3298 | 1 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2816 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6611 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4289 | 0 |
| logged-in-admin | desktop | gacha | 2 | 37.5 | 4418 | 0 |
| logged-in-admin | desktop | gamification | 1 | 32.0 | 3364 | 0 |
| logged-in-admin | desktop | home | 1 | 39.0 | 4975 | 0 |
| logged-in-admin | desktop | inventory | 1 | 36.0 | 3110 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 33.0 | 3294 | 0 |
| logged-in-admin | desktop | legal | 3 | 26.3 | 3337 | 0 |
| logged-in-admin | desktop | notifications | 1 | 33.0 | 4081 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.3 | 3220 | 2 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 4174 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.3 | 3173 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.6 | 2857 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.5 | 3341 | 0 |
| logged-in-admin | mobile | auth | 5 | 43.2 | 12368 | 18 |
| logged-in-admin | mobile | chat | 1 | 31.0 | 6600 | 3 |
| logged-in-admin | mobile | collection | 1 | 32.0 | 4274 | 14 |
| logged-in-admin | mobile | community | 1 | 37.0 | 11624 | 4 |
| logged-in-admin | mobile | gacha | 2 | 33.0 | 4660 | 7 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 15128 | 3 |
| logged-in-admin | mobile | home | 1 | 35.0 | 6081 | 6 |
| logged-in-admin | mobile | inventory | 1 | 35.0 | 3867 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 32.0 | 6209 | 2 |
| logged-in-admin | mobile | legal | 3 | 25.7 | 10651 | 6 |
| logged-in-admin | mobile | notifications | 1 | 29.0 | 17258 | 3 |
| logged-in-admin | mobile | profile | 3 | 42.0 | 19043 | 3 |
| logged-in-admin | mobile | reader | 1 | 31.0 | 15373 | 3 |
| logged-in-admin | mobile | readers | 4 | 30.5 | 9234 | 10 |
| logged-in-admin | mobile | reading | 5 | 32.0 | 9846 | 14 |
| logged-in-admin | mobile | wallet | 4 | 29.8 | 13822 | 34 |
| logged-in-reader | desktop | auth | 5 | 35.6 | 4210 | 3 |
| logged-in-reader | desktop | chat | 1 | 31.0 | 3214 | 0 |
| logged-in-reader | desktop | collection | 1 | 30.0 | 6120 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3592 | 0 |
| logged-in-reader | desktop | gacha | 2 | 32.5 | 3531 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 3099 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2856 | 0 |
| logged-in-reader | desktop | inventory | 1 | 41.0 | 4616 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2888 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.7 | 2859 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2977 | 0 |
| logged-in-reader | desktop | profile | 3 | 31.3 | 3094 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 3488 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.3 | 2969 | 0 |
| logged-in-reader | desktop | reading | 5 | 30.4 | 2884 | 0 |
| logged-in-reader | desktop | wallet | 4 | 29.5 | 3305 | 0 |
| logged-in-reader | mobile | auth | 5 | 24.0 | 5337 | 53 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 6238 | 1 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 7864 | 3 |
| logged-in-reader | mobile | community | 1 | 34.0 | 9830 | 1 |
| logged-in-reader | mobile | gacha | 2 | 33.0 | 5944 | 4 |
| logged-in-reader | mobile | gamification | 1 | 31.0 | 5695 | 0 |
| logged-in-reader | mobile | home | 1 | 35.0 | 5856 | 6 |
| logged-in-reader | mobile | inventory | 1 | 35.0 | 5525 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 6922 | 2 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 7557 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 7617 | 1 |
| logged-in-reader | mobile | profile | 3 | 28.3 | 5990 | 5 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 6158 | 2 |
| logged-in-reader | mobile | readers | 4 | 28.0 | 6914 | 3 |
| logged-in-reader | mobile | reading | 5 | 29.2 | 6010 | 6 |
| logged-in-reader | mobile | wallet | 4 | 28.0 | 7695 | 4 |
| logged-out | desktop | auth | 5 | 24.0 | 5566 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4085 | 1 |
| logged-out | desktop | legal | 3 | 25.0 | 5218 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 8354 | 1 |
| logged-out | mobile | home | 1 | 31.0 | 3264 | 2 |
| logged-out | mobile | legal | 3 | 25.0 | 11847 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4085 | 1431 | 2078 | 1452 | 1452 | 0.0000 | 296.0 | 601447 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5880 | 3697 | 3870 | 3728 | 3728 | 0.0000 | 0.0 | 512383 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4968 | 1886 | 2961 | 1148 | 1148 | 0.0000 | 0.0 | 512822 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4705 | 2131 | 2699 | 1196 | 1196 | 0.0000 | 0.0 | 512202 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6492 | 3585 | 4484 | 1732 | 1732 | 0.0000 | 0.0 | 512127 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5783 | 3117 | 3776 | 2228 | 2228 | 0.0000 | 0.0 | 512297 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5079 | 2278 | 3070 | 1720 | 1740 | 0.0000 | 0.0 | 525860 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4960 | 2794 | 2949 | 1196 | 1528 | 0.0000 | 0.0 | 525840 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5614 | 2172 | 3602 | 1804 | 1804 | 0.0000 | 0.0 | 525929 |
| logged-in-admin | desktop | auth-public | /vi | 39 | 5 | critical | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4975 | 874 | 2968 | 608 | 1108 | 0.0035 | 116.0 | 624919 |
| logged-in-admin | desktop | auth-public | /vi/login | 24 | 42 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2839 | 782 | 821 | 624 | 1092 | 0.0035 | 144.0 | 511020 |
| logged-in-admin | desktop | auth-public | /vi/register | 24 | 31 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2910 | 643 | - | 608 | 1092 | 0.0000 | 49.0 | 511009 |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 53 | 11 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3985 | 969 | 1346 | 588 | 1176 | 0.0035 | 58.0 | 1108244 |
| logged-in-admin | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3702 | 646 | 1696 | 624 | 624 | 0.0000 | 0.0 | 512043 |
| logged-in-admin | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3056 | 865 | 1051 | 644 | 644 | 0.0000 | 0.0 | 512168 |
| logged-in-admin | desktop | reading | /vi/reading | 31 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3036 | 873 | 1029 | 620 | 1020 | 0.0042 | 0.0 | 644859 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 36 | 6 | critical | 0 | 0 | 1 | 0 | 5 | 0 | 5 | 5 | 0 | 0 | 3110 | 931 | 1103 | 676 | 1056 | 0.0042 | 0.0 | 653547 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 40 | 3 | critical | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 4907 | 880 | 2897 | 620 | 1032 | 0.0042 | 0.0 | 745870 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 35 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3929 | 865 | 1921 | 644 | 1040 | 0.0042 | 0.0 | 738899 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6611 | 835 | 1309 | 648 | 1044 | 0.0043 | 0.0 | 643444 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 32 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3262 | 851 | 1255 | 628 | 1004 | 0.0489 | 0.0 | 637591 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3470 | 1120 | 1464 | 1116 | 1116 | 0.0042 | 0.0 | 632007 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 41 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2929 | 924 | 1122 | 908 | 1196 | 0.0489 | 0.0 | 630800 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3526 | 1502 | 1520 | 832 | 1716 | 0.0042 | 0.0 | 634114 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2816 | 797 | 808 | 568 | 980 | 0.0042 | 0.0 | 631534 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3294 | 808 | 1288 | 616 | 992 | 0.0180 | 0.0 | 653068 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | 6 | high | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 4289 | 1290 | 1533 | 584 | 2528 | 0.0042 | 0.0 | 777593 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 32 | 6 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3364 | 923 | 1357 | 748 | 1092 | 0.0432 | 0.0 | 645483 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4381 | 878 | 2374 | 676 | 1040 | 0.0042 | 0.0 | 634174 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2967 | 844 | 957 | 632 | 1012 | 0.0042 | 0.0 | 633906 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3079 | 814 | 1071 | 608 | 980 | 0.0042 | 0.0 | 632252 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 40 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2935 | 822 | 1165 | 612 | 1068 | 0.0042 | 0.0 | 631201 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 33 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4081 | 703 | 2074 | 608 | 984 | 0.0047 | 0.0 | 644910 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4174 | 913 | 2167 | 616 | 1008 | 0.0042 | 0.0 | 632419 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2710 | 680 | 703 | 564 | 1092 | 0.0042 | 0.0 | 633154 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3769 | 645 | 1763 | 732 | 732 | 0.0020 | 0.0 | 528046 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2827 | 790 | 820 | 628 | 932 | 0.0020 | 0.0 | 526322 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3416 | 697 | 1408 | 620 | 928 | 0.0020 | 0.0 | 527167 |
| logged-in-admin | desktop | reading | /vi/reading/session/e26fd5ff-5ed0-47ee-92d0-ab217ba39696 | 33 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3028 | 895 | 1020 | 560 | 964 | 0.0042 | 0.0 | 714987 |
| logged-in-admin | desktop | reading | /vi/reading/session/ec621f38-5e41-4d73-b92f-da98bb21bdd0 | 28 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2768 | 746 | 763 | 664 | 1008 | 0.0042 | 0.0 | 632245 |
| logged-in-admin | desktop | reading | /vi/reading/session/7f250428-778b-47cf-98f4-71cdaf7c2a5f | 28 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2745 | 732 | 739 | 568 | 972 | 0.0042 | 0.0 | 632236 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3777 | 1765 | 1767 | 704 | 1756 | 0.0042 | 0.0 | 633448 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3076 | 1047 | 1069 | 848 | 1216 | 0.0042 | 0.0 | 631173 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3168 | 710 | 1162 | 608 | 980 | 0.0042 | 0.0 | 631250 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2760 | 734 | 752 | 604 | 1092 | 0.0042 | 0.0 | 632911 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2740 | 699 | 735 | 596 | 1080 | 0.0042 | 0.0 | 632980 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3166 | 769 | 1159 | 720 | 1088 | 0.0042 | 0.0 | 633157 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 14 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2856 | 848 | 848 | 692 | 1188 | 0.0033 | 163.0 | 537711 |
| logged-in-reader | desktop | auth-public | /vi/login | 53 | 11 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5508 | 915 | 2636 | 824 | 1380 | 0.0038 | 158.0 | 1108150 |
| logged-in-reader | desktop | auth-public | /vi/register | 24 | 42 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3752 | 1070 | 1449 | 768 | 1292 | 0.0033 | 123.0 | 511019 |
| logged-in-reader | desktop | auth-public | /vi/forgot-password | 53 | 11 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5889 | 791 | 2698 | 624 | 1108 | 0.0033 | 56.0 | 1108181 |
| logged-in-reader | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2870 | 839 | 839 | 648 | 648 | 0.0000 | 0.0 | 512060 |
| logged-in-reader | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3030 | 1021 | 1021 | 764 | 764 | 0.0001 | 0.0 | 512204 |
| logged-in-reader | desktop | reading | /vi/reading | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3246 | 1230 | 1239 | 792 | 1300 | 0.0039 | 0.0 | 641830 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 41 | 1 | critical | 0 | 0 | 2 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 4616 | 1664 | 2605 | 724 | 1656 | 0.0039 | 0.0 | 665932 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 35 | 7 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3981 | 1485 | 1975 | 1072 | 1544 | 0.0039 | 0.0 | 732441 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3081 | 839 | 1072 | 588 | 988 | 0.0039 | 0.0 | 724457 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 30 | 32 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6120 | 707 | 830 | 616 | 616 | 0.0040 | 0.0 | 643357 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 36 | 2 | critical | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3757 | 1427 | 1750 | 652 | 1484 | 0.0726 | 0.0 | 649579 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2739 | 704 | 729 | 772 | 772 | 0.0039 | 0.0 | 633884 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2786 | 734 | 777 | 656 | 1384 | 0.0039 | 0.0 | 632369 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2967 | 937 | 944 | 636 | 1040 | 0.0039 | 0.0 | 634187 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 31 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3214 | 785 | 1206 | 596 | 992 | 0.0039 | 0.0 | 635150 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2888 | 841 | 876 | 796 | 1140 | 0.0177 | 0.0 | 649885 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | 11 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3592 | 852 | 864 | 764 | 1852 | 0.0039 | 0.0 | 643299 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3099 | 832 | 1092 | 604 | 1000 | 0.0430 | 0.0 | 642015 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2956 | 836 | 949 | 604 | 984 | 0.0039 | 0.0 | 634352 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3421 | 760 | 1411 | 608 | 1000 | 0.0039 | 0.0 | 631564 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 34 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4011 | 719 | 2005 | 612 | 996 | 0.0039 | 0.0 | 645629 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2833 | 825 | 825 | 668 | 1076 | 0.0095 | 0.0 | 633277 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2977 | 951 | 969 | 676 | 1076 | 0.0040 | 0.0 | 632293 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3488 | 830 | 1480 | 652 | 1008 | 0.0039 | 0.0 | 632722 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2694 | 668 | 686 | 576 | 1044 | 0.0039 | 0.0 | 633105 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2678 | 668 | 668 | 668 | 668 | 0.0019 | 0.0 | 526351 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3157 | 748 | 1150 | 604 | 920 | 0.0019 | 0.0 | 526874 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2743 | 733 | 733 | 620 | 936 | 0.0019 | 0.0 | 526449 |
| logged-in-reader | desktop | reading | /vi/reading/session/6b2c7f7e-ff4e-4711-a01f-f772d48b0642 | 28 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2771 | 742 | 762 | 592 | 984 | 0.0039 | 0.0 | 632418 |
| logged-in-reader | desktop | reading | /vi/reading/session/d4dfd255-dab9-427f-9afc-a0adc1ea69db | 33 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 769 | 822 | 792 | 792 | 0.0039 | 0.0 | 715038 |
| logged-in-reader | desktop | reading | /vi/reading/session/d992eff1-e886-4839-a938-0a9a15bca51a | 35 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2869 | 718 | 861 | 788 | 788 | 0.0039 | 0.0 | 726954 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3057 | 716 | 1048 | 588 | 948 | 0.0039 | 0.0 | 631434 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3491 | 1435 | 1481 | 732 | 1388 | 0.0039 | 0.0 | 633462 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3081 | 782 | 1072 | 640 | 1016 | 0.0039 | 0.0 | 631587 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2776 | 713 | 765 | 620 | 1088 | 0.0039 | 0.0 | 633122 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2714 | 682 | 705 | 608 | 1144 | 0.0039 | 0.0 | 632907 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2698 | 666 | 687 | 592 | 1124 | 0.0039 | 0.0 | 633309 |
| logged-out | mobile | auth-public | /vi | 31 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3264 | 1252 | 1252 | 1104 | 1104 | 0.0000 | 0.0 | 601468 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8005 | 5992 | 5992 | 5424 | 5424 | 0.0000 | 0.0 | 512288 |
| logged-out | mobile | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7325 | 5311 | 5311 | 4488 | 4488 | 0.0000 | 0.0 | 512886 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4797 | 2653 | 2786 | 1528 | 1528 | 0.0000 | 0.0 | 511944 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 9609 | 7597 | 7598 | 6108 | 6108 | 0.0000 | 0.0 | 512031 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 12036 | 10020 | 10020 | 8948 | 8948 | 0.0000 | 0.0 | 512138 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 9117 | 6936 | 7104 | 6932 | 7256 | 0.0000 | 0.0 | 525829 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 15501 | 12348 | 13476 | 11296 | 11592 | 0.0000 | 0.0 | 525851 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 10923 | 8909 | 8909 | 7692 | 8016 | 0.0000 | 0.0 | 525938 |
| logged-in-admin | mobile | auth-public | /vi | 35 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6081 | 4070 | 4070 | 1204 | 3808 | 0.0024 | 0.0 | 607834 |
| logged-in-admin | mobile | auth-public | /vi/login | 58 | 1 | critical | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 17120 | 8093 | 8093 | 6120 | 7764 | 0.0024 | 0.0 | 1108021 |
| logged-in-admin | mobile | auth-public | /vi/register | 58 | 1 | critical | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 15667 | 6342 | 6808 | 6348 | 6700 | 0.0024 | 0.0 | 1107957 |
| logged-in-admin | mobile | auth-public | /vi/forgot-password | 52 | 7 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 13505 | 5498 | 5816 | 4996 | 5424 | 0.0024 | 0.0 | 1107975 |
| logged-in-admin | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6529 | 4518 | 4518 | 3564 | 3564 | 0.0000 | 0.0 | 512069 |
| logged-in-admin | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 9021 | 7012 | 7012 | 5796 | 5796 | 0.0000 | 0.0 | 512129 |
| logged-in-admin | mobile | reading | /vi/reading | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8942 | 6931 | 6931 | 4132 | 6928 | 0.0071 | 0.0 | 642706 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 35 | 6 | high | 0 | 0 | 0 | 0 | 5 | 1 | 4 | 5 | 0 | 0 | 3867 | 1621 | 1849 | 1028 | 1872 | 0.0071 | 0.0 | 661399 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 36 | 4 | critical | 0 | 0 | 1 | 0 | 5 | 0 | 3 | 5 | 0 | 0 | 5003 | 2989 | 2989 | 628 | 3000 | 0.0071 | 0.0 | 765160 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4316 | 2163 | 2307 | 1832 | 2360 | 0.0071 | 0.0 | 725867 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 32 | 23 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4274 | 1289 | 1308 | 872 | 1208 | 0.0000 | 0.0 | 646063 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 13918 | 5201 | 11907 | 4368 | 5340 | 0.0760 | 0.0 | 635731 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 31 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 11208 | 9192 | 9193 | 7492 | 9172 | 0.0071 | 0.0 | 631574 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 64 | 1 | critical | 0 | 0 | 4 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 32003 | 11246 | 18606 | 9912 | 11240 | 0.0760 | 0.0 | 1255768 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 31 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 10771 | 8760 | 8760 | 7092 | 8564 | 0.0071 | 0.0 | 633701 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 31 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6600 | 4579 | 4579 | 2852 | 4560 | 0.0071 | 0.0 | 631619 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6209 | 4198 | 4199 | 3696 | 4108 | 0.0071 | 0.0 | 649627 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 37 | 1 | critical | 0 | 0 | 2 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 11624 | 8783 | 8889 | 6504 | 8820 | 0.0071 | 0.0 | 713411 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 15128 | 12649 | 13119 | 10844 | 12644 | 0.0071 | 0.0 | 641940 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 10692 | 8683 | 8683 | 6764 | 8744 | 0.0071 | 0.0 | 634079 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 31 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 10255 | 8240 | 8240 | 5692 | 8200 | 0.0071 | 0.0 | 631480 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 11687 | 9260 | 9675 | 6440 | 9260 | 0.0071 | 0.0 | 632232 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 31 | 29 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 22654 | 0 | - | - | - | 0.0000 | 0.0 | 630959 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 17258 | 15246 | 15246 | 13012 | 15244 | 0.0074 | 0.0 | 632049 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 31 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 15373 | 13359 | 13359 | 11468 | 13028 | 0.0071 | 0.0 | 632437 |
| logged-in-admin | mobile | reading | /vi/reading/history | 31 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 16173 | 14153 | 14153 | 10848 | 14188 | 0.0071 | 0.0 | 632820 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 9075 | 7062 | 7062 | 4844 | 7060 | 0.0024 | 0.0 | 526120 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 12314 | 10300 | 10300 | 9944 | 10272 | 0.0000 | 0.0 | 526073 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 10564 | 8554 | 8554 | 7476 | 8540 | 0.0024 | 0.0 | 526331 |
| logged-in-admin | mobile | reading | /vi/reading/session/86a35fea-d5e7-4fb7-b98c-a69e7a0c7295 | 35 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 9192 | 7069 | 7180 | 5820 | 7032 | 0.0072 | 0.0 | 681677 |
| logged-in-admin | mobile | reading | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | 35 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8127 | 5973 | 6118 | 4288 | 5928 | 0.0072 | 0.0 | 681758 |
| logged-in-admin | mobile | reading | /vi/reading/session/ec6dc24c-47f3-4aa8-a3a4-6c589a3d56ce | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6797 | 4143 | 4788 | 3440 | 4132 | 0.0072 | 0.0 | 680936 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8206 | 6166 | 6191 | 5844 | 6160 | 0.0000 | 0.0 | 631017 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 30 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8722 | 6687 | 6707 | 6368 | 6716 | 0.0000 | 0.0 | 631149 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 9235 | 7218 | 7218 | 5208 | 7192 | 0.0071 | 0.0 | 631177 |
| logged-in-reader | mobile | auth-public | /vi | 35 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5856 | 2639 | 3847 | 2640 | 2988 | 0.0024 | 0.0 | 609441 |
| logged-in-reader | mobile | auth-public | /vi/login | 24 | 31 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4581 | 0 | - | 1308 | 1844 | 0.0000 | 0.0 | 511018 |
| logged-in-reader | mobile | auth-public | /vi/register | 24 | 31 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6017 | 0 | - | 1388 | 1388 | 0.0000 | 0.0 | 511121 |
| logged-in-reader | mobile | auth-public | /vi/forgot-password | 24 | 28 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4936 | 0 | - | - | - | 0.0000 | 0.0 | 511074 |
| logged-in-reader | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5219 | 2156 | 3211 | 1052 | 1052 | 0.0000 | 0.0 | 512123 |
| logged-in-reader | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5933 | 3441 | 3924 | 2232 | 2232 | 0.0000 | 0.0 | 512187 |
| logged-in-reader | mobile | reading | /vi/reading | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5537 | 2231 | 3527 | 1172 | 1660 | 0.0071 | 0.0 | 641789 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 35 | 6 | high | 0 | 0 | 0 | 0 | 5 | 0 | 5 | 5 | 0 | 0 | 5525 | 3140 | 3516 | 2516 | 2856 | 0.0071 | 0.0 | 661694 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 36 | 7 | critical | 0 | 0 | 1 | 0 | 5 | 0 | 5 | 5 | 0 | 0 | 6297 | 3773 | 4289 | 2236 | 2660 | 0.0071 | 0.0 | 798034 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5591 | 2762 | 3581 | 1248 | 1796 | 0.0071 | 0.0 | 724234 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 26 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7864 | 2728 | 3142 | 1452 | 1772 | 0.0071 | 0.0 | 642341 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 29 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6745 | 3201 | 4736 | 2096 | 2344 | 0.0892 | 0.0 | 634652 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6222 | 2350 | 4214 | 1064 | 1384 | 0.0071 | 0.0 | 631996 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5003 | 2515 | 2993 | 1596 | 1912 | 0.0071 | 0.0 | 632692 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8949 | 6934 | 6934 | 6936 | 6936 | 0.0000 | 0.0 | 634051 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6238 | 3696 | 4228 | 2944 | 2944 | 0.0071 | 0.0 | 631653 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6922 | 2950 | 4909 | 1736 | 2364 | 0.0267 | 0.0 | 650085 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 34 | 7 | high | 0 | 0 | 0 | 0 | 2 | 0 | 2 | 2 | 0 | 0 | 9830 | 3944 | 7037 | 1552 | 5320 | 0.0173 | 0.0 | 776677 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 31 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5695 | 2860 | 3682 | 1884 | 2204 | 0.0071 | 0.0 | 644334 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7231 | 2987 | 5223 | 1428 | 1760 | 0.0071 | 0.0 | 634340 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5383 | 2563 | 3374 | 1176 | 1504 | 0.0071 | 0.0 | 631626 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 11691 | 2687 | 9677 | 1220 | 1220 | 0.0071 | 0.0 | 631836 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6475 | 3548 | 4463 | 2572 | 3188 | 0.0401 | 0.0 | 633074 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7617 | 3474 | 5604 | 2180 | 2180 | 0.0074 | 0.0 | 632384 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6158 | 2857 | 4147 | 1628 | 1956 | 0.0071 | 0.0 | 632565 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5534 | 2263 | 3523 | 1880 | 2208 | 0.0071 | 0.0 | 633065 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6884 | 4583 | 4872 | 2668 | 2976 | 0.0055 | 0.0 | 526349 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6862 | 3288 | 4851 | 2348 | 2348 | 0.0055 | 0.0 | 526374 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 8926 | 6025 | 6915 | 4940 | 4940 | 0.0055 | 0.0 | 526261 |
| logged-in-reader | mobile | reading | /vi/reading/session/a866b146-3ba0-41d1-8a65-756089c5d3e2 | 30 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5962 | 2428 | 3950 | 1224 | 1548 | 0.0072 | 0.0 | 681184 |
| logged-in-reader | mobile | reading | /vi/reading/session/4e7197ef-946a-4777-94cf-573b2e43b73f | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6430 | 3640 | 4417 | 2672 | 3012 | 0.0072 | 0.0 | 681338 |
| logged-in-reader | mobile | reading | /vi/reading/session/f3e121ce-9dfe-45b4-8cbd-e370840a664e | 30 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6589 | 2831 | 4579 | 1540 | 1540 | 0.0072 | 0.0 | 681159 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5234 | 2641 | 3224 | 1580 | 1580 | 0.0071 | 0.0 | 631403 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7185 | 3023 | 5173 | 2984 | 3312 | 0.0071 | 0.0 | 631216 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6287 | 2704 | 4276 | 1056 | 1388 | 0.0071 | 0.0 | 631282 |

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
| logged-in-admin | desktop | auth-public | /vi | 39 | critical | 8 | 28 | 0 |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 53 | critical | 0 | 49 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 36 | critical | 1 | 33 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 40 | critical | 4 | 34 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 35 | high | 4 | 29 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 32 | high | 3 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 33 | high | 3 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 32 | high | 3 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 33 | high | 4 | 27 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 27 | high | 1 | 23 | 0 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 27 | high | 1 | 23 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/e26fd5ff-5ed0-47ee-92d0-ab217ba39696 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/ec621f38-5e41-4d73-b92f-da98bb21bdd0 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/7f250428-778b-47cf-98f4-71cdaf7c2a5f | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | auth-public | /vi/login | 53 | critical | 0 | 49 | 0 |
| logged-in-reader | desktop | auth-public | /vi/forgot-password | 53 | critical | 0 | 49 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 41 | critical | 5 | 34 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 35 | high | 0 | 33 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 30 | high | 1 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 36 | critical | 5 | 28 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 34 | high | 5 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 27 | high | 1 | 23 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/6b2c7f7e-ff4e-4711-a01f-f772d48b0642 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/d4dfd255-dab9-427f-9afc-a0adc1ea69db | 33 | high | 2 | 29 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/d992eff1-e886-4839-a938-0a9a15bca51a | 35 | high | 3 | 30 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 31 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 35 | high | 5 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi/login | 58 | critical | 5 | 49 | 0 |
| logged-in-admin | mobile | auth-public | /vi/register | 58 | critical | 5 | 49 | 0 |
| logged-in-admin | mobile | auth-public | /vi/forgot-password | 52 | critical | 0 | 49 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 36 | critical | 1 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 32 | high | 3 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 64 | critical | 7 | 53 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 37 | critical | 3 | 32 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 27 | high | 1 | 23 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/86a35fea-d5e7-4fb7-b98c-a69e7a0c7295 | 35 | high | 4 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | 35 | high | 4 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/ec6dc24c-47f3-4aa8-a3a4-6c589a3d56ce | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 35 | high | 5 | 27 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 36 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 34 | high | 0 | 32 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 31 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/a866b146-3ba0-41d1-8a65-756089c5d3e2 | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/4e7197ef-946a-4777-94cf-573b2e43b73f | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/f3e121ce-9dfe-45b4-8cbd-e370840a664e | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 15193 | 12886 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 14096 | 10688 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 13315 | 11257 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 12798 | 10445 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 12585 | 10723 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-out | mobile | auth-public | /vi/legal/privacy | GET | 200 | 12276 | 11116 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 11315 | 9681 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 11192 | 9579 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 10239 | 9800 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-out | mobile | auth-public | /vi/verify-email | GET | 200 | 9943 | 8811 | html | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 9209 | 6316 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 9126 | 7364 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 8866 | 7356 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 8842 | 7540 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 8737 | 6366 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 8713 | 6965 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 8634 | 6651 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 8503 | 7340 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 8158 | 5575 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 8044 | 6006 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | - | 8001 | - | api | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | - | 8000 | - | api | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 7927 | 7581 | api | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 7724 | 7720 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fui-avatars.com%2Fapi%2F%3Fbackground%3D111%26color%3Dfff%26name%3DLucifer&w=384&q=75 |
| logged-out | mobile | auth-public | /vi/reset-password | GET | 200 | 7528 | 5971 | html | https://www.tarotnow.xyz/vi/reset-password |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 7451 | 6032 | html | https://www.tarotnow.xyz/vi/login |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7157 | 5081 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 7098 | 6889 | api | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | GET | 200 | 7009 | 4680 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | mobile | reading | /vi/reading/session/86a35fea-d5e7-4fb7-b98c-a69e7a0c7295 | GET | 200 | 6995 | 5693 | html | https://www.tarotnow.xyz/vi/reading/session/86a35fea-d5e7-4fb7-b98c-a69e7a0c7295 |
| logged-in-admin | mobile | auth-public | /vi/register | GET | 200 | 6973 | 5671 | html | https://www.tarotnow.xyz/vi/register |
| logged-in-admin | mobile | auth-public | /vi/verify-email | GET | 200 | 6956 | 5406 | html | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 6928 | 6923 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fui-avatars.com%2Fapi%2F%3Fbackground%3D111%26color%3Dfff%26name%3DLucifer&w=384&q=75 |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 6875 | 4000 | html | https://www.tarotnow.xyz/vi/reading |
| logged-out | mobile | auth-public | /vi/legal/tos | GET | 200 | 6871 | 6793 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 6635 | 6257 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 6361 | 1832 | static | https://www.tarotnow.xyz/_next/static/chunks/05cb3wg4t5bkg.js |
| logged-in-admin | mobile | auth-public | /vi/register | GET | 200 | 6173 | 6119 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 6067 | 5152 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | mobile | auth-public | /vi/forgot-password | GET | 200 | 6047 | 5125 | html | https://www.tarotnow.xyz/vi/forgot-password |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | - | 6002 | - | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | - | 6001 | - | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | - | 6001 | - | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | - | 6000 | - | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | - | 6000 | - | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | - | 5999 | - | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | - | 5998 | - | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-out | mobile | auth-public | /vi/login | GET | 200 | 5923 | 5263 | html | https://www.tarotnow.xyz/vi/login |
| logged-in-admin | mobile | reading | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | GET | 200 | 5903 | 3530 | html | https://www.tarotnow.xyz/vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 |
| logged-in-admin | mobile | auth-public | /vi/forgot-password | GET | 200 | 5429 | 4880 | html | https://www.tarotnow.xyz/vi |
| logged-out | mobile | auth-public | /vi/register | GET | 200 | 5247 | 4290 | html | https://www.tarotnow.xyz/vi/register |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 5189 | 5184 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 5113 | 4120 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 4839 | 411 | static | https://www.tarotnow.xyz/_next/static/chunks/05cb3wg4t5bkg.js |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 4683 | 638 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 4618 | 934 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 4574 | 1977 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 4559 | 387 | static | https://www.tarotnow.xyz/_next/static/chunks/05cb3wg4t5bkg.js |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 4517 | 2732 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | mobile | auth-public | /vi/reset-password | GET | 200 | 4465 | 3437 | html | https://www.tarotnow.xyz/vi/reset-password |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 4343 | 2450 | static | https://www.tarotnow.xyz/_next/static/chunks/0i4ja86d8ahgq.js |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 4279 | 4153 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 4269 | 389 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 4175 | 396 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 4148 | 2518 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 4144 | 3558 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 4127 | 427 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | reading | /vi/reading/session/ec6dc24c-47f3-4aa8-a3a4-6c589a3d56ce | GET | 200 | 4094 | 3290 | html | https://www.tarotnow.xyz/vi/reading/session/ec6dc24c-47f3-4aa8-a3a4-6c589a3d56ce |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 4082 | 1940 | static | https://www.tarotnow.xyz/_next/static/chunks/0i4ja86d8ahgq.js |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 4053 | 1048 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-admin | mobile | auth-public | /vi | GET | 200 | 4008 | 953 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | reading | /vi/reading/session/f3e121ce-9dfe-45b4-8cbd-e370840a664e | GET | 200 | 3891 | 1300 | static | https://www.tarotnow.xyz/_next/static/chunks/05cb3wg4t5bkg.js |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 3865 | 3055 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 3853 | 643 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 3848 | 2630 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-out | desktop | auth-public | /vi/reset-password | GET | 200 | 3770 | 1051 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | reading | /vi/reading/session/4e7197ef-946a-4777-94cf-573b2e43b73f | GET | 200 | 3755 | 1581 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | reading | /vi/reading/session/4e7197ef-946a-4777-94cf-573b2e43b73f | GET | 200 | 3736 | 788 | static | https://www.tarotnow.xyz/_next/static/chunks/0i4ja86d8ahgq.js |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 3703 | 1225 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 3681 | 407 | static | https://www.tarotnow.xyz/_next/static/chunks/0i4ja86d8ahgq.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | auth-public | /vi/verify-email | GET | 200 | 800 | 404 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | auth-public | /vi/login | GET | 200 | 799 | 587 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | auth-public | /vi/forgot-password | GET | 200 | 798 | 632 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 797 | 321 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 796 | 152 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 796 | 352 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 796 | 792 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 794 | 224 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 794 | 449 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 793 | 342 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | auth-public | /vi/reset-password | GET | 200 | 793 | 394 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 792 | 117 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 791 | 117 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 790 | 341 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | auth-public | /vi/verify-email | GET | 200 | 789 | 323 | html | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 789 | 345 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 789 | 325 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | reading | /vi/reading/session/f3e121ce-9dfe-45b4-8cbd-e370840a664e | GET | 200 | 789 | 772 | static | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| logged-in-reader | desktop | auth-public | /vi/reset-password | GET | 200 | 786 | 319 | html | https://www.tarotnow.xyz/vi/reset-password |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 785 | 781 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | auth-public | /vi | GET | 200 | 783 | 346 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | auth-public | /vi/reset-password | GET | 200 | 783 | 391 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 782 | 664 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 781 | 244 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 780 | 770 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 779 | 319 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 779 | 674 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | auth-public | /vi/reset-password | GET | 200 | 779 | 368 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 778 | 394 | static | https://www.tarotnow.xyz/_next/static/chunks/05cb3wg4t5bkg.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 777 | 400 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 776 | 346 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | reading | /vi/reading/session/f3e121ce-9dfe-45b4-8cbd-e370840a664e | GET | 200 | 776 | 376 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 775 | 316 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 775 | 376 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 774 | 345 | static | https://www.tarotnow.xyz/_next/static/chunks/0i4ja86d8ahgq.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 774 | 409 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 774 | 385 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | GET | 200 | 773 | 330 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 770 | 576 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 769 | 362 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 765 | 496 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | auth-public | /vi/register | GET | 200 | 765 | 381 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | reading | /vi/reading/session/f3e121ce-9dfe-45b4-8cbd-e370840a664e | GET | 200 | 763 | 367 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 762 | 367 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 761 | 756 | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 760 | 335 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 758 | 373 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-out | desktop | auth-public | /vi/reset-password | GET | 200 | 757 | 420 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 757 | 342 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 757 | 386 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 756 | 729 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 756 | 343 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-reader | desktop | auth-public | /vi/login | GET | 200 | 755 | 341 | html | https://www.tarotnow.xyz/vi/login |
| logged-in-reader | mobile | auth-public | /vi/login | GET | 200 | 755 | 589 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | auth-public | /vi/login | GET | 200 | 755 | 401 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-reader | mobile | reader-chat | /vi/chat | GET | 200 | 754 | 377 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 754 | 375 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 753 | 709 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | reading | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | GET | 200 | 753 | 699 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 752 | 363 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 752 | 734 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 752 | 736 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 750 | 741 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 749 | 734 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 748 | 329 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-out | desktop | auth-public | /vi/legal/tos | GET | 200 | 747 | 351 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 745 | 709 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 744 | 741 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 743 | 727 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | auth-public | /vi/login | GET | 200 | 743 | 598 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 742 | 353 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | mobile | reading | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | GET | 200 | 741 | 698 | static | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 740 | 328 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | desktop | auth-public | /vi/forgot-password | GET | 200 | 739 | 339 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | GET | 200 | 738 | 248 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | reading | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | GET | 200 | 738 | 698 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | reading | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | GET | 200 | 738 | 162 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | reading | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | GET | 200 | 737 | 729 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-reader | desktop | reader-chat | /vi/chat | GET | 200 | 736 | 317 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 734 | 80 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-out | desktop | /vi | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-out | mobile | /vi | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-out | mobile | /vi | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| logged-out | mobile | /vi/register | https://www.tarotnow.xyz/api/legal/runtime-policies |
| logged-in-admin | mobile | /vi | https://www.tarotnow.xyz/api/wallet/balance |
| logged-in-admin | mobile | /vi | https://www.tarotnow.xyz/api/chat/unread-count |
| logged-in-admin | mobile | /vi | https://www.tarotnow.xyz/api/notifications/unread-count |
| logged-in-admin | mobile | /vi | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/api/wallet/balance |
| logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/api/chat/unread-count |
| logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/api/notifications/unread-count |
| logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/login | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/wallet/balance |
| logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/chat/unread-count |
| logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/notifications/unread-count |
| logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/register | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/wallet/balance |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/chat/unread-count |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/notifications/unread-count |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | /vi/gacha | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| logged-in-admin | mobile | /vi/gacha | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=256&q=75 |
| logged-in-admin | mobile | /vi/gacha | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/gacha | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/gacha | https://www.tarotnow.xyz/api/gacha/history?page=1&pageSize=6 |
| logged-in-admin | mobile | /vi/gacha/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | /vi/gacha/history | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F01_The_Fool_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F04_The_Empress_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F05_The_Emperor_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F06_The_Hierophant_50_20260325_181348.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
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
| logged-in-admin | mobile | /vi/profile/mfa | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/profile/mfa | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/profile/mfa | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/readers | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/readers | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/readers | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/chat | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/chat | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/chat | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/leaderboard | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/leaderboard | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/community | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/community | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/community | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/community | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | mobile | /vi/gamification | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/gamification | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/gamification | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/wallet/deposit | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/wallet/deposit | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/wallet/deposit | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/wallet/deposit/history | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/wallet/deposit/history | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0i4ja86d8ahgq.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/05cb3wg4t5bkg.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | /vi/notifications | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/notifications | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/notifications | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/reader/apply | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/reader/apply | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/reader/apply | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/reading/history | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/reading/history | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/reading/history | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/legal/tos | https://www.tarotnow.xyz/api/chat/unread-count |
| logged-in-admin | mobile | /vi/legal/tos | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | mobile | /vi/legal/privacy | https://www.tarotnow.xyz/api/chat/unread-count |
| logged-in-admin | mobile | /vi/legal/privacy | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | mobile | /vi/legal/ai-disclaimer | https://www.tarotnow.xyz/api/chat/unread-count |
| logged-in-admin | mobile | /vi/legal/ai-disclaimer | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | mobile | /vi/reading/session/86a35fea-d5e7-4fb7-b98c-a69e7a0c7295 | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/reading/session/86a35fea-d5e7-4fb7-b98c-a69e7a0c7295 | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/reading/session/86a35fea-d5e7-4fb7-b98c-a69e7a0c7295 | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/reading/session/86a35fea-d5e7-4fb7-b98c-a69e7a0c7295 | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-admin | mobile | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/reading/session/ab240530-2a80-426e-825d-881185492fa6 | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-admin | mobile | /vi/reading/session/ec6dc24c-47f3-4aa8-a3a4-6c589a3d56ce | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/reading/session/ec6dc24c-47f3-4aa8-a3a4-6c589a3d56ce | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/reading/session/ec6dc24c-47f3-4aa8-a3a4-6c589a3d56ce | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/api/chat/unread-count |
| logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/api/notifications/unread-count |
| logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0i4ja86d8ahgq.js |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0rm0_6_wsunhe.js |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/0_yn3f1mymbbi.js |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-reader | mobile | /vi/login | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| logged-in-reader | mobile | /vi/login | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0i4ja86d8ahgq.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0rm0_6_wsunhe.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/0_yn3f1mymbbi.js |
| logged-in-reader | mobile | /vi/register | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-reader | mobile | /vi/register | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0i4ja86d8ahgq.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0rm0_6_wsunhe.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/0_yn3f1mymbbi.js |
| logged-in-reader | mobile | /vi/forgot-password | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-reader | mobile | /vi/forgot-password | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-reader | mobile | /vi/reading | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-reader | mobile | /vi/gacha | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/gacha | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/gacha/history | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-reader | mobile | /vi/gacha/history | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F09_Strength_50_20260325_181351.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F10_The_Hermit_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-reader | mobile | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F11_Wheel_of+_Fortune_50_20260325_181353.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-reader | mobile | /vi/profile | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | /vi/profile | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/profile | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/profile/reader | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-reader | mobile | /vi/profile/reader | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-reader | mobile | /vi/chat | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-reader | mobile | /vi/leaderboard | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/leaderboard | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/community | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/wallet | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-reader | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-reader | mobile | /vi/wallet/deposit/history | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-reader | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-reader | mobile | /vi/notifications | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-reader | mobile | /vi/reader/apply | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/reader/apply | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/reading/history | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-reader | mobile | /vi/reading/session/a866b146-3ba0-41d1-8a65-756089c5d3e2 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/reading/session/a866b146-3ba0-41d1-8a65-756089c5d3e2 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/reading/session/4e7197ef-946a-4777-94cf-573b2e43b73f | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-reader | mobile | /vi/reading/session/f3e121ce-9dfe-45b4-8cbd-e370840a664e | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | https://www.tarotnow.xyz/_next/static/chunks/0hen0b13nlrzm.js |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

## Coverage Notes
| Scenario | Viewport | Note |
| --- | --- | --- |
| logged-out | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-out | desktop | dynamic-routes: skipped for logged-out scenario. |
| logged-out | desktop | scenario-filter:logged-out-protected-routes-skipped=20 |
| logged-in-admin | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | desktop | reading.init.daily_1: blocked (400). |
| logged-in-admin | desktop | reading.init.spread_3: created e26fd5ff-5ed0-47ee-92d0-ab217ba39696. |
| logged-in-admin | desktop | reading.init.spread_5: created ec621f38-5e41-4d73-b92f-da98bb21bdd0. |
| logged-in-admin | desktop | reading.init.spread_10: created 7f250428-778b-47cf-98f4-71cdaf7c2a5f. |
| logged-in-admin | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | desktop | reader-detail:ui-discovery-empty |
| logged-in-admin | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-admin | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-admin | desktop | community-posts:api-discovery-1 |
| logged-in-admin | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | desktop | reading.init.daily_1: blocked (400). |
| logged-in-reader | desktop | reading.init.spread_3: created 6b2c7f7e-ff4e-4711-a01f-f772d48b0642. |
| logged-in-reader | desktop | reading.init.spread_5: created d4dfd255-dab9-427f-9afc-a0adc1ea69db. |
| logged-in-reader | desktop | reading.init.spread_10: created d992eff1-e886-4839-a938-0a9a15bca51a. |
| logged-in-reader | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | desktop | reader-detail:ui-discovery-empty |
| logged-in-reader | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-reader | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-reader | desktop | community-posts:api-discovery-1 |
| logged-in-reader | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | desktop | scenario-filter:reader-admin-routes-skipped=0 |
| logged-out | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-out | mobile | dynamic-routes: skipped for logged-out scenario. |
| logged-out | mobile | scenario-filter:logged-out-protected-routes-skipped=20 |
| logged-in-admin | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | mobile | reading.init.daily_1: blocked (400). |
| logged-in-admin | mobile | reading.init.spread_3: created 86a35fea-d5e7-4fb7-b98c-a69e7a0c7295. |
| logged-in-admin | mobile | reading.init.spread_5: created ab240530-2a80-426e-825d-881185492fa6. |
| logged-in-admin | mobile | reading.init.spread_10: created ec6dc24c-47f3-4aa8-a3a4-6c589a3d56ce. |
| logged-in-admin | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | mobile | reader-detail:ui-discovery-empty |
| logged-in-admin | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-admin | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-admin | mobile | community-posts:api-discovery-1 |
| logged-in-admin | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | mobile | reading.init.daily_1: blocked (400). |
| logged-in-reader | mobile | reading.init.spread_3: created a866b146-3ba0-41d1-8a65-756089c5d3e2. |
| logged-in-reader | mobile | reading.init.spread_5: created 4e7197ef-946a-4777-94cf-573b2e43b73f. |
| logged-in-reader | mobile | reading.init.spread_10: created f3e121ce-9dfe-45b4-8cbd-e370840a664e. |
| logged-in-reader | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | mobile | reader-detail:ui-discovery-empty |
| logged-in-reader | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-reader | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-reader | mobile | community-posts:api-discovery-1 |
| logged-in-reader | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | mobile | scenario-filter:reader-admin-routes-skipped=0 |

## Login Bootstrap Notes
### logged-in-admin / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.

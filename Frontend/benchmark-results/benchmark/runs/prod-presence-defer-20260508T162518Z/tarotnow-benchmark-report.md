# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T16:39:11.304Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 18
- High pages (request count): 154
- High slow requests: 203
- Medium slow requests: 448

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 3824 | 224 | 1 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 48 | 3572 | 1468 | 20 | 0 | 16 | 0 | yes |
| logged-in-reader | desktop | 38 | 3411 | 1155 | 1 | 0 | 17 | 0 | yes |
| logged-out | mobile | 9 | 3356 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 48 | 3572 | 1451 | 8 | 0 | 17 | 0 | yes |
| logged-in-reader | mobile | 38 | 3398 | 1177 | 1 | 0 | 16 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 3586 | 0 |
| logged-in-admin | desktop | auth | 5 | 37.0 | 4031 | 3 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 3058 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6655 | 1 |
| logged-in-admin | desktop | community | 1 | 36.0 | 4786 | 0 |
| logged-in-admin | desktop | gacha | 2 | 33.5 | 3734 | 0 |
| logged-in-admin | desktop | gamification | 1 | 31.0 | 3228 | 0 |
| logged-in-admin | desktop | home | 1 | 35.0 | 3267 | 0 |
| logged-in-admin | desktop | inventory | 1 | 39.0 | 4377 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 3168 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.7 | 2985 | 0 |
| logged-in-admin | desktop | notifications | 1 | 33.0 | 5977 | 0 |
| logged-in-admin | desktop | profile | 3 | 30.0 | 3680 | 0 |
| logged-in-admin | desktop | reader | 1 | 33.0 | 3466 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.0 | 3221 | 0 |
| logged-in-admin | desktop | reading | 5 | 30.2 | 3070 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.0 | 3023 | 16 |
| logged-in-admin | mobile | admin | 10 | 29.6 | 3155 | 0 |
| logged-in-admin | mobile | auth | 5 | 37.0 | 5362 | 6 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2867 | 0 |
| logged-in-admin | mobile | collection | 1 | 31.0 | 5958 | 0 |
| logged-in-admin | mobile | community | 1 | 30.0 | 3454 | 0 |
| logged-in-admin | mobile | gacha | 2 | 35.5 | 3701 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 3052 | 0 |
| logged-in-admin | mobile | home | 1 | 29.0 | 3401 | 0 |
| logged-in-admin | mobile | inventory | 1 | 35.0 | 3461 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 3413 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 3078 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2926 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.3 | 3089 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2885 | 0 |
| logged-in-admin | mobile | readers | 7 | 29.9 | 3790 | 2 |
| logged-in-admin | mobile | reading | 5 | 28.8 | 3268 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.8 | 3229 | 0 |
| logged-in-reader | desktop | auth | 5 | 35.0 | 3511 | 1 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2803 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6501 | 0 |
| logged-in-reader | desktop | community | 1 | 34.0 | 4045 | 0 |
| logged-in-reader | desktop | gacha | 2 | 34.5 | 3553 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2864 | 0 |
| logged-in-reader | desktop | home | 1 | 29.0 | 3066 | 0 |
| logged-in-reader | desktop | inventory | 1 | 37.0 | 3624 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 5454 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.7 | 3241 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2851 | 0 |
| logged-in-reader | desktop | profile | 3 | 30.0 | 3168 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2828 | 0 |
| logged-in-reader | desktop | readers | 7 | 29.1 | 3533 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.8 | 2958 | 0 |
| logged-in-reader | desktop | wallet | 4 | 29.8 | 3046 | 0 |
| logged-in-reader | mobile | auth | 5 | 40.8 | 3680 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2698 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5774 | 0 |
| logged-in-reader | mobile | community | 1 | 36.0 | 3611 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.5 | 3557 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 3772 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2707 | 0 |
| logged-in-reader | mobile | inventory | 1 | 35.0 | 3082 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2880 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.7 | 3423 | 0 |
| logged-in-reader | mobile | notifications | 1 | 33.0 | 4542 | 0 |
| logged-in-reader | mobile | profile | 3 | 30.7 | 3144 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2927 | 0 |
| logged-in-reader | mobile | readers | 7 | 29.0 | 3169 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.0 | 3131 | 1 |
| logged-in-reader | mobile | wallet | 4 | 28.8 | 3517 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 3761 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4168 | 1 |
| logged-out | desktop | legal | 3 | 25.0 | 3813 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 3574 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 3248 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 3027 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4168 | 1529 | 2078 | 1536 | 1536 | 0.0000 | 493.0 | 601425 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2921 | 688 | 910 | 700 | 700 | 0.0000 | 0.0 | 512332 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5162 | 817 | 3139 | 632 | 632 | 0.0000 | 0.0 | 512983 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3695 | 780 | 1689 | 664 | 664 | 0.0000 | 0.0 | 511984 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2738 | 730 | 730 | 608 | 608 | 0.0000 | 0.0 | 512106 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4291 | 694 | 2283 | 608 | 608 | 0.0000 | 0.0 | 512197 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2763 | 753 | 753 | 680 | 680 | 0.0000 | 0.0 | 526041 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3505 | 699 | 1494 | 652 | 652 | 0.0000 | 0.0 | 526055 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5171 | 744 | 3165 | 736 | 736 | 0.0000 | 0.0 | 525995 |
| logged-in-admin | desktop | auth-public | /vi | 35 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3267 | 998 | 1251 | 696 | 1332 | 0.0035 | 261.0 | 613329 |
| logged-in-admin | desktop | auth-public | /vi/login | 62 | 2 | critical | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4806 | 777 | 2064 | 644 | 1320 | 0.0035 | 402.0 | 1123508 |
| logged-in-admin | desktop | auth-public | /vi/register | 24 | 41 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3382 | 848 | 1277 | 692 | 1248 | 0.0035 | 41.0 | 510982 |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 51 | 13 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4144 | 863 | 863 | 780 | 1404 | 0.0035 | 408.0 | 1048847 |
| logged-in-admin | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3437 | 881 | 1424 | 736 | 736 | 0.0000 | 0.0 | 512080 |
| logged-in-admin | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4384 | 680 | 2361 | 656 | 656 | 0.0000 | 0.0 | 512194 |
| logged-in-admin | desktop | reading | /vi/reading | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2870 | 851 | 864 | 640 | 1048 | 0.0042 | 0.0 | 641806 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 39 | 2 | critical | 0 | 0 | 1 | 0 | 5 | 2 | 3 | 5 | 0 | 0 | 4377 | 828 | 2369 | 788 | 1088 | 0.0042 | 0.0 | 664381 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 35 | 6 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3813 | 1292 | 1806 | 1080 | 1508 | 0.0042 | 0.0 | 732696 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3655 | 873 | 1640 | 764 | 1188 | 0.0042 | 0.0 | 728593 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6655 | 753 | 760 | 608 | 608 | 0.0043 | 15.0 | 643325 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 34 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4266 | 867 | 2257 | 608 | 1044 | 0.0489 | 0.0 | 647428 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3309 | 1250 | 1303 | 1232 | 1596 | 0.0042 | 0.0 | 631806 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 40 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3465 | 865 | 1842 | 628 | 1048 | 0.0489 | 0.0 | 630950 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3106 | 906 | 1100 | 856 | 1364 | 0.0042 | 0.0 | 633940 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3058 | 720 | 1032 | 612 | 1020 | 0.0042 | 0.0 | 632044 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3168 | 719 | 1160 | 644 | 976 | 0.0180 | 0.0 | 652356 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 36 | 3 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 4786 | 775 | 1985 | 608 | 2064 | 0.0042 | 23.0 | 778787 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3228 | 850 | 1212 | 664 | 1032 | 0.0432 | 0.0 | 644725 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3307 | 914 | 1238 | 612 | 1088 | 0.0042 | 0.0 | 634314 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2725 | 698 | 708 | 620 | 1008 | 0.0042 | 0.0 | 631677 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3065 | 781 | 1053 | 672 | 1264 | 0.0042 | 0.0 | 632213 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 34 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2994 | 1818 | - | - | - | 0.0000 | 55.0 | 631211 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 33 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5977 | 781 | 3964 | 636 | 1092 | 0.0042 | 0.0 | 644875 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 33 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3466 | 744 | 1416 | 608 | 992 | 0.0042 | 0.0 | 645610 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3236 | 806 | 1222 | 616 | 1096 | 0.0042 | 0.0 | 633083 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3053 | 682 | 1043 | 612 | 952 | 0.0020 | 0.0 | 527940 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2848 | 840 | 840 | 812 | 1120 | 0.0020 | 0.0 | 526166 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3055 | 683 | 1046 | 596 | 908 | 0.0020 | 0.0 | 526450 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3221 | 855 | 1208 | 664 | 1060 | 0.0000 | 0.0 | 647730 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2794 | 784 | 785 | 632 | 1172 | 0.0000 | 0.0 | 647770 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 32 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5666 | 844 | 3657 | 668 | 1004 | 0.0000 | 0.0 | 650098 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 31 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3757 | 787 | 1667 | 612 | 956 | 0.0022 | 0.0 | 696750 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3002 | 784 | 784 | 636 | 968 | 0.0000 | 0.0 | 644708 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4319 | 920 | 2297 | 692 | 1032 | 0.0000 | 0.0 | 646580 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4131 | 887 | 2099 | 636 | 1028 | 0.0000 | 0.0 | 649054 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2930 | 788 | 794 | 628 | 1096 | 0.0000 | 0.0 | 655762 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3115 | 837 | 1029 | 668 | 1040 | 0.0000 | 0.0 | 650083 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2926 | 876 | 882 | 624 | 920 | 0.0000 | 0.0 | 646236 |
| logged-in-admin | desktop | reading | /vi/reading/session/f828c893-4746-46f5-8e08-7de94cd84f35 | 31 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3338 | 891 | 1232 | 664 | 1100 | 0.0042 | 0.0 | 712886 |
| logged-in-admin | desktop | reading | /vi/reading/session/4cedf788-12bb-4123-9073-992b264c96c2 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2843 | 814 | 818 | 772 | 1172 | 0.0042 | 0.0 | 632353 |
| logged-in-admin | desktop | reading | /vi/reading/session/60b1af90-19b3-4b78-80c3-b720c958cb1a | 36 | 3 | critical | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3062 | 835 | 1053 | 628 | 1024 | 0.0042 | 0.0 | 727762 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3040 | 752 | 1028 | 628 | 996 | 0.0042 | 0.0 | 631269 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3133 | 738 | 1125 | 620 | 1044 | 0.0042 | 0.0 | 631319 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3013 | 776 | 1006 | 860 | 860 | 0.0042 | 0.0 | 631485 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2871 | 852 | 863 | 616 | 1212 | 0.0042 | 0.0 | 632889 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3063 | 830 | 1054 | 932 | 932 | 0.0042 | 0.0 | 632980 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4323 | 759 | 2311 | 624 | 1000 | 0.0042 | 0.0 | 633135 |
| logged-in-reader | desktop | auth-public | /vi | 29 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3066 | 820 | 1052 | 1152 | 1152 | 0.0038 | 95.0 | 607859 |
| logged-in-reader | desktop | auth-public | /vi/login | 53 | 10 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4964 | 719 | 2165 | 700 | 1348 | 0.0038 | 437.0 | 1108154 |
| logged-in-reader | desktop | auth-public | /vi/register | 24 | 41 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3077 | 802 | 1826 | 684 | 1308 | 0.0038 | 432.0 | 511028 |
| logged-in-reader | desktop | auth-public | /vi/forgot-password | 50 | 13 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3500 | 775 | 775 | 684 | 1320 | 0.0033 | 267.0 | 1037857 |
| logged-in-reader | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3171 | 776 | 1163 | 676 | 676 | 0.0000 | 0.0 | 512062 |
| logged-in-reader | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2845 | 825 | 826 | 716 | 716 | 0.0001 | 0.0 | 512136 |
| logged-in-reader | desktop | reading | /vi/reading | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2885 | 834 | 861 | 636 | 1112 | 0.0039 | 0.0 | 641693 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 37 | 4 | critical | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3624 | 940 | 1611 | 640 | 1332 | 0.0039 | 0.0 | 654920 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 37 | 5 | critical | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3693 | 835 | 1682 | 668 | 1056 | 0.0039 | 0.0 | 735352 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3413 | 842 | 1401 | 588 | 992 | 0.0039 | 0.0 | 726955 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 32 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6501 | 772 | 791 | 636 | 636 | 0.0040 | 0.0 | 642397 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3286 | 1013 | 1265 | 616 | 1088 | 0.0726 | 0.0 | 639095 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3034 | 905 | 1024 | 900 | 1260 | 0.0039 | 0.0 | 632773 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3184 | 757 | 1173 | 596 | 1012 | 0.0039 | 0.0 | 632657 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4867 | 875 | 2852 | 676 | 1136 | 0.0039 | 0.0 | 634207 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2803 | 754 | 786 | 632 | 1040 | 0.0039 | 0.0 | 631966 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5454 | 1130 | 3429 | 1308 | 1308 | 0.0177 | 0.0 | 650016 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 34 | 5 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 4045 | 785 | 1247 | 644 | 1804 | 0.0039 | 0.0 | 776557 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 845 | 845 | 692 | 1092 | 0.0430 | 0.0 | 642151 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2928 | 897 | 918 | 696 | 1240 | 0.0039 | 0.0 | 634288 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3103 | 805 | 1093 | 656 | 1040 | 0.0039 | 0.0 | 634874 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3102 | 797 | 1093 | 680 | 1076 | 0.0039 | 0.0 | 632057 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3051 | 878 | 1035 | 644 | 1080 | 0.0095 | 0.0 | 637842 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2851 | 841 | 841 | 608 | 1060 | 0.0044 | 0.0 | 632336 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2828 | 788 | 813 | 696 | 1120 | 0.0039 | 0.0 | 632774 |
| logged-in-reader | desktop | reading | /vi/reading/history | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3003 | 967 | 985 | 756 | 1316 | 0.0039 | 0.0 | 633979 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2777 | 764 | 764 | 652 | 956 | 0.0019 | 0.0 | 526474 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4174 | 800 | 2163 | 660 | 992 | 0.0019 | 0.0 | 528064 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2773 | 762 | 762 | 600 | 952 | 0.0019 | 0.0 | 526558 |
| logged-in-reader | desktop | reading | /vi/reading/session/e4197b22-32f1-4a80-9529-3f4c62e3a202 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2903 | 868 | 892 | 668 | 1080 | 0.0043 | 0.0 | 632462 |
| logged-in-reader | desktop | reading | /vi/reading/session/2385fd1c-641f-41a2-9aa0-2216f893cc49 | 36 | 3 | critical | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3165 | 763 | 1154 | 668 | 1064 | 0.0039 | 0.0 | 727645 |
| logged-in-reader | desktop | reading | /vi/reading/session/fa2fc343-c413-437a-b088-9b30ecb4fcd3 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2836 | 808 | 824 | 640 | 1032 | 0.0039 | 0.0 | 632581 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3090 | 697 | 1083 | 620 | 1008 | 0.0039 | 0.0 | 634440 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2863 | 823 | 852 | 600 | 1016 | 0.0039 | 0.0 | 632294 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4731 | 721 | 2722 | 624 | 992 | 0.0039 | 0.0 | 631655 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 32 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3683 | 657 | 1672 | 608 | 1012 | 0.0039 | 0.0 | 644925 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2740 | 659 | 687 | 628 | 1224 | 0.0039 | 0.0 | 633235 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2758 | 713 | 745 | 628 | 1188 | 0.0039 | 0.0 | 633379 |
| logged-out | mobile | auth-public | /vi | 29 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3248 | 1236 | 1237 | 996 | 996 | 0.0000 | 0.0 | 601217 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3492 | 713 | 1480 | 572 | 572 | 0.0000 | 0.0 | 512341 |
| logged-out | mobile | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3194 | 700 | 1187 | 576 | 576 | 0.0000 | 0.0 | 512972 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3933 | 683 | 1925 | 564 | 564 | 0.0000 | 0.0 | 511969 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3591 | 721 | 1584 | 544 | 544 | 0.0000 | 0.0 | 512212 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3662 | 674 | 1652 | 576 | 576 | 0.0000 | 0.0 | 512185 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 716 | 716 | 612 | 612 | 0.0000 | 0.0 | 525970 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3061 | 671 | 1052 | 692 | 692 | 0.0000 | 0.0 | 525968 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3290 | 769 | 1278 | 588 | 588 | 0.0000 | 0.0 | 526046 |
| logged-in-admin | mobile | auth-public | /vi | 29 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3401 | 1141 | 1393 | 720 | 1092 | 0.0055 | 0.0 | 607837 |
| logged-in-admin | mobile | auth-public | /vi/login | 53 | 10 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 11637 | 4001 | 7437 | 3164 | 3164 | 0.0439 | 0.0 | 1108086 |
| logged-in-admin | mobile | auth-public | /vi/register | 60 | 4 | critical | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4310 | 958 | 1333 | 836 | 1200 | 0.0055 | 0.0 | 1107198 |
| logged-in-admin | mobile | auth-public | /vi/forgot-password | 24 | 38 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3388 | 705 | 1606 | 564 | 924 | 0.0024 | 0.0 | 511075 |
| logged-in-admin | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3587 | 916 | 1578 | 872 | 872 | 0.0000 | 0.0 | 512109 |
| logged-in-admin | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3890 | 1534 | 1881 | 1456 | 1456 | 0.0000 | 0.0 | 512208 |
| logged-in-admin | mobile | reading | /vi/reading | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3308 | 1275 | 1288 | 1044 | 1388 | 0.0000 | 0.0 | 641723 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 35 | 5 | high | 0 | 0 | 0 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 3461 | 994 | 1449 | 716 | 1068 | 0.0071 | 0.0 | 661424 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 39 | 3 | critical | 0 | 0 | 2 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 3986 | 1003 | 1978 | 680 | 1032 | 0.0071 | 0.0 | 801997 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3416 | 1120 | 1406 | 772 | 1128 | 0.0071 | 0.0 | 728550 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 31 | 36 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5958 | 1024 | 1055 | 704 | 1052 | 0.0000 | 0.0 | 645248 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3384 | 1232 | 1373 | 956 | 1300 | 0.0000 | 0.0 | 637429 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3060 | 749 | 1051 | 676 | 1016 | 0.0000 | 0.0 | 631543 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 41 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2824 | 886 | 1316 | 796 | 1120 | 0.0760 | 0.0 | 630885 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3823 | 919 | 1814 | 612 | 952 | 0.0071 | 0.0 | 633764 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 848 | 855 | 724 | 1052 | 0.0000 | 0.0 | 631767 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3413 | 656 | 1404 | 540 | 868 | 0.0267 | 0.0 | 649718 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3454 | 661 | 718 | 532 | 1876 | 0.0051 | 0.0 | 642945 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3052 | 1037 | 1037 | 760 | 1096 | 0.0000 | 0.0 | 642013 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3003 | 992 | 992 | 780 | 1204 | 0.0000 | 0.0 | 634051 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3262 | 952 | 1251 | 812 | 1148 | 0.0000 | 0.0 | 634796 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3556 | 913 | 1543 | 752 | 1092 | 0.0071 | 0.0 | 632258 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3095 | 851 | 851 | 712 | 1052 | 0.0071 | 0.0 | 631115 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2926 | 909 | 917 | 872 | 872 | 0.0000 | 0.0 | 631940 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2885 | 860 | 872 | 760 | 1096 | 0.0000 | 0.0 | 632636 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2813 | 746 | 805 | 620 | 952 | 0.0000 | 0.0 | 632918 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2859 | 848 | 848 | 752 | 1072 | 0.0032 | 0.0 | 526101 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3348 | 1166 | 1332 | 672 | 1008 | 0.0055 | 0.0 | 526136 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3027 | 1013 | 1013 | 900 | 1212 | 0.0032 | 0.0 | 526258 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2848 | 820 | 836 | 644 | 980 | 0.0000 | 0.0 | 647586 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 31 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4209 | 938 | 2200 | 728 | 1056 | 0.0000 | 0.0 | 650228 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3292 | 1270 | 1281 | 1056 | 1360 | 0.0000 | 0.0 | 645911 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 30 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2942 | 922 | 925 | 708 | 1020 | 0.0000 | 0.0 | 664709 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2856 | 839 | 845 | 636 | 944 | 0.0000 | 0.0 | 644577 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 31 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3417 | 1013 | 1408 | 900 | 1212 | 0.0000 | 0.0 | 649102 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2981 | 959 | 966 | 792 | 1128 | 0.0000 | 0.0 | 648730 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2926 | 913 | 914 | 680 | 1060 | 0.0000 | 0.0 | 655641 |
| logged-in-admin | mobile | admin | /vi/admin/users | 32 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3259 | 946 | 1243 | 868 | 1196 | 0.0000 | 0.0 | 652453 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2824 | 773 | 793 | 744 | 744 | 0.0000 | 0.0 | 646112 |
| logged-in-admin | mobile | reading | /vi/reading/session/4d68762d-32b0-4770-9e25-bfc9d9cb7e62 | 30 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3397 | 910 | 1384 | 780 | 1120 | 0.0071 | 0.0 | 680962 |
| logged-in-admin | mobile | reading | /vi/reading/session/a27600a5-fbc0-4d42-acec-89870a2009c6 | 30 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3848 | 792 | 1834 | 584 | 920 | 0.0071 | 0.0 | 681117 |
| logged-in-admin | mobile | reading | /vi/reading/session/9ad599f4-5a77-4a9e-91df-11084765bf37 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2976 | 922 | 933 | 692 | 1028 | 0.0000 | 0.0 | 632351 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 32 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3472 | 956 | 1455 | 712 | 1032 | 0.0000 | 0.0 | 643344 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 32 | 2 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4585 | 1284 | 2502 | 912 | 1232 | 0.0000 | 0.0 | 643088 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2958 | 821 | 845 | 580 | 904 | 0.0000 | 0.0 | 631174 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4022 | 861 | 2006 | 684 | 1028 | 0.0071 | 0.0 | 632702 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2955 | 872 | 872 | 600 | 944 | 0.0000 | 0.0 | 632868 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 33 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4713 | 1692 | 2699 | 776 | 1688 | 0.0071 | 0.0 | 646235 |
| logged-in-reader | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2707 | 692 | 692 | 580 | 948 | 0.0032 | 0.0 | 537660 |
| logged-in-reader | mobile | auth-public | /vi/login | 53 | 10 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4416 | 795 | 1720 | 568 | 932 | 0.0055 | 0.0 | 1092326 |
| logged-in-reader | mobile | auth-public | /vi/register | 50 | 13 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3426 | 744 | 756 | 556 | 920 | 0.0055 | 0.0 | 1037836 |
| logged-in-reader | mobile | auth-public | /vi/forgot-password | 53 | 10 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3792 | 706 | 1116 | 572 | 916 | 0.0055 | 0.0 | 1092479 |
| logged-in-reader | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3565 | 690 | 1556 | 580 | 580 | 0.0000 | 0.0 | 511985 |
| logged-in-reader | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3202 | 680 | 1191 | 564 | 564 | 0.0000 | 0.0 | 512230 |
| logged-in-reader | mobile | reading | /vi/reading | 33 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3950 | 893 | 1938 | 568 | 896 | 0.0000 | 0.0 | 654782 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 35 | 5 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3082 | 847 | 1070 | 560 | 916 | 0.0071 | 0.0 | 661564 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 37 | 5 | critical | 0 | 0 | 1 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 3796 | 855 | 1786 | 580 | 924 | 0.0071 | 0.0 | 799633 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3318 | 891 | 1308 | 588 | 932 | 0.0071 | 0.0 | 724401 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 33 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5774 | 781 | 810 | 552 | 552 | 0.0000 | 0.0 | 642170 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3014 | 835 | 1003 | 552 | 888 | 0.0000 | 0.0 | 639155 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3178 | 796 | 1168 | 580 | 904 | 0.0000 | 0.0 | 635077 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3239 | 728 | 1228 | 552 | 892 | 0.0071 | 0.0 | 632302 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2937 | 898 | 898 | 568 | 908 | 0.0000 | 0.0 | 635380 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2698 | 659 | 685 | 536 | 860 | 0.0000 | 0.0 | 631846 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2880 | 824 | 866 | 584 | 912 | 0.0196 | 0.0 | 649945 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 36 | 3 | critical | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3611 | 797 | 873 | 784 | 1568 | 0.0000 | 0.0 | 779101 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3772 | 949 | 1760 | 704 | 1040 | 0.0071 | 0.0 | 642211 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3019 | 839 | 1008 | 560 | 900 | 0.0000 | 0.0 | 637559 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4379 | 827 | 2364 | 600 | 944 | 0.0071 | 0.0 | 631876 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3804 | 724 | 1792 | 568 | 896 | 0.0071 | 0.0 | 631965 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 847 | 847 | 584 | 940 | 0.0330 | 0.0 | 633176 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 33 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4542 | 871 | 2530 | 592 | 932 | 0.0000 | 0.0 | 645269 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2927 | 790 | 882 | 600 | 960 | 0.0000 | 0.0 | 632603 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3284 | 718 | 1273 | 576 | 916 | 0.0071 | 0.0 | 632999 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4184 | 744 | 2163 | 588 | 900 | 0.0032 | 0.0 | 528260 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2711 | 692 | 692 | 548 | 860 | 0.0032 | 0.0 | 526380 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3374 | 706 | 1345 | 584 | 900 | 0.0055 | 0.0 | 526440 |
| logged-in-reader | mobile | reading | /vi/reading/session/5b5051d3-906e-4110-8726-89e662180866 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2761 | 723 | 746 | 556 | 880 | 0.0000 | 0.0 | 632476 |
| logged-in-reader | mobile | reading | /vi/reading/session/31cb1daf-28c3-4dae-beef-f752cffead2e | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2808 | 750 | 796 | 564 | 892 | 0.0000 | 0.0 | 632308 |
| logged-in-reader | mobile | reading | /vi/reading/session/04d92163-9af4-41cc-ab5e-d8e301f6c293 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2852 | 823 | 838 | 620 | 956 | 0.0000 | 0.0 | 632654 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3704 | 789 | 1675 | 616 | 944 | 0.0071 | 0.0 | 631523 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2812 | 768 | 799 | 548 | 876 | 0.0000 | 0.0 | 631392 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4389 | 796 | 2376 | 700 | 1020 | 0.0000 | 0.0 | 644615 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2762 | 698 | 749 | 552 | 964 | 0.0000 | 0.0 | 633023 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2832 | 715 | 821 | 568 | 900 | 0.0000 | 0.0 | 634096 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2748 | 707 | 732 | 564 | 916 | 0.0000 | 0.0 | 633345 |

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
| logged-in-admin | desktop | auth-public | /vi | 35 | high | 5 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi/login | 62 | critical | 7 | 50 | 0 |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 51 | critical | 0 | 46 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 39 | critical | 3 | 34 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 35 | high | 0 | 33 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 34 | high | 4 | 28 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 33 | high | 4 | 27 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 33 | high | 4 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 27 | high | 1 | 23 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/f828c893-4746-46f5-8e08-7de94cd84f35 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/4cedf788-12bb-4123-9073-992b264c96c2 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/60b1af90-19b3-4b78-80c3-b720c958cb1a | 36 | critical | 4 | 30 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | auth-public | /vi/login | 53 | critical | 0 | 49 | 0 |
| logged-in-reader | desktop | auth-public | /vi/forgot-password | 50 | critical | 0 | 46 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 37 | critical | 2 | 33 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 37 | critical | 2 | 33 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 34 | high | 0 | 32 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 32 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 27 | high | 1 | 23 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/e4197b22-32f1-4a80-9529-3f4c62e3a202 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/2385fd1c-641f-41a2-9aa0-2216f893cc49 | 36 | critical | 4 | 30 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/fa2fc343-c413-437a-b088-9b30ecb4fcd3 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 32 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi/login | 53 | critical | 0 | 49 | 0 |
| logged-in-admin | mobile | auth-public | /vi/register | 60 | critical | 5 | 49 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 39 | critical | 4 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 31 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 32 | high | 3 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 32 | high | 1 | 29 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/4d68762d-32b0-4770-9e25-bfc9d9cb7e62 | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/a27600a5-fbc0-4d42-acec-89870a2009c6 | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/9ad599f4-5a77-4a9e-91df-11084765bf37 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 32 | high | 3 | 27 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 32 | high | 3 | 27 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 33 | high | 4 | 27 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | mobile | auth-public | /vi/login | 53 | critical | 0 | 49 | 0 |
| logged-in-reader | mobile | auth-public | /vi/register | 50 | critical | 0 | 46 | 0 |
| logged-in-reader | mobile | auth-public | /vi/forgot-password | 53 | critical | 0 | 49 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 33 | high | 4 | 27 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 37 | critical | 2 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 33 | high | 4 | 27 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 27 | high | 1 | 23 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/5b5051d3-906e-4110-8726-89e662180866 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/31cb1daf-28c3-4dae-beef-f752cffead2e | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/04d92163-9af4-41cc-ab5e-d8e301f6c293 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 4 | 27 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5745 | 4751 | static | https://www.tarotnow.xyz/_next/static/chunks/0y-5.-cr3v3~z.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5730 | 4708 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5662 | 5209 | static | https://www.tarotnow.xyz/_next/static/chunks/064xhtgi5eo9n.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5602 | 2300 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5531 | 1559 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5374 | 5210 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5374 | 5214 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5336 | 5214 | static | https://www.tarotnow.xyz/_next/static/chunks/0rm0_6_wsunhe.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5323 | 4389 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5313 | 5214 | static | https://www.tarotnow.xyz/_next/static/chunks/0_yn3f1mymbbi.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5295 | 4395 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5244 | 4387 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5242 | 2317 | static | https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5233 | 5214 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5193 | 4361 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5176 | 4388 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 4817 | 4793 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 4726 | 2317 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 4155 | 3323 | static | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 4018 | 3369 | static | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 3812 | 3398 | static | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 3632 | 328 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 3253 | 328 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 3039 | 3023 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 2785 | 932 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-out | desktop | auth-public | /vi/register | GET | 200 | 2749 | 2729 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 2487 | 2317 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 2466 | 2447 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 2372 | 2317 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 2352 | 2325 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 2195 | 1435 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 2173 | 1108 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 2059 | 2044 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 2036 | 251 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | auth-public | /vi/verify-email | GET | 200 | 1994 | 1963 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1981 | 351 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1956 | 410 | html | https://www.tarotnow.xyz/vi/login |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 1952 | 1928 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1938 | 1642 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 1925 | 1624 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1919 | 640 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 1917 | 1886 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-out | desktop | auth-public | /vi/verify-email | GET | 200 | 1904 | 1895 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1832 | 398 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | GET | 200 | 1831 | 1683 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | GET | 200 | 1829 | 310 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | auth-public | /vi/login | GET | 200 | 1786 | 662 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | auth-public | /vi/login | GET | 200 | 1775 | 1738 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | admin | /vi/admin/readings | GET | 200 | 1729 | 1709 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1721 | 419 | static | https://www.tarotnow.xyz/_next/static/chunks/0y-5.-cr3v3~z.js |
| logged-in-admin | desktop | auth-public | /vi/login | GET | 200 | 1719 | 247 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1711 | 419 | static | https://www.tarotnow.xyz/_next/static/chunks/064xhtgi5eo9n.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1710 | 420 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 1672 | 336 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1633 | 81 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1622 | 421 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1619 | 442 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1618 | 494 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1612 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1606 | 266 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1605 | 421 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 1605 | 1450 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1598 | 420 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-out | mobile | auth-public | /vi/forgot-password | GET | 200 | 1594 | 709 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1529 | 266 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 1501 | 1479 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 1473 | 1454 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1448 | 1420 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 1445 | 1419 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 1444 | 1402 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | auth-public | /vi/verify-email | GET | 200 | 1444 | 1210 | html | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-admin | mobile | reading | /vi/reading/session/a27600a5-fbc0-4d42-acec-89870a2009c6 | GET | 200 | 1434 | 1410 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1406 | 1388 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 1391 | 1376 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | admin | /vi/admin/gamification | GET | 200 | 1352 | 1294 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | auth-public | /vi/login | GET | 200 | 1347 | 1329 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 1345 | 1319 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 1333 | 1021 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-out | desktop | auth-public | /vi/forgot-password | GET | 200 | 1284 | 296 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1283 | 593 | html | https://www.tarotnow.xyz/vi |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 800 | 338 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 800 | 427 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 799 | 548 | static | https://www.tarotnow.xyz/_next/static/chunks/0h8lshv9j35pw.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 798 | 312 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 797 | 322 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 796 | 405 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | reading | /vi/reading/session/4d68762d-32b0-4770-9e25-bfc9d9cb7e62 | GET | 200 | 796 | 770 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 795 | 311 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | GET | 200 | 795 | 527 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 794 | 187 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 794 | 323 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 793 | 500 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 793 | 345 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 792 | 320 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 791 | 745 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | auth-public | /vi | GET | 200 | 790 | 327 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 789 | 321 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 789 | 530 | static | https://www.tarotnow.xyz/_next/static/chunks/0sa2w0m9.t3c8.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 788 | 741 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 787 | 515 | static | https://www.tarotnow.xyz/_next/static/chunks/0sa2w0m9.t3c8.js |
| logged-in-admin | mobile | admin | /vi/admin/promotions | GET | 200 | 787 | 407 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 784 | 326 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 783 | 516 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 783 | 438 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | desktop | reading | /vi/reading/session/2385fd1c-641f-41a2-9aa0-2216f893cc49 | GET | 200 | 782 | 650 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 780 | 328 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 780 | 328 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 779 | 515 | static | https://www.tarotnow.xyz/_next/static/chunks/064xhtgi5eo9n.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 779 | 317 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 778 | 767 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Ffree_draw_ticket_50_20260416_165452.avif&w=48&q=75 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | GET | 200 | 778 | 322 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 778 | 530 | static | https://www.tarotnow.xyz/_next/static/chunks/0y-5.-cr3v3~z.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | GET | 200 | 777 | 339 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 775 | 327 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 774 | 322 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 774 | 315 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 772 | 395 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 771 | 430 | html | https://www.tarotnow.xyz/vi/admin |
| logged-out | desktop | auth-public | /vi/register | GET | 200 | 769 | 363 | html | https://www.tarotnow.xyz/vi/register |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 769 | 749 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | auth-public | /vi/verify-email | GET | 200 | 768 | 332 | html | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 767 | 758 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fexp_booster_50_20260416_165452.avif&w=48&q=75 |
| logged-in-reader | mobile | auth-public | /vi/verify-email | GET | 200 | 767 | 746 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 766 | 332 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 766 | 309 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | desktop | admin | /vi/admin/users | GET | 200 | 765 | 356 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 763 | 533 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | reading | /vi/reading/session/04d92163-9af4-41cc-ab5e-d8e301f6c293 | GET | 200 | 763 | 389 | html | https://www.tarotnow.xyz/vi/reading/session/04d92163-9af4-41cc-ab5e-d8e301f6c293 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | GET | 200 | 761 | 314 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 761 | 365 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 758 | 538 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 758 | 605 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 758 | 348 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 757 | 394 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | GET | 200 | 755 | 323 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | desktop | reading | /vi/reading/session/4cedf788-12bb-4123-9073-992b264c96c2 | GET | 200 | 753 | 311 | html | https://www.tarotnow.xyz/vi/reading/session/4cedf788-12bb-4123-9073-992b264c96c2 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 752 | 345 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | mobile | auth-public | /vi/register | GET | 200 | 749 | 383 | html | https://www.tarotnow.xyz/vi/register |
| logged-in-reader | desktop | reading | /vi/reading/session/fa2fc343-c413-437a-b088-9b30ecb4fcd3 | GET | 200 | 748 | 321 | html | https://www.tarotnow.xyz/vi/reading/session/fa2fc343-c413-437a-b088-9b30ecb4fcd3 |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 748 | 341 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | reading | /vi/reading/session/60b1af90-19b3-4b78-80c3-b720c958cb1a | GET | 200 | 747 | 307 | html | https://www.tarotnow.xyz/vi/reading/session/60b1af90-19b3-4b78-80c3-b720c958cb1a |
| logged-in-reader | mobile | auth-public | /vi/reset-password | GET | 200 | 746 | 730 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 744 | 331 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 744 | 323 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | auth-public | /vi/reset-password | GET | 200 | 743 | 341 | html | https://www.tarotnow.xyz/vi/reset-password |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 743 | 700 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fexp_booster_50_20260416_165452.avif&w=96&q=75 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 741 | 301 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | auth-public | /vi/forgot-password | GET | 200 | 740 | 271 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | GET | 200 | 739 | 350 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 739 | 375 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 736 | 355 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | auth-public | /vi/forgot-password | GET | 200 | 735 | 721 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 734 | 518 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 733 | 517 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | admin | /vi/admin/gamification | GET | 200 | 732 | 316 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 732 | 332 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 732 | 355 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | reading | /vi/reading/history | GET | 200 | 730 | 325 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 730 | 327 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 729 | 231 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-out | desktop | /vi | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | desktop | /vi/login | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| logged-in-admin | desktop | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F14_Death_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0y-5.-cr3v3~z.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/064xhtgi5eo9n.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/0sa2w0m9.t3c8.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-reader | desktop | /vi/register | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/wallet/balance |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/chat/unread-count |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/notifications/unread-count |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/forgot-password | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/reading | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

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
| logged-in-admin | desktop | reading.init.spread_3: created f828c893-4746-46f5-8e08-7de94cd84f35. |
| logged-in-admin | desktop | reading.init.spread_5: created 4cedf788-12bb-4123-9073-992b264c96c2. |
| logged-in-admin | desktop | reading.init.spread_10: created 60b1af90-19b3-4b78-80c3-b720c958cb1a. |
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
| logged-in-reader | desktop | reading.init.spread_3: created e4197b22-32f1-4a80-9529-3f4c62e3a202. |
| logged-in-reader | desktop | reading.init.spread_5: created 2385fd1c-641f-41a2-9aa0-2216f893cc49. |
| logged-in-reader | desktop | reading.init.spread_10: created fa2fc343-c413-437a-b088-9b30ecb4fcd3. |
| logged-in-reader | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | desktop | reader-detail:ui-discovery-empty |
| logged-in-reader | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-reader | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-reader | desktop | community-posts:api-discovery-1 |
| logged-in-reader | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | desktop | scenario-filter:reader-admin-routes-skipped=10 |
| logged-out | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-out | mobile | dynamic-routes: skipped for logged-out scenario. |
| logged-out | mobile | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | mobile | reading.init.daily_1: blocked (400). |
| logged-in-admin | mobile | reading.init.spread_3: created 4d68762d-32b0-4770-9e25-bfc9d9cb7e62. |
| logged-in-admin | mobile | reading.init.spread_5: created a27600a5-fbc0-4d42-acec-89870a2009c6. |
| logged-in-admin | mobile | reading.init.spread_10: created 9ad599f4-5a77-4a9e-91df-11084765bf37. |
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
| logged-in-reader | mobile | reading.init.spread_3: created 5b5051d3-906e-4110-8726-89e662180866. |
| logged-in-reader | mobile | reading.init.spread_5: created 31cb1daf-28c3-4dae-beef-f752cffead2e. |
| logged-in-reader | mobile | reading.init.spread_10: created 04d92163-9af4-41cc-ab5e-d8e301f6c293. |
| logged-in-reader | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | mobile | reader-detail:ui-discovery-empty |
| logged-in-reader | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-reader | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-reader | mobile | community-posts:api-discovery-1 |
| logged-in-reader | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | mobile | scenario-filter:reader-admin-routes-skipped=10 |

## Login Bootstrap Notes
### logged-in-admin / desktop
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.

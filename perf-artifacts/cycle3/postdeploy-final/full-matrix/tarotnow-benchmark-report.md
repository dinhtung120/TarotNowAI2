# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-04-30T07:24:43.284Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 11
- High pages (request count): 180
- High slow requests: 79
- Medium slow requests: 605

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2887 | 237 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 48 | 3095 | 1544 | 0 | 0 | 10 | 1 | yes |
| logged-in-reader | desktop | 38 | 3172 | 1227 | 0 | 0 | 6 | 1 | yes |
| logged-out | mobile | 9 | 2956 | 233 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 48 | 3277 | 1553 | 0 | 0 | 6 | 2 | yes |
| logged-in-reader | mobile | 38 | 2908 | 1188 | 0 | 0 | 5 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 33.5 | 2903 | 0 |
| logged-in-admin | desktop | auth | 5 | 31.6 | 3259 | 0 |
| logged-in-admin | desktop | chat | 1 | 31.0 | 2723 | 0 |
| logged-in-admin | desktop | collection | 1 | 32.0 | 6934 | 0 |
| logged-in-admin | desktop | community | 1 | 32.0 | 3563 | 0 |
| logged-in-admin | desktop | gacha | 2 | 34.0 | 3177 | 0 |
| logged-in-admin | desktop | gamification | 1 | 33.0 | 3263 | 0 |
| logged-in-admin | desktop | home | 1 | 33.0 | 3020 | 0 |
| logged-in-admin | desktop | inventory | 1 | 33.0 | 2812 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 34.0 | 3493 | 0 |
| logged-in-admin | desktop | legal | 3 | 28.3 | 2799 | 0 |
| logged-in-admin | desktop | notifications | 1 | 33.0 | 3326 | 0 |
| logged-in-admin | desktop | profile | 3 | 32.0 | 2925 | 0 |
| logged-in-admin | desktop | reader | 1 | 31.0 | 2798 | 0 |
| logged-in-admin | desktop | readers | 7 | 31.3 | 2938 | 0 |
| logged-in-admin | desktop | reading | 5 | 33.2 | 2976 | 0 |
| logged-in-admin | desktop | wallet | 4 | 31.3 | 3085 | 0 |
| logged-in-admin | mobile | admin | 10 | 33.5 | 3362 | 0 |
| logged-in-admin | mobile | auth | 5 | 33.0 | 3468 | 0 |
| logged-in-admin | mobile | chat | 1 | 31.0 | 2981 | 0 |
| logged-in-admin | mobile | collection | 1 | 32.0 | 6912 | 0 |
| logged-in-admin | mobile | community | 1 | 40.0 | 4172 | 0 |
| logged-in-admin | mobile | gacha | 2 | 37.0 | 3360 | 0 |
| logged-in-admin | mobile | gamification | 1 | 34.0 | 3323 | 0 |
| logged-in-admin | mobile | home | 1 | 29.0 | 3007 | 0 |
| logged-in-admin | mobile | inventory | 1 | 33.0 | 3223 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 33.0 | 3247 | 0 |
| logged-in-admin | mobile | legal | 3 | 28.7 | 3056 | 0 |
| logged-in-admin | mobile | notifications | 1 | 32.0 | 2874 | 0 |
| logged-in-admin | mobile | profile | 3 | 32.3 | 3124 | 0 |
| logged-in-admin | mobile | reader | 1 | 31.0 | 2958 | 0 |
| logged-in-admin | mobile | readers | 7 | 31.4 | 2953 | 0 |
| logged-in-admin | mobile | reading | 5 | 31.2 | 2894 | 0 |
| logged-in-admin | mobile | wallet | 4 | 31.3 | 3308 | 0 |
| logged-in-reader | desktop | auth | 5 | 30.8 | 3181 | 0 |
| logged-in-reader | desktop | chat | 1 | 31.0 | 2745 | 0 |
| logged-in-reader | desktop | collection | 1 | 32.0 | 7250 | 0 |
| logged-in-reader | desktop | community | 1 | 32.0 | 3539 | 0 |
| logged-in-reader | desktop | gacha | 2 | 36.5 | 2949 | 0 |
| logged-in-reader | desktop | gamification | 1 | 34.0 | 3507 | 0 |
| logged-in-reader | desktop | home | 1 | 33.0 | 3155 | 0 |
| logged-in-reader | desktop | inventory | 1 | 33.0 | 2746 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 34.0 | 2876 | 0 |
| logged-in-reader | desktop | legal | 3 | 29.3 | 3004 | 0 |
| logged-in-reader | desktop | notifications | 1 | 32.0 | 2744 | 0 |
| logged-in-reader | desktop | profile | 3 | 32.3 | 3023 | 0 |
| logged-in-reader | desktop | reader | 1 | 31.0 | 2706 | 0 |
| logged-in-reader | desktop | readers | 7 | 31.3 | 3007 | 0 |
| logged-in-reader | desktop | reading | 5 | 35.2 | 3269 | 0 |
| logged-in-reader | desktop | wallet | 4 | 32.0 | 3000 | 0 |
| logged-in-reader | mobile | auth | 5 | 30.0 | 3047 | 0 |
| logged-in-reader | mobile | chat | 1 | 31.0 | 2726 | 0 |
| logged-in-reader | mobile | collection | 1 | 32.0 | 5580 | 0 |
| logged-in-reader | mobile | community | 1 | 32.0 | 3480 | 0 |
| logged-in-reader | mobile | gacha | 2 | 34.0 | 2722 | 0 |
| logged-in-reader | mobile | gamification | 1 | 34.0 | 2837 | 0 |
| logged-in-reader | mobile | home | 1 | 29.0 | 2712 | 0 |
| logged-in-reader | mobile | inventory | 1 | 33.0 | 2797 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 33.0 | 2695 | 0 |
| logged-in-reader | mobile | legal | 3 | 28.0 | 2737 | 0 |
| logged-in-reader | mobile | notifications | 1 | 32.0 | 2859 | 0 |
| logged-in-reader | mobile | profile | 3 | 32.7 | 2857 | 0 |
| logged-in-reader | mobile | reader | 1 | 32.0 | 2965 | 0 |
| logged-in-reader | mobile | readers | 7 | 31.0 | 2762 | 0 |
| logged-in-reader | mobile | reading | 5 | 31.4 | 2760 | 0 |
| logged-in-reader | mobile | wallet | 4 | 31.5 | 2810 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2872 | 0 |
| logged-out | desktop | home | 1 | 33.0 | 3322 | 0 |
| logged-out | desktop | legal | 3 | 28.0 | 2767 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2899 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 3514 | 0 |
| logged-out | mobile | legal | 3 | 28.0 | 2864 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | /vi | 33 | 1 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3322 | 1305 | 1305 | 1040 | 1040 | 0.0000 | 151.0 | 556205 |
| logged-out | desktop | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3529 | 1520 | 1520 | 1340 | 1340 | 0.0000 | 0.0 | 432524 |
| logged-out | desktop | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2765 | 754 | 754 | 624 | 624 | 0.0000 | 0.0 | 433041 |
| logged-out | desktop | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2658 | 650 | 650 | 620 | 620 | 0.0000 | 0.0 | 432165 |
| logged-out | desktop | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2652 | 641 | 642 | 484 | 484 | 0.0000 | 0.0 | 432199 |
| logged-out | desktop | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2758 | 739 | 739 | 688 | 688 | 0.0000 | 0.0 | 432250 |
| logged-out | desktop | /vi/legal/tos | 28 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2789 | 782 | 782 | 496 | 496 | 0.0000 | 0.0 | 469136 |
| logged-out | desktop | /vi/legal/privacy | 28 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2686 | 680 | 680 | 484 | 484 | 0.0000 | 0.0 | 469072 |
| logged-out | desktop | /vi/legal/ai-disclaimer | 28 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2825 | 816 | 816 | 548 | 548 | 0.0000 | 0.0 | 469268 |
| logged-in-admin | desktop | /vi | 33 | 4 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 3020 | 744 | 1006 | 1100 | 1100 | 0.0024 | 353.0 | 563783 |
| logged-in-admin | desktop | /vi/login | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 0 | 2 | 2 | 0 | 0 | 3180 | 1147 | 1167 | 972 | 1564 | 0.0028 | 324.0 | 483330 |
| logged-in-admin | desktop | /vi/register | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 0 | 2 | 2 | 0 | 0 | 3049 | 1036 | 1036 | 916 | 1508 | 0.0024 | 308.0 | 483209 |
| logged-in-admin | desktop | /vi/forgot-password | 34 | 4 | high | 0 | 0 | 0 | 0 | 2 | 1 | 1 | 2 | 0 | 0 | 3414 | 1318 | 1366 | 1424 | 1424 | 0.0028 | 90.0 | 564099 |
| logged-in-admin | desktop | /vi/reset-password | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 3130 | 1095 | 1095 | 1084 | 1688 | 0.0030 | 318.0 | 483152 |
| logged-in-admin | desktop | /vi/verify-email | 34 | 4 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 3522 | 1152 | 1509 | 1620 | 1620 | 0.0024 | 151.0 | 563912 |
| logged-in-admin | desktop | /vi/reading | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2891 | 874 | 874 | 764 | 1200 | 0.0029 | 0.0 | 588489 |
| logged-in-admin | desktop | /vi/inventory | 33 | 8 | high | 0 | 0 | 0 | 0 | 6 | 0 | 5 | 6 | 0 | 0 | 2812 | 766 | 794 | 644 | 1072 | 0.0029 | 0.0 | 590998 |
| logged-in-admin | desktop | /vi/gacha | 35 | 6 | high | 0 | 0 | 1 | 0 | 6 | 0 | 0 | 6 | 0 | 0 | 3366 | 1351 | 1351 | 752 | 1528 | 0.0029 | 0.0 | 675586 |
| logged-in-admin | desktop | /vi/gacha/history | 33 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2988 | 904 | 934 | 880 | 1576 | 0.0029 | 0.0 | 673060 |
| logged-in-admin | desktop | /vi/collection | 32 | 33 | high | 0 | 0 | 0 | 0 | 23 | 8 | 0 | 22 | 1 | 0 | 6934 | 717 | 718 | 596 | 964 | 0.0031 | 51.0 | 589005 |
| logged-in-admin | desktop | /vi/profile | 34 | 1 | high | 0 | 0 | 1 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 3133 | 730 | 1002 | 604 | 1048 | 0.0477 | 0.0 | 584684 |
| logged-in-admin | desktop | /vi/profile/mfa | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2855 | 806 | 815 | 652 | 1032 | 0.0029 | 0.0 | 577990 |
| logged-in-admin | desktop | /vi/profile/reader | 31 | 39 | high | 0 | 0 | 0 | 0 | 3 | 1 | 0 | 3 | 0 | 0 | 2786 | 689 | 937 | 588 | 988 | 0.0481 | 0.0 | 577197 |
| logged-in-admin | desktop | /vi/readers | 31 | 5 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 2760 | 730 | 731 | 576 | 996 | 0.0029 | 0.0 | 580791 |
| logged-in-admin | desktop | /vi/chat | 31 | 8 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2723 | 706 | 707 | 560 | 972 | 0.0029 | 0.0 | 578042 |
| logged-in-admin | desktop | /vi/leaderboard | 34 | 5 | high | 0 | 0 | 1 | 0 | 3 | 2 | 0 | 3 | 0 | 0 | 3493 | 984 | 1486 | 712 | 1108 | 0.0167 | 0.0 | 589910 |
| logged-in-admin | desktop | /vi/community | 32 | 12 | high | 0 | 0 | 0 | 0 | 3 | 2 | 0 | 3 | 0 | 0 | 3563 | 805 | 812 | 660 | 1792 | 0.0029 | 0.0 | 588767 |
| logged-in-admin | desktop | /vi/gamification | 33 | 3 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3263 | 842 | 1248 | 632 | 1064 | 0.0180 | 0.0 | 591251 |
| logged-in-admin | desktop | /vi/wallet | 32 | 1 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3696 | 1644 | 1645 | 1320 | 1904 | 0.0029 | 0.0 | 581288 |
| logged-in-admin | desktop | /vi/wallet/deposit | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 2783 | 766 | 767 | 684 | 1080 | 0.0029 | 0.0 | 577816 |
| logged-in-admin | desktop | /vi/wallet/deposit/history | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2866 | 806 | 807 | 580 | 1188 | 0.0029 | 0.0 | 578805 |
| logged-in-admin | desktop | /vi/wallet/withdraw | 31 | 37 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 2995 | 803 | 805 | 656 | 1144 | 0.0029 | 0.0 | 577214 |
| logged-in-admin | desktop | /vi/notifications | 33 | 7 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3326 | 888 | 1302 | 560 | 952 | 0.0033 | 0.0 | 582768 |
| logged-in-admin | desktop | /vi/reader/apply | 31 | 4 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2798 | 760 | 760 | 636 | 1072 | 0.0029 | 0.0 | 578967 |
| logged-in-admin | desktop | /vi/reading/history | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2771 | 759 | 759 | 644 | 1276 | 0.0029 | 0.0 | 578918 |
| logged-in-admin | desktop | /vi/legal/tos | 28 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2746 | 733 | 733 | 660 | 1024 | 0.0012 | 0.0 | 470096 |
| logged-in-admin | desktop | /vi/legal/privacy | 29 | 2 | high | 0 | 0 | 0 | 1 | 1 | 0 | 0 | 1 | 0 | 0 | 2864 | 840 | 841 | 564 | 912 | 0.0012 | 0.0 | 470295 |
| logged-in-admin | desktop | /vi/legal/ai-disclaimer | 28 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2787 | 754 | 772 | 652 | 992 | 0.0012 | 0.0 | 470294 |
| logged-in-admin | desktop | /vi/admin | 33 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2753 | 698 | 713 | 564 | 960 | 0.0000 | 0.0 | 596242 |
| logged-in-admin | desktop | /vi/admin/deposits | 33 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2734 | 719 | 719 | 676 | 1100 | 0.0000 | 0.0 | 596466 |
| logged-in-admin | desktop | /vi/admin/disputes | 33 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2767 | 732 | 733 | 600 | 960 | 0.0000 | 0.0 | 594772 |
| logged-in-admin | desktop | /vi/admin/gamification | 37 | 1 | critical | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3219 | 722 | 1206 | 556 | 908 | 0.0022 | 0.0 | 640982 |
| logged-in-admin | desktop | /vi/admin/promotions | 32 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3069 | 960 | 961 | 740 | 1120 | 0.0000 | 0.0 | 593408 |
| logged-in-admin | desktop | /vi/admin/reader-requests | 33 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3052 | 794 | 802 | 652 | 984 | 0.0000 | 0.0 | 595335 |
| logged-in-admin | desktop | /vi/admin/readings | 34 | 2 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2943 | 924 | 924 | 728 | 1240 | 0.0000 | 0.0 | 598495 |
| logged-in-admin | desktop | /vi/admin/system-configs | 32 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2789 | 766 | 767 | 632 | 1116 | 0.0000 | 0.0 | 603604 |
| logged-in-admin | desktop | /vi/admin/users | 35 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2956 | 859 | 925 | 576 | 1292 | 0.0000 | 0.0 | 600210 |
| logged-in-admin | desktop | /vi/admin/withdrawals | 33 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2744 | 727 | 727 | 632 | 928 | 0.0000 | 0.0 | 594890 |
| logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2760 | 742 | 745 | 672 | 1240 | 0.0029 | 0.0 | 579357 |
| logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2753 | 720 | 725 | 596 | 1200 | 0.0029 | 0.0 | 579387 |
| logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 32 | 2 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2840 | 766 | 792 | 516 | 1208 | 0.0029 | 0.0 | 579943 |
| logged-in-admin | desktop | /vi/reading/session/0cbaad24-c06d-491f-8c2b-d0e5103f100d | 38 | 1 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3305 | 1107 | 1280 | 680 | 1256 | 0.0029 | 0.0 | 673126 |
| logged-in-admin | desktop | /vi/reading/session/70caef91-ed5b-474c-b4a3-4675414c483d | 31 | 8 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2824 | 755 | 757 | 604 | 1012 | 0.0029 | 0.0 | 578775 |
| logged-in-admin | desktop | /vi/reading/session/33a23dc7-8b59-4385-b20c-467d52f8a45e | 35 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3087 | 895 | 1023 | 632 | 1080 | 0.0029 | 0.0 | 660210 |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2867 | 798 | 803 | 592 | 948 | 0.0029 | 0.0 | 577594 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 32 | 2 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3235 | 1120 | 1122 | 684 | 1232 | 0.0029 | 0.0 | 579359 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3348 | 763 | 1303 | 556 | 960 | 0.0029 | 0.0 | 577713 |
| logged-in-reader | desktop | /vi | 33 | 4 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 3155 | 886 | 1130 | 1240 | 1240 | 0.0022 | 62.0 | 563931 |
| logged-in-reader | desktop | /vi/login | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 0 | 2 | 2 | 0 | 0 | 2994 | 970 | 970 | 816 | 1528 | 0.0022 | 485.0 | 483158 |
| logged-in-reader | desktop | /vi/register | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3142 | 1053 | 1053 | 896 | 1680 | 0.0022 | 391.0 | 483112 |
| logged-in-reader | desktop | /vi/forgot-password | 34 | 4 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 3515 | 1351 | 1366 | 1444 | 1444 | 0.0022 | 434.0 | 563701 |
| logged-in-reader | desktop | /vi/reset-password | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 3225 | 1183 | 1183 | 984 | 1592 | 0.0026 | 353.0 | 483064 |
| logged-in-reader | desktop | /vi/verify-email | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 3027 | 990 | 990 | 872 | 1484 | 0.0022 | 336.0 | 483142 |
| logged-in-reader | desktop | /vi/reading | 33 | 1 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2921 | 871 | 873 | 580 | 1228 | 0.0026 | 0.0 | 590248 |
| logged-in-reader | desktop | /vi/inventory | 33 | 7 | high | 0 | 0 | 0 | 0 | 5 | 2 | 0 | 5 | 0 | 0 | 2746 | 702 | 703 | 536 | 944 | 0.0026 | 0.0 | 590920 |
| logged-in-reader | desktop | /vi/gacha | 39 | 2 | critical | 0 | 0 | 1 | 0 | 6 | 1 | 0 | 6 | 0 | 0 | 3019 | 938 | 939 | 636 | 1060 | 0.0026 | 0.0 | 681841 |
| logged-in-reader | desktop | /vi/gacha/history | 34 | 2 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2879 | 761 | 824 | 532 | 1320 | 0.0026 | 0.0 | 672284 |
| logged-in-reader | desktop | /vi/collection | 32 | 26 | high | 0 | 0 | 0 | 0 | 12 | 2 | 0 | 12 | 0 | 0 | 7250 | 865 | 899 | 576 | 996 | 0.0026 | 13.0 | 588333 |
| logged-in-reader | desktop | /vi/profile | 35 | 2 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 3232 | 832 | 1069 | 680 | 1112 | 0.0713 | 4.0 | 586295 |
| logged-in-reader | desktop | /vi/profile/mfa | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3060 | 766 | 945 | 952 | 952 | 0.0026 | 0.0 | 578399 |
| logged-in-reader | desktop | /vi/profile/reader | 31 | 5 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2776 | 727 | 727 | 520 | 1340 | 0.0026 | 0.0 | 579097 |
| logged-in-reader | desktop | /vi/readers | 32 | 3 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 2874 | 820 | 844 | 888 | 888 | 0.0026 | 0.0 | 581983 |
| logged-in-reader | desktop | /vi/chat | 31 | 8 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2745 | 698 | 710 | 568 | 1004 | 0.0026 | 0.0 | 578249 |
| logged-in-reader | desktop | /vi/leaderboard | 34 | 4 | high | 0 | 0 | 1 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 2876 | 779 | 784 | 576 | 936 | 0.0164 | 0.0 | 590091 |
| logged-in-reader | desktop | /vi/community | 32 | 12 | high | 0 | 0 | 0 | 0 | 3 | 0 | 1 | 3 | 0 | 0 | 3539 | 715 | 716 | 512 | 1736 | 0.0026 | 0.0 | 588952 |
| logged-in-reader | desktop | /vi/gamification | 34 | 1 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3507 | 1149 | 1150 | 888 | 1204 | 0.0177 | 0.0 | 592700 |
| logged-in-reader | desktop | /vi/wallet | 32 | 1 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3102 | 1026 | 1026 | 620 | 1148 | 0.0026 | 0.0 | 581730 |
| logged-in-reader | desktop | /vi/wallet/deposit | 32 | 1 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3103 | 925 | 928 | 572 | 1012 | 0.0026 | 2.0 | 578987 |
| logged-in-reader | desktop | /vi/wallet/deposit/history | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2814 | 755 | 767 | 564 | 1004 | 0.0026 | 7.0 | 578376 |
| logged-in-reader | desktop | /vi/wallet/withdraw | 33 | 3 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2981 | 862 | 863 | 528 | 1036 | 0.0026 | 0.0 | 581681 |
| logged-in-reader | desktop | /vi/notifications | 32 | 8 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2744 | 697 | 712 | 544 | 984 | 0.0026 | 0.0 | 582038 |
| logged-in-reader | desktop | /vi/reader/apply | 31 | 4 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2706 | 648 | 662 | 536 | 908 | 0.0026 | 0.0 | 578986 |
| logged-in-reader | desktop | /vi/reading/history | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2836 | 741 | 757 | 592 | 1228 | 0.0026 | 0.0 | 579416 |
| logged-in-reader | desktop | /vi/legal/tos | 30 | 1 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3067 | 783 | 783 | 680 | 680 | 0.0010 | 0.0 | 472242 |
| logged-in-reader | desktop | /vi/legal/privacy | 30 | 1 | high | 0 | 0 | 0 | 1 | 1 | 0 | 0 | 1 | 0 | 0 | 3194 | 890 | 890 | 816 | 816 | 0.0010 | 0.0 | 471188 |
| logged-in-reader | desktop | /vi/legal/ai-disclaimer | 28 | 3 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 2752 | 731 | 731 | 552 | 932 | 0.0010 | 0.0 | 470301 |
| logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2822 | 703 | 721 | 508 | 1124 | 0.0026 | 0.0 | 579769 |
| logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3208 | 1029 | 1042 | 868 | 868 | 0.0026 | 2.0 | 579741 |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 32 | 1 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3587 | 1348 | 1366 | 608 | 1128 | 0.0026 | 0.0 | 580243 |
| logged-in-reader | desktop | /vi/reading/session/a24a62fe-e811-43e6-9181-2d98e5388673 | 38 | 1 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3195 | 761 | 1051 | 556 | 1020 | 0.0026 | 5.0 | 673104 |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | 37 | 1 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3757 | 1320 | 1609 | 676 | 1144 | 0.0026 | 0.0 | 672257 |
| logged-in-reader | desktop | /vi/reading/session/9c624020-f4ae-4422-9c39-a7c852858fd9 | 37 | 1 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3634 | 1178 | 1550 | 908 | 908 | 0.0026 | 0.0 | 672273 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2777 | 731 | 750 | 552 | 948 | 0.0026 | 0.0 | 577697 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2822 | 781 | 783 | 652 | 1028 | 0.0026 | 0.0 | 577737 |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2958 | 870 | 880 | 648 | 1028 | 0.0026 | 0.0 | 577786 |
| logged-out | mobile | /vi | 29 | 5 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3514 | 1247 | 1323 | 1084 | 1488 | 0.0000 | 10.0 | 479651 |
| logged-out | mobile | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3014 | 800 | 897 | 760 | 760 | 0.0000 | 0.0 | 432526 |
| logged-out | mobile | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2756 | 697 | 697 | 524 | 524 | 0.0000 | 0.0 | 433098 |
| logged-out | mobile | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2915 | 775 | 775 | 556 | 556 | 0.0000 | 0.0 | 432121 |
| logged-out | mobile | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2744 | 674 | 674 | 512 | 512 | 0.0000 | 0.0 | 432190 |
| logged-out | mobile | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3066 | 833 | 906 | 716 | 716 | 0.0000 | 0.0 | 432332 |
| logged-out | mobile | /vi/legal/tos | 28 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2800 | 698 | 699 | 536 | 536 | 0.0000 | 0.0 | 469212 |
| logged-out | mobile | /vi/legal/privacy | 28 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2910 | 761 | 775 | 696 | 696 | 0.0000 | 0.0 | 469048 |
| logged-out | mobile | /vi/legal/ai-disclaimer | 28 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2882 | 690 | 690 | 572 | 572 | 0.0000 | 0.0 | 469245 |
| logged-in-admin | mobile | /vi | 29 | 8 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3007 | 838 | 842 | 652 | 1072 | 0.0024 | 1.0 | 483170 |
| logged-in-admin | mobile | /vi/login | 34 | 4 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3301 | 1076 | 1122 | 1036 | 1036 | 0.0024 | 0.0 | 571704 |
| logged-in-admin | mobile | /vi/register | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3439 | 1146 | 1146 | 1000 | 1388 | 0.0024 | 0.0 | 483004 |
| logged-in-admin | mobile | /vi/forgot-password | 37 | 1 | critical | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3491 | 1131 | 1221 | 1060 | 1060 | 0.0024 | 6.0 | 575616 |
| logged-in-admin | mobile | /vi/reset-password | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3471 | 1223 | 1237 | 1088 | 1444 | 0.0024 | 0.0 | 483214 |
| logged-in-admin | mobile | /vi/verify-email | 34 | 4 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3640 | 1349 | 1380 | 1068 | 1068 | 0.0024 | 6.0 | 571512 |
| logged-in-admin | mobile | /vi/reading | 32 | 2 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3057 | 892 | 894 | 460 | 876 | 0.0000 | 0.0 | 588715 |
| logged-in-admin | mobile | /vi/inventory | 33 | 8 | high | 0 | 0 | 0 | 0 | 6 | 6 | 0 | 6 | 0 | 0 | 3223 | 863 | 866 | 600 | 992 | 0.0000 | 0.0 | 590983 |
| logged-in-admin | mobile | /vi/gacha | 40 | 1 | critical | 0 | 0 | 1 | 0 | 6 | 5 | 1 | 6 | 0 | 0 | 3362 | 1151 | 1151 | 600 | 1044 | 0.0000 | 40.0 | 748238 |
| logged-in-admin | mobile | /vi/gacha/history | 34 | 1 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3357 | 956 | 1027 | 608 | 996 | 0.0000 | 0.0 | 675092 |
| logged-in-admin | mobile | /vi/collection | 32 | 32 | high | 0 | 0 | 0 | 0 | 23 | 13 | 0 | 22 | 1 | 0 | 6912 | 944 | 954 | 648 | 964 | 0.0000 | 85.0 | 589102 |
| logged-in-admin | mobile | /vi/profile | 34 | 1 | high | 0 | 0 | 1 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3295 | 844 | 1132 | 684 | 1056 | 0.0000 | 0.0 | 595710 |
| logged-in-admin | mobile | /vi/profile/mfa | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3113 | 897 | 921 | 688 | 1048 | 0.0000 | 0.0 | 578155 |
| logged-in-admin | mobile | /vi/profile/reader | 32 | 37 | high | 0 | 0 | 1 | 1 | 3 | 2 | 0 | 3 | 0 | 0 | 2965 | 936 | 1180 | 688 | 688 | 0.0718 | 0.0 | 577147 |
| logged-in-admin | mobile | /vi/readers | 32 | 3 | high | 0 | 0 | 0 | 0 | 2 | 2 | 0 | 2 | 0 | 0 | 3516 | 931 | 944 | 724 | 724 | 0.0000 | 0.0 | 587354 |
| logged-in-admin | mobile | /vi/chat | 31 | 8 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2981 | 792 | 798 | 572 | 936 | 0.0000 | 0.0 | 577938 |
| logged-in-admin | mobile | /vi/leaderboard | 33 | 4 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3247 | 915 | 951 | 724 | 724 | 0.0196 | 0.0 | 588625 |
| logged-in-admin | mobile | /vi/community | 40 | 3 | critical | 0 | 0 | 1 | 1 | 3 | 2 | 0 | 3 | 0 | 0 | 4172 | 1035 | 1162 | 836 | 1884 | 0.0051 | 0.0 | 726716 |
| logged-in-admin | mobile | /vi/gamification | 34 | 1 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3323 | 1069 | 1069 | 640 | 992 | 0.0000 | 0.0 | 593821 |
| logged-in-admin | mobile | /vi/wallet | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3541 | 1113 | 1128 | 920 | 920 | 0.0000 | 24.0 | 580136 |
| logged-in-admin | mobile | /vi/wallet/deposit | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3350 | 1003 | 1016 | 632 | 980 | 0.0000 | 0.0 | 578275 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3191 | 864 | 898 | 556 | 916 | 0.0000 | 0.0 | 578503 |
| logged-in-admin | mobile | /vi/wallet/withdraw | 32 | 35 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 3150 | 966 | 966 | 632 | 1068 | 0.0030 | 0.0 | 579404 |
| logged-in-admin | mobile | /vi/notifications | 32 | 8 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2874 | 739 | 742 | 576 | 940 | 0.0000 | 0.0 | 581950 |
| logged-in-admin | mobile | /vi/reader/apply | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2958 | 763 | 795 | 476 | 832 | 0.0000 | 0.0 | 578984 |
| logged-in-admin | mobile | /vi/reading/history | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3289 | 909 | 925 | 708 | 708 | 0.0000 | 0.0 | 578973 |
| logged-in-admin | mobile | /vi/legal/tos | 30 | 1 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3277 | 988 | 988 | 560 | 864 | 0.0024 | 0.0 | 473435 |
| logged-in-admin | mobile | /vi/legal/privacy | 28 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3000 | 767 | 774 | 636 | 636 | 0.0024 | 0.0 | 470162 |
| logged-in-admin | mobile | /vi/legal/ai-disclaimer | 28 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2890 | 774 | 780 | 564 | 880 | 0.0024 | 0.0 | 470344 |
| logged-in-admin | mobile | /vi/admin | 33 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3545 | 1196 | 1200 | 784 | 784 | 0.0000 | 0.0 | 596106 |
| logged-in-admin | mobile | /vi/admin/deposits | 33 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3282 | 979 | 1005 | 636 | 992 | 0.0000 | 0.0 | 596357 |
| logged-in-admin | mobile | /vi/admin/disputes | 33 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3175 | 888 | 905 | 628 | 964 | 0.0000 | 0.0 | 594476 |
| logged-in-admin | mobile | /vi/admin/gamification | 36 | 1 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3214 | 869 | 1113 | 580 | 924 | 0.0030 | 0.0 | 639743 |
| logged-in-admin | mobile | /vi/admin/promotions | 32 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3185 | 911 | 915 | 556 | 900 | 0.0000 | 0.0 | 593297 |
| logged-in-admin | mobile | /vi/admin/reader-requests | 33 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4024 | 1647 | 1647 | 756 | 756 | 0.0000 | 0.0 | 595226 |
| logged-in-admin | mobile | /vi/admin/readings | 34 | 2 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3542 | 1170 | 1185 | 516 | 892 | 0.0000 | 0.0 | 598370 |
| logged-in-admin | mobile | /vi/admin/system-configs | 34 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3293 | 881 | 1137 | 704 | 704 | 0.0000 | 0.0 | 637013 |
| logged-in-admin | mobile | /vi/admin/users | 34 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3379 | 993 | 1000 | 536 | 1164 | 0.0000 | 0.0 | 598604 |
| logged-in-admin | mobile | /vi/admin/withdrawals | 33 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2980 | 824 | 824 | 508 | 828 | 0.0000 | 0.0 | 594721 |
| logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2873 | 828 | 829 | 560 | 924 | 0.0000 | 0.0 | 579199 |
| logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2852 | 757 | 774 | 536 | 904 | 0.0000 | 0.0 | 579322 |
| logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2972 | 813 | 942 | 552 | 952 | 0.0000 | 0.0 | 579267 |
| logged-in-admin | mobile | /vi/reading/session/a5e6f0fd-2d46-4d1d-8be2-8beaf1ff4265 | 31 | 7 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2689 | 669 | 670 | 464 | 816 | 0.0000 | 0.0 | 578586 |
| logged-in-admin | mobile | /vi/reading/session/c340407e-aa52-4043-9c09-19c5373f3355 | 31 | 7 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2751 | 735 | 735 | 472 | 816 | 0.0000 | 0.0 | 578491 |
| logged-in-admin | mobile | /vi/reading/session/0f8b2bd4-64df-469f-b41b-270623220ccb | 31 | 7 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2686 | 667 | 673 | 452 | 804 | 0.0000 | 0.0 | 578759 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 32 | 1 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2796 | 763 | 782 | 448 | 772 | 0.0000 | 0.0 | 579824 |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 32 | 1 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2897 | 880 | 881 | 492 | 864 | 0.0000 | 0.0 | 579527 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2764 | 749 | 749 | 468 | 804 | 0.0000 | 0.0 | 577527 |
| logged-in-reader | mobile | /vi | 29 | 8 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 2712 | 693 | 694 | 592 | 944 | 0.0020 | 0.0 | 482912 |
| logged-in-reader | mobile | /vi/login | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 2968 | 953 | 953 | 916 | 1268 | 0.0020 | 0.0 | 483078 |
| logged-in-reader | mobile | /vi/register | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3022 | 1010 | 1010 | 848 | 1204 | 0.0020 | 0.0 | 483187 |
| logged-in-reader | mobile | /vi/forgot-password | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3045 | 1032 | 1032 | 776 | 1136 | 0.0020 | 0.0 | 482984 |
| logged-in-reader | mobile | /vi/reset-password | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3042 | 1027 | 1027 | 892 | 1240 | 0.0020 | 0.0 | 483102 |
| logged-in-reader | mobile | /vi/verify-email | 30 | 8 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 3158 | 1144 | 1144 | 960 | 1312 | 0.0020 | 0.0 | 483145 |
| logged-in-reader | mobile | /vi/reading | 33 | 1 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2888 | 848 | 876 | 448 | 812 | 0.0000 | 0.0 | 590786 |
| logged-in-reader | mobile | /vi/inventory | 33 | 7 | high | 0 | 0 | 0 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2797 | 761 | 785 | 552 | 888 | 0.0000 | 0.0 | 590921 |
| logged-in-reader | mobile | /vi/gacha | 35 | 6 | high | 0 | 0 | 1 | 0 | 6 | 1 | 0 | 6 | 0 | 0 | 2862 | 845 | 845 | 444 | 1080 | 0.0000 | 0.0 | 675990 |
| logged-in-reader | mobile | /vi/gacha/history | 33 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2581 | 669 | 698 | 444 | 852 | 0.0000 | 0.0 | 671292 |
| logged-in-reader | mobile | /vi/collection | 32 | 27 | high | 0 | 0 | 0 | 0 | 12 | 3 | 0 | 12 | 0 | 0 | 5580 | 701 | 727 | 464 | 464 | 0.0000 | 0.0 | 588428 |
| logged-in-reader | mobile | /vi/profile | 36 | 2 | critical | 0 | 0 | 1 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 2960 | 685 | 947 | 456 | 964 | 0.0000 | 0.0 | 595740 |
| logged-in-reader | mobile | /vi/profile/mfa | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2816 | 764 | 806 | 480 | 812 | 0.0000 | 0.0 | 578242 |
| logged-in-reader | mobile | /vi/profile/reader | 31 | 4 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 2794 | 733 | 780 | 464 | 1264 | 0.0000 | 0.0 | 579028 |
| logged-in-reader | mobile | /vi/readers | 31 | 4 | high | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 2785 | 747 | 769 | 544 | 884 | 0.0000 | 0.0 | 580543 |
| logged-in-reader | mobile | /vi/chat | 31 | 8 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2726 | 689 | 712 | 480 | 808 | 0.0000 | 0.0 | 578497 |
| logged-in-reader | mobile | /vi/leaderboard | 33 | 5 | high | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 2 | 0 | 0 | 2695 | 654 | 681 | 468 | 800 | 0.0196 | 0.0 | 588915 |
| logged-in-reader | mobile | /vi/community | 32 | 12 | high | 0 | 0 | 0 | 0 | 3 | 1 | 0 | 3 | 0 | 0 | 3480 | 720 | 741 | 544 | 1692 | 0.0051 | 0.0 | 588852 |
| logged-in-reader | mobile | /vi/gamification | 34 | 2 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2837 | 819 | 819 | 488 | 1104 | 0.0000 | 0.0 | 593061 |
| logged-in-reader | mobile | /vi/wallet | 33 | 1 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2846 | 832 | 832 | 448 | 1104 | 0.0000 | 0.0 | 583304 |
| logged-in-reader | mobile | /vi/wallet/deposit | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2753 | 718 | 740 | 508 | 848 | 0.0000 | 0.0 | 578501 |
| logged-in-reader | mobile | /vi/wallet/deposit/history | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2745 | 703 | 730 | 520 | 868 | 0.0000 | 0.0 | 578406 |
| logged-in-reader | mobile | /vi/wallet/withdraw | 31 | 4 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2896 | 834 | 883 | 560 | 912 | 0.0000 | 0.0 | 579552 |
| logged-in-reader | mobile | /vi/notifications | 32 | 7 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2859 | 823 | 831 | 516 | 856 | 0.0000 | 0.0 | 581984 |
| logged-in-reader | mobile | /vi/reader/apply | 32 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2965 | 925 | 951 | 488 | 936 | 0.0000 | 0.0 | 580907 |
| logged-in-reader | mobile | /vi/reading/history | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 2755 | 714 | 739 | 552 | 980 | 0.0000 | 0.0 | 579483 |
| logged-in-reader | mobile | /vi/legal/tos | 28 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2789 | 773 | 773 | 480 | 808 | 0.0020 | 0.0 | 470111 |
| logged-in-reader | mobile | /vi/legal/privacy | 28 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2687 | 663 | 663 | 500 | 804 | 0.0020 | 0.0 | 470192 |
| logged-in-reader | mobile | /vi/legal/ai-disclaimer | 28 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2735 | 720 | 721 | 484 | 808 | 0.0020 | 0.0 | 470232 |
| logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2772 | 739 | 758 | 500 | 836 | 0.0000 | 0.0 | 579680 |
| logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2736 | 703 | 724 | 512 | 920 | 0.0000 | 0.0 | 579728 |
| logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2720 | 689 | 706 | 564 | 896 | 0.0000 | 0.0 | 579489 |
| logged-in-reader | mobile | /vi/reading/session/b07c677a-7de4-49ab-9637-faf1cacaf2e6 | 31 | 7 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2672 | 637 | 658 | 548 | 864 | 0.0000 | 0.0 | 579036 |
| logged-in-reader | mobile | /vi/reading/session/fd6abf66-e9b2-4196-b383-9b8a406b1085 | 31 | 6 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2717 | 702 | 704 | 480 | 820 | 0.0000 | 0.0 | 578809 |
| logged-in-reader | mobile | /vi/reading/session/721d3960-cd8f-477d-84c5-7b39eae75756 | 31 | 7 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2770 | 727 | 757 | 512 | 844 | 0.0000 | 0.0 | 578844 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2757 | 726 | 741 | 464 | 792 | 0.0000 | 0.0 | 577739 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2824 | 793 | 797 | 492 | 812 | 0.0000 | 0.0 | 577838 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2741 | 719 | 727 | 504 | 824 | 0.0000 | 0.0 | 577892 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 23 | 8 | 0 | 22 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 12 | 2 | 0 | 12 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 23 | 13 | 0 | 22 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 12 | 3 | 0 | 12 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | /vi | 33 | high | 0 | 31 | 0 |
| logged-out | desktop | /vi/legal/tos | 28 | high | 0 | 26 | 0 |
| logged-out | desktop | /vi/legal/privacy | 28 | high | 0 | 26 | 0 |
| logged-out | desktop | /vi/legal/ai-disclaimer | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | /vi | 33 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/login | 30 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | /vi/register | 30 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | /vi/forgot-password | 34 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/reset-password | 30 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | /vi/verify-email | 34 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/reading | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/inventory | 33 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/gacha | 35 | high | 1 | 32 | 0 |
| logged-in-admin | desktop | /vi/gacha/history | 33 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/collection | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/profile | 34 | high | 1 | 31 | 0 |
| logged-in-admin | desktop | /vi/profile/mfa | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/profile/reader | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/readers | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/chat | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/leaderboard | 34 | high | 1 | 31 | 0 |
| logged-in-admin | desktop | /vi/community | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/gamification | 33 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | /vi/wallet | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/wallet/deposit | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/wallet/deposit/history | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/wallet/withdraw | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/notifications | 33 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | /vi/reader/apply | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/reading/history | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/legal/tos | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | /vi/legal/privacy | 29 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | /vi/legal/ai-disclaimer | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | /vi/admin | 33 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/admin/deposits | 33 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/admin/disputes | 33 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/admin/gamification | 37 | critical | 1 | 34 | 0 |
| logged-in-admin | desktop | /vi/admin/promotions | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/admin/reader-requests | 33 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/admin/readings | 34 | high | 1 | 31 | 0 |
| logged-in-admin | desktop | /vi/admin/system-configs | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/admin/users | 35 | high | 0 | 33 | 0 |
| logged-in-admin | desktop | /vi/admin/withdrawals | 33 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 32 | high | 1 | 29 | 0 |
| logged-in-admin | desktop | /vi/reading/session/0cbaad24-c06d-491f-8c2b-d0e5103f100d | 38 | critical | 2 | 34 | 0 |
| logged-in-admin | desktop | /vi/reading/session/70caef91-ed5b-474c-b4a3-4675414c483d | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/reading/session/33a23dc7-8b59-4385-b20c-467d52f8a45e | 35 | high | 0 | 33 | 0 |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 32 | high | 1 | 29 | 0 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi | 33 | high | 0 | 31 | 0 |
| logged-in-reader | desktop | /vi/login | 30 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | /vi/register | 30 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | /vi/forgot-password | 34 | high | 0 | 31 | 0 |
| logged-in-reader | desktop | /vi/reset-password | 30 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | /vi/verify-email | 30 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | /vi/reading | 33 | high | 1 | 30 | 0 |
| logged-in-reader | desktop | /vi/inventory | 33 | high | 0 | 31 | 0 |
| logged-in-reader | desktop | /vi/gacha | 39 | critical | 1 | 36 | 0 |
| logged-in-reader | desktop | /vi/gacha/history | 34 | high | 1 | 31 | 0 |
| logged-in-reader | desktop | /vi/collection | 32 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/profile | 35 | high | 1 | 31 | 0 |
| logged-in-reader | desktop | /vi/profile/mfa | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/profile/reader | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/readers | 32 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/chat | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/leaderboard | 34 | high | 1 | 31 | 0 |
| logged-in-reader | desktop | /vi/community | 32 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/gamification | 34 | high | 1 | 31 | 0 |
| logged-in-reader | desktop | /vi/wallet | 32 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/wallet/deposit | 32 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/wallet/deposit/history | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/wallet/withdraw | 33 | high | 1 | 30 | 0 |
| logged-in-reader | desktop | /vi/notifications | 32 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/reader/apply | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/reading/history | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/legal/tos | 30 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | /vi/legal/privacy | 30 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | /vi/legal/ai-disclaimer | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 32 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/reading/session/a24a62fe-e811-43e6-9181-2d98e5388673 | 38 | critical | 2 | 34 | 0 |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | 37 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/reading/session/9c624020-f4ae-4422-9c39-a7c852858fd9 | 37 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | high | 0 | 29 | 0 |
| logged-out | mobile | /vi | 29 | high | 0 | 27 | 0 |
| logged-out | mobile | /vi/legal/tos | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | /vi/legal/privacy | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | /vi/legal/ai-disclaimer | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | /vi/login | 34 | high | 0 | 31 | 0 |
| logged-in-admin | mobile | /vi/register | 30 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | /vi/forgot-password | 37 | critical | 1 | 32 | 0 |
| logged-in-admin | mobile | /vi/reset-password | 30 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | /vi/verify-email | 34 | high | 0 | 31 | 0 |
| logged-in-admin | mobile | /vi/reading | 32 | high | 1 | 29 | 0 |
| logged-in-admin | mobile | /vi/inventory | 33 | high | 0 | 31 | 0 |
| logged-in-admin | mobile | /vi/gacha | 40 | critical | 1 | 37 | 0 |
| logged-in-admin | mobile | /vi/gacha/history | 34 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/collection | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/profile | 34 | high | 1 | 31 | 0 |
| logged-in-admin | mobile | /vi/profile/mfa | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/profile/reader | 32 | high | 1 | 29 | 0 |
| logged-in-admin | mobile | /vi/readers | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/chat | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/leaderboard | 33 | high | 0 | 31 | 0 |
| logged-in-admin | mobile | /vi/community | 40 | critical | 2 | 36 | 0 |
| logged-in-admin | mobile | /vi/gamification | 34 | high | 1 | 31 | 0 |
| logged-in-admin | mobile | /vi/wallet | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/wallet/deposit | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/wallet/withdraw | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/notifications | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/reader/apply | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/reading/history | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/legal/tos | 30 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | /vi/legal/privacy | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | /vi/legal/ai-disclaimer | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | /vi/admin | 33 | high | 0 | 31 | 0 |
| logged-in-admin | mobile | /vi/admin/deposits | 33 | high | 0 | 31 | 0 |
| logged-in-admin | mobile | /vi/admin/disputes | 33 | high | 0 | 31 | 0 |
| logged-in-admin | mobile | /vi/admin/gamification | 36 | critical | 0 | 34 | 0 |
| logged-in-admin | mobile | /vi/admin/promotions | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/admin/reader-requests | 33 | high | 0 | 31 | 0 |
| logged-in-admin | mobile | /vi/admin/readings | 34 | high | 1 | 31 | 0 |
| logged-in-admin | mobile | /vi/admin/system-configs | 34 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/admin/users | 34 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/admin/withdrawals | 33 | high | 0 | 31 | 0 |
| logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/reading/session/a5e6f0fd-2d46-4d1d-8be2-8beaf1ff4265 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/reading/session/c340407e-aa52-4043-9c09-19c5373f3355 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/reading/session/0f8b2bd4-64df-469f-b41b-270623220ccb | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | /vi/login | 30 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | /vi/register | 30 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | /vi/forgot-password | 30 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | /vi/reset-password | 30 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | /vi/verify-email | 30 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | /vi/reading | 33 | high | 1 | 30 | 0 |
| logged-in-reader | mobile | /vi/inventory | 33 | high | 0 | 31 | 0 |
| logged-in-reader | mobile | /vi/gacha | 35 | high | 1 | 32 | 0 |
| logged-in-reader | mobile | /vi/gacha/history | 33 | high | 0 | 31 | 0 |
| logged-in-reader | mobile | /vi/collection | 32 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/profile | 36 | critical | 2 | 31 | 0 |
| logged-in-reader | mobile | /vi/profile/mfa | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/profile/reader | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/readers | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/chat | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/leaderboard | 33 | high | 0 | 31 | 0 |
| logged-in-reader | mobile | /vi/community | 32 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/gamification | 34 | high | 1 | 31 | 0 |
| logged-in-reader | mobile | /vi/wallet | 33 | high | 1 | 30 | 0 |
| logged-in-reader | mobile | /vi/wallet/deposit | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/wallet/deposit/history | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/wallet/withdraw | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/notifications | 32 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/reader/apply | 32 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/reading/history | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/legal/tos | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | /vi/legal/privacy | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | /vi/legal/ai-disclaimer | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/reading/session/b07c677a-7de4-49ab-9637-faf1cacaf2e6 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/reading/session/fd6abf66-e9b2-4196-b383-9b8a406b1085 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/reading/session/721d3960-cd8f-477d-84c5-7b39eae75756 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | high | 0 | 29 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | desktop | /vi/wallet | GET | 200 | 1561 | 1081 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1552 | 321 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-out | desktop | /vi/login | GET | 200 | 1429 | 1061 | html | https://www.tarotnow.xyz/vi/login |
| logged-in-admin | desktop | /vi/gacha | GET | 200 | 1294 | 325 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 1292 | 315 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 1224 | 343 | html | https://www.tarotnow.xyz/vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1137 | 101 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1136 | 63 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1116 | 84 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1114 | 87 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1113 | 81 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg6ntv_3jdd4.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1108 | 64 | static | https://www.tarotnow.xyz/_next/static/chunks/0ia1rui7sf6...css |
| logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1089 | 327 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1082 | 62 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | /vi/reading/session/9c624020-f4ae-4422-9c39-a7c852858fd9 | GET | 200 | 1079 | 411 | html | https://www.tarotnow.xyz/vi/reading/session/9c624020-f4ae-4422-9c39-a7c852858fd9 |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1078 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1077 | 76 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1073 | 76 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-out | desktop | /vi | GET | 200 | 1065 | 339 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | /vi/gamification | GET | 200 | 1061 | 311 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | /vi/gacha | GET | 200 | 1061 | 341 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1045 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | desktop | /vi/reading/session/0cbaad24-c06d-491f-8c2b-d0e5103f100d | GET | 200 | 1043 | 402 | html | https://www.tarotnow.xyz/vi/reading/session/0cbaad24-c06d-491f-8c2b-d0e5103f100d |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1033 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1026 | 357 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 1025 | 416 | html | https://www.tarotnow.xyz/vi/admin |
| logged-out | mobile | /vi | GET | 200 | 1010 | 699 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | /vi/gamification | GET | 200 | 998 | 427 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | /vi/wallet | GET | 200 | 990 | 398 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 982 | 93 | static | https://www.tarotnow.xyz/_next/static/chunks/09njxa758vvw_.js |
| logged-in-reader | desktop | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 960 | 436 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-reader | desktop | /vi/wallet | GET | 200 | 959 | 316 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 943 | 70 | static | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 937 | 118 | static | https://www.tarotnow.xyz/_next/static/chunks/013oon8u7jcs1.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 924 | 87 | static | https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 920 | 115 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 918 | 83 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 914 | 80 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | /vi/legal/tos | GET | 200 | 908 | 335 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | mobile | /vi/wallet/deposit | GET | 200 | 901 | 317 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | mobile | /vi/admin/deposits | GET | 200 | 900 | 433 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-admin | mobile | /vi/admin/users | GET | 200 | 900 | 305 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-admin | mobile | /vi/community | GET | 200 | 899 | 563 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | /vi/gacha | GET | 200 | 894 | 632 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| logged-in-admin | desktop | /vi/admin/promotions | GET | 200 | 892 | 531 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 876 | 86 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 875 | 626 | static | https://www.tarotnow.xyz/_next/static/chunks/0vsq~5x0h0e14.js |
| logged-in-admin | desktop | /vi/leaderboard | GET | 200 | 873 | 450 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | /vi/reader/apply | GET | 200 | 872 | 311 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 863 | 88 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 861 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 859 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 853 | 74 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 853 | 87 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | desktop | /vi/gacha | GET | 200 | 844 | 432 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 844 | 67 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg6ntv_3jdd4.js |
| logged-in-admin | desktop | /vi/admin/readings | GET | 200 | 843 | 458 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 843 | 87 | static | https://www.tarotnow.xyz/_next/static/chunks/09njxa758vvw_.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 841 | 64 | static | https://www.tarotnow.xyz/_next/static/chunks/105-15pf-dz_l.js |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 838 | 88 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | /vi/verify-email | GET | 200 | 837 | 323 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 835 | 80 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 835 | 69 | static | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-admin | mobile | /vi/leaderboard | GET | 200 | 831 | 434 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 829 | 67 | static | https://www.tarotnow.xyz/_next/static/chunks/013oon8u7jcs1.js |
| logged-in-admin | mobile | /vi/readers | GET | 200 | 821 | 355 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | /vi/admin/promotions | GET | 200 | 821 | 318 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-admin | mobile | /vi/reading/history | GET | 200 | 820 | 319 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 818 | 362 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 815 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 815 | 326 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | /vi/reading | GET | 200 | 814 | 310 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | /vi/reading | GET | 200 | 812 | 328 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 810 | 90 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-reader | desktop | /vi/wallet/deposit | GET | 200 | 808 | 326 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | /vi/reading/session/33a23dc7-8b59-4385-b20c-467d52f8a45e | GET | 200 | 805 | 394 | html | https://www.tarotnow.xyz/vi/reading/session/33a23dc7-8b59-4385-b20c-467d52f8a45e |
| logged-in-reader | desktop | /vi/reading | GET | 200 | 805 | 369 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 805 | 72 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | /vi/wallet/withdraw | GET | 200 | 803 | 412 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | desktop | /vi/gacha/history | GET | 200 | 799 | 453 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 798 | 352 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 795 | 67 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | /vi/admin/gamification | GET | 200 | 794 | 336 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | mobile | /vi/wallet | GET | 200 | 788 | 314 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 787 | 317 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | /vi/admin/system-configs | GET | 200 | 785 | 370 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 781 | 72 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | GET | 200 | 777 | 315 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 777 | 385 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-reader | mobile | /vi/reading | GET | 200 | 777 | 309 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | /vi/gamification | GET | 200 | 777 | 321 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | /vi/legal/privacy | GET | 200 | 776 | 322 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-reader | mobile | /vi/gacha | GET | 200 | 774 | 316 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 773 | 87 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | /vi/gamification | GET | 200 | 771 | 313 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | /vi/admin/users | GET | 200 | 769 | 370 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-admin | mobile | /vi/community | GET | 200 | 769 | 294 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 767 | 310 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 765 | 59 | static | https://www.tarotnow.xyz/_next/static/chunks/013oon8u7jcs1.js |
| logged-in-admin | desktop | /vi/notifications | GET | 200 | 764 | 329 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 764 | 64 | static | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-out | desktop | /vi/legal/ai-disclaimer | GET | 200 | 756 | 373 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | mobile | /vi/reset-password | GET | 200 | 756 | 413 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | /vi/inventory | GET | 200 | 755 | 343 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 753 | 64 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 752 | 369 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | mobile | /vi/admin/withdrawals | GET | 200 | 750 | 310 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-reader | desktop | /vi/wallet/withdraw | GET | 200 | 746 | 306 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | /vi/profile/mfa | GET | 200 | 746 | 445 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 745 | 64 | static | https://www.tarotnow.xyz/_next/static/chunks/09njxa758vvw_.js |
| logged-in-reader | desktop | /vi/legal/privacy | GET | 200 | 743 | 446 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 742 | 66 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | /vi/gacha/history | GET | 200 | 741 | 393 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 740 | 66 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 740 | 90 | static | https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 738 | 64 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 735 | 320 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | mobile | /vi | GET | 200 | 734 | 361 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 733 | 358 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 731 | 75 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 731 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 731 | 404 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 729 | 313 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | /vi/verify-email | GET | 200 | 728 | 433 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | /vi/login | GET | 200 | 727 | 413 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | /vi | GET | 200 | 722 | 529 | html | https://www.tarotnow.xyz/vi |
| logged-out | desktop | /vi/legal/tos | GET | 200 | 720 | 327 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | desktop | /vi/reset-password | GET | 200 | 719 | 333 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 719 | 418 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | /vi/reading/session/9c624020-f4ae-4422-9c39-a7c852858fd9 | GET | 200 | 718 | 113 | static | https://www.tarotnow.xyz/_next/static/chunks/013oon8u7jcs1.js |
| logged-in-reader | desktop | /vi/leaderboard | GET | 200 | 715 | 345 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | desktop | /vi/reading/session/9c624020-f4ae-4422-9c39-a7c852858fd9 | GET | 200 | 713 | 97 | static | https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-reader | mobile | /vi/inventory | GET | 200 | 712 | 384 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 711 | 342 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | /vi/profile | GET | 200 | 710 | 412 | html | https://www.tarotnow.xyz/vi/profile |
| logged-out | mobile | /vi/verify-email | GET | 200 | 710 | 403 | html | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-reader | mobile | /vi/profile/mfa | GET | 200 | 709 | 314 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | desktop | /vi/reading/session/a24a62fe-e811-43e6-9181-2d98e5388673 | GET | 200 | 708 | 307 | html | https://www.tarotnow.xyz/vi/reading/session/a24a62fe-e811-43e6-9181-2d98e5388673 |
| logged-in-admin | desktop | /vi/admin/reader-requests | GET | 200 | 707 | 395 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-reader | desktop | /vi/reading/session/9c624020-f4ae-4422-9c39-a7c852858fd9 | GET | 200 | 707 | 61 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 706 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | /vi/community | GET | 200 | 704 | 360 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 702 | 86 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 702 | 72 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | desktop | /vi/reading/session/9c624020-f4ae-4422-9c39-a7c852858fd9 | GET | 200 | 702 | 103 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 701 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg6ntv_3jdd4.js |
| logged-in-admin | mobile | /vi/profile | GET | 200 | 701 | 409 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 700 | 319 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | /vi/readers | GET | 200 | 700 | 410 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | /vi/wallet/deposit | GET | 200 | 699 | 319 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 699 | 322 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | /vi/reading/history | GET | 200 | 698 | 405 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 697 | 60 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | /vi/reader/apply | GET | 200 | 696 | 353 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 696 | 333 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | mobile | /vi/profile/reader | GET | 200 | 695 | 314 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | desktop | /vi/admin/system-configs | GET | 200 | 694 | 403 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 694 | 318 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | /vi/reading/session/70caef91-ed5b-474c-b4a3-4675414c483d | GET | 200 | 694 | 366 | html | https://www.tarotnow.xyz/vi/reading/session/70caef91-ed5b-474c-b4a3-4675414c483d |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| - | - | - | - |

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
| logged-in-admin | desktop | reading.init.spread_3: created 0cbaad24-c06d-491f-8c2b-d0e5103f100d. |
| logged-in-admin | desktop | reading.init.spread_5: created 70caef91-ed5b-474c-b4a3-4675414c483d. |
| logged-in-admin | desktop | reading.init.spread_10: created 33a23dc7-8b59-4385-b20c-467d52f8a45e. |
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
| logged-in-reader | desktop | reading.init.spread_3: created a24a62fe-e811-43e6-9181-2d98e5388673. |
| logged-in-reader | desktop | reading.init.spread_5: created 0f201915-be10-483e-86d4-9e5e4381072f. |
| logged-in-reader | desktop | reading.init.spread_10: created 9c624020-f4ae-4422-9c39-a7c852858fd9. |
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
| logged-in-admin | mobile | reading.init.spread_3: created a5e6f0fd-2d46-4d1d-8be2-8beaf1ff4265. |
| logged-in-admin | mobile | reading.init.spread_5: created c340407e-aa52-4043-9c09-19c5373f3355. |
| logged-in-admin | mobile | reading.init.spread_10: created 0f8b2bd4-64df-469f-b41b-270623220ccb. |
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
| logged-in-reader | mobile | reading.init.spread_3: created b07c677a-7de4-49ab-9637-faf1cacaf2e6. |
| logged-in-reader | mobile | reading.init.spread_5: created fd6abf66-e9b2-4196-b383-9b8a406b1085. |
| logged-in-reader | mobile | reading.init.spread_10: created 721d3960-cd8f-477d-84c5-7b39eae75756. |
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

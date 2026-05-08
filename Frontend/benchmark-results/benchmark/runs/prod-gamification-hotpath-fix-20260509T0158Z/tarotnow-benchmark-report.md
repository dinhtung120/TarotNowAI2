# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T19:08:23.915Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 6
- High pages (request count): 142
- High slow requests: 30
- Medium slow requests: 188

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2825 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 2941 | 1274 | 0 | 0 | 15 | 0 | yes |
| logged-in-reader | desktop | 33 | 2936 | 977 | 0 | 0 | 17 | 1 | yes |
| logged-out | mobile | 9 | 2774 | 222 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 2941 | 1320 | 0 | 0 | 11 | 0 | yes |
| logged-in-reader | mobile | 33 | 2969 | 1002 | 0 | 0 | 22 | 1 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 2887 | 0 |
| logged-in-admin | desktop | chat | 1 | 30.0 | 2885 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 5953 | 0 |
| logged-in-admin | desktop | community | 1 | 36.0 | 3936 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.5 | 2865 | 0 |
| logged-in-admin | desktop | gamification | 1 | 31.0 | 3270 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2680 | 0 |
| logged-in-admin | desktop | inventory | 1 | 33.0 | 2870 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 3246 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2750 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2712 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2830 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2767 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.4 | 2748 | 0 |
| logged-in-admin | desktop | reading | 5 | 28.6 | 2747 | 0 |
| logged-in-admin | desktop | wallet | 4 | 35.0 | 2955 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.4 | 2784 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2780 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5623 | 0 |
| logged-in-admin | mobile | community | 1 | 30.0 | 3564 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.5 | 2865 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2878 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2772 | 0 |
| logged-in-admin | mobile | inventory | 1 | 32.0 | 2884 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 33.0 | 2922 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2684 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2744 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 2846 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2947 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2761 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.6 | 2852 | 0 |
| logged-in-admin | mobile | wallet | 4 | 48.0 | 3400 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2752 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6231 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3428 | 0 |
| logged-in-reader | desktop | gacha | 2 | 33.0 | 2849 | 0 |
| logged-in-reader | desktop | gamification | 1 | 30.0 | 2893 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2712 | 0 |
| logged-in-reader | desktop | inventory | 1 | 32.0 | 2837 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 32.0 | 2860 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2657 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2843 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.7 | 2945 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2660 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.0 | 2778 | 0 |
| logged-in-reader | desktop | reading | 5 | 28.6 | 2725 | 0 |
| logged-in-reader | desktop | wallet | 4 | 36.0 | 3023 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2698 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5597 | 0 |
| logged-in-reader | mobile | community | 1 | 30.0 | 3580 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.0 | 2870 | 0 |
| logged-in-reader | mobile | gamification | 1 | 31.0 | 2909 | 0 |
| logged-in-reader | mobile | home | 1 | 35.0 | 2864 | 0 |
| logged-in-reader | mobile | inventory | 1 | 33.0 | 2845 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 32.0 | 2851 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2705 | 0 |
| logged-in-reader | mobile | notifications | 1 | 31.0 | 2884 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.7 | 3064 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2750 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.9 | 2774 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.8 | 2854 | 0 |
| logged-in-reader | mobile | wallet | 4 | 36.0 | 3066 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2702 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3772 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2716 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2722 | 0 |
| logged-out | mobile | home | 1 | 26.0 | 3090 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2756 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3772 | 1091 | 1748 | 1080 | 1080 | 0.0000 | 527.0 | 601820 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2695 | 685 | 685 | 620 | 620 | 0.0000 | 0.0 | 512529 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 721 | 722 | 532 | 532 | 0.0000 | 0.0 | 512979 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2765 | 758 | 758 | 564 | 564 | 0.0000 | 0.0 | 512061 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2646 | 639 | 639 | 512 | 512 | 0.0000 | 0.0 | 512224 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2672 | 665 | 665 | 488 | 488 | 0.0000 | 0.0 | 512177 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2699 | 689 | 689 | 488 | 796 | 0.0000 | 0.0 | 526108 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2767 | 759 | 759 | 496 | 512 | 0.0000 | 0.0 | 526131 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2681 | 670 | 670 | 468 | 796 | 0.0000 | 0.0 | 526176 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2680 | 673 | 673 | 492 | 1068 | 0.0035 | 191.0 | 537790 |
| logged-in-admin | desktop | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2881 | 868 | 869 | 480 | 916 | 0.0041 | 0.0 | 645397 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 33 | 8 | high | 0 | 0 | 2 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2870 | 862 | 862 | 496 | 1124 | 0.0041 | 0.0 | 648112 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2876 | 869 | 869 | 480 | 1176 | 0.0041 | 0.0 | 727585 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2854 | 837 | 838 | 484 | 1196 | 0.0041 | 0.0 | 727626 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 28 | high | 0 | 0 | 0 | 0 | 13 | 4 | 3 | 12 | 1 | 0 | 5953 | 675 | 688 | 480 | 480 | 0.0042 | 0.0 | 643624 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3127 | 924 | 1121 | 560 | 1228 | 0.0489 | 0.0 | 636939 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2669 | 652 | 661 | 468 | 868 | 0.0041 | 0.0 | 631851 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 40 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2694 | 845 | 1010 | 540 | 1192 | 0.0489 | 0.0 | 631359 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2856 | 850 | 850 | 496 | 1120 | 0.0041 | 0.0 | 637411 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2885 | 856 | 879 | 564 | 956 | 0.0041 | 0.0 | 634228 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3246 | 887 | 1240 | 552 | 1208 | 0.0041 | 0.0 | 654020 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 36 | 4 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3936 | 706 | 1180 | 504 | 1880 | 0.0041 | 0.0 | 779541 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3270 | 853 | 1261 | 520 | 944 | 0.0279 | 0.0 | 645736 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3681 | 975 | 976 | 792 | 1348 | 0.0000 | 28.0 | 1135673 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2686 | 666 | 678 | 504 | 888 | 0.0041 | 0.0 | 631998 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2713 | 698 | 704 | 528 | 1092 | 0.0041 | 0.0 | 632614 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2740 | 845 | 845 | 516 | 1192 | 0.0041 | 0.0 | 631343 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2712 | 692 | 702 | 472 | 896 | 0.0046 | 0.0 | 632378 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2767 | 751 | 761 | 548 | 928 | 0.0041 | 0.0 | 632825 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2677 | 665 | 671 | 460 | 1044 | 0.0041 | 0.0 | 633131 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2956 | 945 | 945 | 764 | 1092 | 0.0020 | 0.0 | 526311 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2668 | 660 | 660 | 492 | 788 | 0.0020 | 0.0 | 526384 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2627 | 617 | 617 | 476 | 836 | 0.0020 | 0.0 | 526494 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3161 | 722 | 1149 | 516 | 844 | 0.0000 | 0.0 | 647669 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2763 | 745 | 748 | 508 | 956 | 0.0000 | 0.0 | 648025 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2918 | 905 | 912 | 576 | 952 | 0.0000 | 0.0 | 646278 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3228 | 717 | 1223 | 608 | 940 | 0.0022 | 0.0 | 699558 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2841 | 828 | 834 | 528 | 860 | 0.0000 | 0.0 | 645012 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2729 | 710 | 722 | 500 | 832 | 0.0000 | 0.0 | 646738 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2787 | 780 | 780 | 552 | 1028 | 0.0000 | 0.0 | 649132 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 754 | 763 | 588 | 1000 | 0.0000 | 0.0 | 656140 |
| logged-in-admin | desktop | admin | /vi/admin/users | 31 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2844 | 695 | 837 | 500 | 824 | 0.0000 | 0.0 | 651839 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2832 | 808 | 824 | 672 | 1000 | 0.0000 | 0.0 | 646357 |
| logged-in-admin | desktop | reading | /vi/reading/session/26b570f0-4558-4324-a02e-e6dfb0eaddb5 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2785 | 767 | 776 | 608 | 948 | 0.0041 | 0.0 | 632518 |
| logged-in-admin | desktop | reading | /vi/reading/session/afebe741-090f-48e7-bf9a-5bce862c48c6 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2722 | 705 | 714 | 620 | 1000 | 0.0041 | 0.0 | 632451 |
| logged-in-admin | desktop | reading | /vi/reading/session/4c16b9de-bc17-421c-8c51-1c8e070a0fed | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2670 | 650 | 661 | 448 | 828 | 0.0041 | 0.0 | 632572 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 802 | 810 | 488 | 888 | 0.0041 | 0.0 | 631422 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2724 | 704 | 716 | 620 | 968 | 0.0041 | 0.0 | 631564 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2807 | 788 | 796 | 480 | 880 | 0.0041 | 0.0 | 631711 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2657 | 638 | 648 | 496 | 1068 | 0.0041 | 0.0 | 633265 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2682 | 665 | 672 | 488 | 992 | 0.0041 | 0.0 | 633143 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2692 | 672 | 683 | 512 | 1072 | 0.0041 | 0.0 | 633498 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2712 | 703 | 703 | 564 | 1152 | 0.0038 | 244.0 | 537780 |
| logged-in-reader | desktop | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2896 | 865 | 888 | 560 | 932 | 0.0039 | 0.0 | 645280 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2837 | 830 | 830 | 496 | 1124 | 0.0039 | 0.0 | 647192 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2862 | 852 | 853 | 500 | 1148 | 0.0039 | 0.0 | 728490 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2836 | 826 | 826 | 452 | 1180 | 0.0039 | 0.0 | 727850 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 25 | high | 0 | 0 | 0 | 0 | 6 | 0 | 0 | 6 | 0 | 0 | 6231 | 800 | 807 | 588 | 588 | 0.0040 | 0.0 | 642625 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 32 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3251 | 1184 | 1245 | 456 | 1356 | 0.0726 | 0.0 | 637804 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2756 | 742 | 749 | 532 | 880 | 0.0039 | 0.0 | 632023 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2829 | 797 | 821 | 492 | 1296 | 0.0039 | 0.0 | 633431 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3177 | 1119 | 1169 | 1172 | 1172 | 0.0039 | 0.0 | 634255 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2752 | 740 | 741 | 532 | 932 | 0.0039 | 0.0 | 632285 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2860 | 850 | 850 | 508 | 1132 | 0.0039 | 0.0 | 653547 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | 9 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3428 | 683 | 691 | 488 | 1600 | 0.0039 | 0.0 | 643386 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 30 | 7 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2893 | 880 | 884 | 640 | 1056 | 0.0277 | 0.0 | 643811 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 31 | 4 | high | 0 | 0 | 2 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 2869 | 859 | 859 | 504 | 1184 | 0.0039 | 0.0 | 637676 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3676 | 982 | 982 | 884 | 1476 | 0.0000 | 50.0 | 1135675 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2678 | 667 | 669 | 484 | 840 | 0.0039 | 0.0 | 631894 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2868 | 856 | 856 | 504 | 944 | 0.0095 | 0.0 | 634207 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2843 | 833 | 833 | 616 | 1024 | 0.0040 | 0.0 | 633660 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2660 | 644 | 652 | 488 | 868 | 0.0039 | 0.0 | 632797 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2637 | 622 | 628 | 492 | 996 | 0.0039 | 0.0 | 633068 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2615 | 607 | 607 | 480 | 816 | 0.0019 | 0.0 | 526381 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2721 | 712 | 712 | 500 | 808 | 0.0019 | 0.0 | 526344 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2636 | 627 | 627 | 472 | 780 | 0.0019 | 0.0 | 526547 |
| logged-in-reader | desktop | reading | /vi/reading/session/e4e4d714-6db9-49aa-a887-2f8c123d24d0 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2670 | 651 | 659 | 484 | 844 | 0.0039 | 0.0 | 632367 |
| logged-in-reader | desktop | reading | /vi/reading/session/3f406a9c-c3e5-4b16-a767-cb3071b071a4 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2742 | 727 | 733 | 484 | 856 | 0.0048 | 0.0 | 632437 |
| logged-in-reader | desktop | reading | /vi/reading/session/1dc68087-4b2a-4437-80f6-85788f11f876 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2681 | 670 | 671 | 496 | 860 | 0.0039 | 0.0 | 632756 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2721 | 705 | 712 | 500 | 868 | 0.0039 | 0.0 | 631744 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2742 | 732 | 734 | 536 | 904 | 0.0039 | 0.0 | 631477 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2664 | 653 | 655 | 468 | 856 | 0.0039 | 0.0 | 631371 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2705 | 693 | 696 | 512 | 1104 | 0.0039 | 0.0 | 632969 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2708 | 698 | 698 | 472 | 1048 | 0.0039 | 0.0 | 633066 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 707 | 717 | 504 | 1088 | 0.0039 | 0.0 | 633484 |
| logged-out | mobile | auth-public | /vi | 26 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3090 | 1081 | 1081 | 824 | 1180 | 0.0000 | 0.0 | 531173 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2683 | 657 | 677 | 580 | 580 | 0.0000 | 0.0 | 512576 |
| logged-out | mobile | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2713 | 703 | 703 | 476 | 476 | 0.0000 | 0.0 | 513615 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2668 | 468 | 660 | 480 | 480 | 0.0000 | 0.0 | 512023 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 854 | 854 | 684 | 684 | 0.0000 | 0.0 | 512210 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2682 | 672 | 672 | 456 | 456 | 0.0000 | 0.0 | 512344 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2699 | 682 | 682 | 580 | 912 | 0.0000 | 0.0 | 526066 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2785 | 763 | 763 | 492 | 800 | 0.0000 | 0.0 | 526036 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2785 | 772 | 773 | 488 | 796 | 0.0000 | 0.0 | 526172 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2772 | 760 | 760 | 584 | 948 | 0.0032 | 0.0 | 537827 |
| logged-in-admin | mobile | reading | /vi/reading | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2874 | 853 | 862 | 476 | 840 | 0.0000 | 0.0 | 644322 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 1 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 2884 | 876 | 876 | 488 | 1112 | 0.0000 | 0.0 | 646894 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 33 | 9 | high | 0 | 0 | 2 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2874 | 864 | 864 | 480 | 1112 | 0.0000 | 0.0 | 728327 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2855 | 841 | 841 | 460 | 836 | 0.0000 | 0.0 | 726393 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 27 | high | 0 | 0 | 0 | 0 | 13 | 5 | 0 | 12 | 1 | 0 | 5623 | 727 | 759 | 476 | 476 | 0.0000 | 0.0 | 643386 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3102 | 909 | 1093 | 488 | 1104 | 0.0689 | 0.0 | 636666 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2698 | 656 | 686 | 448 | 784 | 0.0000 | 0.0 | 631765 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 40 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2738 | 882 | 995 | 496 | 1128 | 0.0760 | 0.0 | 630993 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2880 | 868 | 869 | 468 | 1080 | 0.0000 | 0.0 | 636425 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2780 | 697 | 727 | 468 | 808 | 0.0000 | 0.0 | 631853 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2922 | 882 | 882 | 504 | 1144 | 0.0000 | 0.0 | 654421 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3564 | 798 | 821 | 608 | 2036 | 0.0051 | 0.0 | 643312 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2878 | 844 | 863 | 696 | 1036 | 0.0000 | 0.0 | 643193 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3655 | 992 | 992 | 784 | 1156 | 0.0000 | 0.0 | 1136298 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 80 | 13 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4377 | 641 | 642 | 444 | 808 | 0.0055 | 0.0 | 1636384 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2750 | 710 | 739 | 472 | 876 | 0.0000 | 0.0 | 632683 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2818 | 855 | 855 | 604 | 944 | 0.0071 | 0.0 | 631469 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2744 | 704 | 732 | 556 | 892 | 0.0000 | 0.0 | 632272 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2947 | 907 | 936 | 680 | 1008 | 0.0000 | 0.0 | 632731 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2775 | 729 | 760 | 476 | 812 | 0.0000 | 0.0 | 632899 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2707 | 696 | 696 | 492 | 816 | 0.0032 | 0.0 | 526433 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2643 | 631 | 631 | 444 | 756 | 0.0032 | 0.0 | 526456 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2701 | 689 | 689 | 460 | 800 | 0.0032 | 0.0 | 526448 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2764 | 745 | 752 | 492 | 824 | 0.0000 | 0.0 | 647911 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2829 | 784 | 815 | 504 | 860 | 0.0000 | 0.0 | 647869 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2800 | 757 | 785 | 472 | 800 | 0.0000 | 0.0 | 646138 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2865 | 715 | 849 | 460 | 780 | 0.0000 | 0.0 | 698596 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2752 | 716 | 741 | 484 | 796 | 0.0000 | 0.0 | 644890 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2703 | 661 | 691 | 464 | 776 | 0.0000 | 0.0 | 646745 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2690 | 655 | 679 | 544 | 880 | 0.0000 | 0.0 | 649007 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2875 | 731 | 866 | 452 | 796 | 0.0000 | 0.0 | 689434 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2779 | 766 | 766 | 464 | 1104 | 0.0000 | 0.0 | 650242 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2778 | 743 | 767 | 476 | 784 | 0.0000 | 0.0 | 646256 |
| logged-in-admin | mobile | reading | /vi/reading/session/eee9d192-5b43-469f-bd46-90f4c92190ed | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2790 | 675 | 696 | 452 | 788 | 0.0000 | 0.0 | 632470 |
| logged-in-admin | mobile | reading | /vi/reading/session/4196f2b5-4f7c-4c80-9f3b-e2d33568d001 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2968 | 833 | 952 | 508 | 840 | 0.0000 | 0.0 | 695262 |
| logged-in-admin | mobile | reading | /vi/reading/session/62c8bf21-3570-40b4-a4a1-0ae93d490b7b | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2854 | 814 | 842 | 552 | 888 | 0.0000 | 0.0 | 632253 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2751 | 716 | 736 | 492 | 824 | 0.0000 | 0.0 | 631616 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2744 | 698 | 729 | 480 | 816 | 0.0000 | 0.0 | 631352 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2708 | 670 | 695 | 560 | 880 | 0.0000 | 0.0 | 631341 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2713 | 674 | 702 | 444 | 800 | 0.0000 | 0.0 | 633363 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2765 | 658 | 688 | 452 | 844 | 0.0000 | 0.0 | 632950 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2766 | 735 | 755 | 500 | 840 | 0.0000 | 0.0 | 633372 |
| logged-in-reader | mobile | auth-public | /vi | 35 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 747 | 854 | 452 | 808 | 0.0032 | 0.0 | 613411 |
| logged-in-reader | mobile | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2894 | 866 | 870 | 432 | 856 | 0.0000 | 0.0 | 645603 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 33 | 8 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2845 | 831 | 831 | 412 | 1052 | 0.0000 | 0.0 | 648144 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 33 | 9 | high | 0 | 0 | 2 | 0 | 5 | 2 | 0 | 5 | 0 | 0 | 2855 | 841 | 841 | 420 | 1076 | 0.0000 | 0.0 | 728556 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2884 | 867 | 867 | 448 | 1116 | 0.0000 | 0.0 | 727614 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 25 | high | 0 | 0 | 0 | 0 | 6 | 4 | 0 | 6 | 0 | 0 | 5597 | 783 | 783 | 464 | 780 | 0.0000 | 0.0 | 642502 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3719 | 1470 | 1705 | 468 | 1440 | 0.0892 | 0.0 | 639245 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2790 | 776 | 776 | 452 | 788 | 0.0000 | 0.0 | 632168 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2684 | 670 | 671 | 424 | 1160 | 0.0000 | 0.0 | 632645 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2882 | 869 | 869 | 452 | 1100 | 0.0000 | 0.0 | 636583 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2698 | 686 | 686 | 432 | 772 | 0.0000 | 0.0 | 632378 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2851 | 839 | 839 | 444 | 1084 | 0.0000 | 0.0 | 653762 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3580 | 841 | 841 | 732 | 1900 | 0.0051 | 0.0 | 643659 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2909 | 889 | 889 | 480 | 872 | 0.0000 | 0.0 | 645293 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 30 | 4 | high | 0 | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 2894 | 880 | 881 | 452 | 1108 | 0.0000 | 0.0 | 635720 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3713 | 979 | 979 | 736 | 1100 | 0.0000 | 0.0 | 1135596 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2815 | 787 | 788 | 440 | 792 | 0.0000 | 0.0 | 634332 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2842 | 823 | 830 | 468 | 832 | 0.0330 | 0.0 | 633322 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2884 | 867 | 867 | 472 | 816 | 0.0000 | 0.0 | 636046 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2750 | 733 | 736 | 444 | 772 | 0.0000 | 0.0 | 632820 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2981 | 957 | 970 | 660 | 1008 | 0.0000 | 0.0 | 633521 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2717 | 703 | 703 | 460 | 768 | 0.0032 | 0.0 | 526307 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2667 | 652 | 653 | 432 | 740 | 0.0032 | 0.0 | 526628 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 714 | 714 | 444 | 760 | 0.0032 | 0.0 | 526540 |
| logged-in-reader | mobile | reading | /vi/reading/session/e19db5d3-6831-4689-bbd1-f1a3611b621a | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2740 | 726 | 726 | 464 | 808 | 0.0000 | 0.0 | 632680 |
| logged-in-reader | mobile | reading | /vi/reading/session/ffe02224-b8b1-40a6-872c-d311af3cdb10 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2907 | 790 | 894 | 452 | 796 | 0.0000 | 0.0 | 695120 |
| logged-in-reader | mobile | reading | /vi/reading/session/4d2231b3-f259-4df5-9a0a-6d661e251b23 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 730 | 730 | 584 | 912 | 0.0000 | 0.0 | 632698 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2795 | 754 | 777 | 432 | 768 | 0.0000 | 0.0 | 633777 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2806 | 786 | 787 | 440 | 756 | 0.0000 | 0.0 | 633752 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2735 | 721 | 721 | 452 | 764 | 0.0000 | 0.0 | 631646 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2717 | 699 | 700 | 428 | 764 | 0.0000 | 0.0 | 633182 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2692 | 677 | 677 | 444 | 780 | 0.0000 | 0.0 | 633353 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2789 | 771 | 778 | 436 | 852 | 0.0000 | 0.0 | 633159 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 13 | 4 | 3 | 12 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 6 | 0 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 13 | 5 | 0 | 12 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 6 | 4 | 0 | 6 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 33 | high | 3 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 31 | high | 3 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 56 | critical | 3 | 49 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/26b570f0-4558-4324-a02e-e6dfb0eaddb5 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/afebe741-090f-48e7-bf9a-5bce862c48c6 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/4c16b9de-bc17-421c-8c51-1c8e070a0fed | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 32 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 56 | critical | 3 | 49 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/e4e4d714-6db9-49aa-a887-2f8c123d24d0 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/3f406a9c-c3e5-4b16-a767-cb3071b071a4 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/1dc68087-4b2a-4437-80f6-85788f11f876 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 33 | high | 3 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 33 | high | 3 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 56 | critical | 3 | 49 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 80 | critical | 3 | 71 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/eee9d192-5b43-469f-bd46-90f4c92190ed | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/4196f2b5-4f7c-4c80-9f3b-e2d33568d001 | 34 | high | 3 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/62c8bf21-3570-40b4-a4a1-0ae93d490b7b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 35 | high | 5 | 27 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 33 | high | 3 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 33 | high | 3 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 33 | high | 3 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 31 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 56 | critical | 3 | 49 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/e19db5d3-6831-4689-bbd1-f1a3611b621a | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/ffe02224-b8b1-40a6-872c-d311af3cdb10 | 34 | high | 3 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/4d2231b3-f259-4df5-9a0a-6d661e251b23 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 1381 | 327 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 1113 | 317 | html | https://www.tarotnow.xyz/vi/profile |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1099 | 589 | static | https://www.tarotnow.xyz/_next/static/chunks/08nqfkhj3~_69.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 1009 | 519 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-out | mobile | auth-public | /vi | GET | 200 | 941 | 551 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 923 | 339 | html | https://www.tarotnow.xyz/vi/readers |
| logged-out | desktop | auth-public | /vi | GET | 200 | 907 | 345 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | GET | 200 | 905 | 573 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 894 | 529 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 866 | 385 | html | https://www.tarotnow.xyz/vi/profile |
| logged-out | desktop | auth-public | /vi | GET | 200 | 856 | 558 | static | https://www.tarotnow.xyz/_next/static/chunks/08f_jn-s1oto5.js |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 856 | 552 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 851 | 361 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 836 | 335 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 834 | 340 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 833 | 358 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 833 | 349 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 826 | 334 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 823 | 339 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 822 | 337 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 814 | 331 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 812 | 320 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 811 | 320 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 809 | 344 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 807 | 352 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 807 | 324 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 806 | 311 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 805 | 327 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 804 | 313 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-out | mobile | auth-public | /vi/reset-password | GET | 200 | 802 | 551 | html | https://www.tarotnow.xyz/vi/reset-password |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 800 | 317 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 800 | 314 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 799 | 312 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 798 | 331 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 792 | 369 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 792 | 333 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 791 | 311 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 791 | 329 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 785 | 315 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 783 | 319 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 782 | 329 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 782 | 299 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 779 | 386 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 776 | 538 | static | https://www.tarotnow.xyz/_next/static/chunks/08pjgbp2d2w~p.js |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 773 | 340 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 773 | 311 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 771 | 304 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 770 | 528 | static | https://www.tarotnow.xyz/_next/static/chunks/0gzhvg6hs4zlc.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 770 | 298 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | reading | /vi/reading/session/4196f2b5-4f7c-4c80-9f3b-e2d33568d001 | GET | 200 | 769 | 360 | html | https://www.tarotnow.xyz/vi/reading/session/4196f2b5-4f7c-4c80-9f3b-e2d33568d001 |
| logged-in-admin | mobile | reading | /vi/reading/session/62c8bf21-3570-40b4-a4a1-0ae93d490b7b | GET | 200 | 769 | 412 | html | https://www.tarotnow.xyz/vi/reading/session/62c8bf21-3570-40b4-a4a1-0ae93d490b7b |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 769 | 307 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 764 | 312 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 764 | 541 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 757 | 539 | static | https://www.tarotnow.xyz/_next/static/chunks/08nqfkhj3~_69.js |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 756 | 541 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 755 | 323 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 754 | 298 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 749 | 539 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 740 | 352 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 738 | 321 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | desktop | admin | /vi/admin/readings | GET | 200 | 733 | 369 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 725 | 315 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 721 | 345 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | reading | /vi/reading/session/26b570f0-4558-4324-a02e-e6dfb0eaddb5 | GET | 200 | 716 | 332 | html | https://www.tarotnow.xyz/vi/reading/session/26b570f0-4558-4324-a02e-e6dfb0eaddb5 |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 716 | 542 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 715 | 330 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 714 | 311 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 712 | 540 | static | https://www.tarotnow.xyz/_next/static/chunks/08f_jn-s1oto5.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 710 | 332 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | GET | 200 | 709 | 328 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 709 | 321 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | GET | 200 | 707 | 401 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-out | mobile | auth-public | /vi/legal/privacy | GET | 200 | 706 | 328 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 705 | 326 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 705 | 333 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-out | desktop | auth-public | /vi/legal/privacy | GET | 200 | 704 | 344 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 702 | 311 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 698 | 326 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 698 | 329 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | desktop | admin | /vi/admin/deposits | GET | 200 | 696 | 329 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 694 | 324 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | reading | /vi/reading/session/ffe02224-b8b1-40a6-872c-d311af3cdb10 | GET | 200 | 694 | 330 | html | https://www.tarotnow.xyz/vi/reading/session/ffe02224-b8b1-40a6-872c-d311af3cdb10 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 692 | 317 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 691 | 313 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 689 | 330 | html | https://www.tarotnow.xyz/vi/login |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 688 | 343 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 688 | 324 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 688 | 324 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-out | desktop | auth-public | /vi/forgot-password | GET | 200 | 687 | 372 | html | https://www.tarotnow.xyz/vi/forgot-password |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 687 | 331 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 686 | 367 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 685 | 319 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 683 | 328 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-out | desktop | auth-public | /vi | GET | 200 | 682 | 569 | static | https://www.tarotnow.xyz/_next/static/chunks/04rddris79wjy.js |
| logged-in-reader | desktop | reader-chat | /vi/chat | GET | 200 | 682 | 338 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | mobile | auth-public | /vi | GET | 200 | 680 | 328 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 677 | 344 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 676 | 320 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 674 | 333 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 674 | 334 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 673 | 310 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 672 | 324 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-out | desktop | auth-public | /vi/register | GET | 200 | 671 | 333 | html | https://www.tarotnow.xyz/vi/register |
| logged-in-admin | mobile | admin | /vi/admin/promotions | GET | 200 | 671 | 331 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 670 | 333 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-reader | desktop | reading | /vi/reading/session/3f406a9c-c3e5-4b16-a767-cb3071b071a4 | GET | 200 | 669 | 313 | html | https://www.tarotnow.xyz/vi/reading/session/3f406a9c-c3e5-4b16-a767-cb3071b071a4 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 669 | 315 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 668 | 327 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | mobile | reading | /vi/reading/session/e19db5d3-6831-4689-bbd1-f1a3611b621a | GET | 200 | 668 | 312 | html | https://www.tarotnow.xyz/vi/reading/session/e19db5d3-6831-4689-bbd1-f1a3611b621a |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| - | - | - | - |

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
| logged-in-admin | desktop | reading.init.spread_3: created 26b570f0-4558-4324-a02e-e6dfb0eaddb5. |
| logged-in-admin | desktop | reading.init.spread_5: created afebe741-090f-48e7-bf9a-5bce862c48c6. |
| logged-in-admin | desktop | reading.init.spread_10: created 4c16b9de-bc17-421c-8c51-1c8e070a0fed. |
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
| logged-in-reader | desktop | reading.init.spread_3: created e4e4d714-6db9-49aa-a887-2f8c123d24d0. |
| logged-in-reader | desktop | reading.init.spread_5: created 3f406a9c-c3e5-4b16-a767-cb3071b071a4. |
| logged-in-reader | desktop | reading.init.spread_10: created 1dc68087-4b2a-4437-80f6-85788f11f876. |
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
| logged-in-admin | mobile | reading.init.spread_3: created eee9d192-5b43-469f-bd46-90f4c92190ed. |
| logged-in-admin | mobile | reading.init.spread_5: created 4196f2b5-4f7c-4c80-9f3b-e2d33568d001. |
| logged-in-admin | mobile | reading.init.spread_10: created 62c8bf21-3570-40b4-a4a1-0ae93d490b7b. |
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
| logged-in-reader | mobile | reading.init.spread_3: created e19db5d3-6831-4689-bbd1-f1a3611b621a. |
| logged-in-reader | mobile | reading.init.spread_5: created ffe02224-b8b1-40a6-872c-d311af3cdb10. |
| logged-in-reader | mobile | reading.init.spread_10: created 4d2231b3-f259-4df5-9a0a-6d661e251b23. |
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
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login bootstrap succeeded.

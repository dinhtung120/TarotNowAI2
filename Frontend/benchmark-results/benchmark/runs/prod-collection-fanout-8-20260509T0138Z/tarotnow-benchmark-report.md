# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T18:46:27.352Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 7
- High pages (request count): 142
- High slow requests: 27
- Medium slow requests: 218

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2845 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 3007 | 1279 | 1 | 0 | 13 | 1 | yes |
| logged-in-reader | desktop | 33 | 2950 | 969 | 9 | 0 | 15 | 0 | yes |
| logged-out | mobile | 9 | 2799 | 225 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 2898 | 1275 | 0 | 0 | 10 | 3 | yes |
| logged-in-reader | mobile | 33 | 2942 | 988 | 0 | 0 | 18 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2915 | 0 |
| logged-in-admin | desktop | chat | 1 | 30.0 | 2899 | 0 |
| logged-in-admin | desktop | collection | 1 | 31.0 | 6677 | 0 |
| logged-in-admin | desktop | community | 1 | 36.0 | 4095 | 0 |
| logged-in-admin | desktop | gacha | 2 | 30.5 | 2919 | 0 |
| logged-in-admin | desktop | gamification | 1 | 31.0 | 3261 | 1 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2731 | 0 |
| logged-in-admin | desktop | inventory | 1 | 30.0 | 2849 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 3250 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2742 | 0 |
| logged-in-admin | desktop | notifications | 1 | 30.0 | 2891 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2872 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2756 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.3 | 2806 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.8 | 2850 | 0 |
| logged-in-admin | desktop | wallet | 4 | 35.5 | 3042 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.3 | 2807 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2802 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5455 | 0 |
| logged-in-admin | mobile | community | 1 | 36.0 | 3629 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.0 | 2872 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2935 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2756 | 0 |
| logged-in-admin | mobile | inventory | 1 | 30.0 | 2780 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 2872 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2691 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2716 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 2842 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2740 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2774 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.8 | 2802 | 0 |
| logged-in-admin | mobile | wallet | 4 | 36.8 | 3025 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2748 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6132 | 0 |
| logged-in-reader | desktop | community | 1 | 37.0 | 3605 | 0 |
| logged-in-reader | desktop | gacha | 2 | 31.5 | 2896 | 0 |
| logged-in-reader | desktop | gamification | 1 | 33.0 | 3463 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2754 | 0 |
| logged-in-reader | desktop | inventory | 1 | 30.0 | 2839 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2738 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2684 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2840 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.0 | 2826 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2711 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.3 | 2801 | 0 |
| logged-in-reader | desktop | reading | 5 | 33.0 | 2906 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.0 | 2765 | 9 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2783 | 0 |
| logged-in-reader | mobile | collection | 1 | 31.0 | 5573 | 0 |
| logged-in-reader | mobile | community | 1 | 30.0 | 3533 | 0 |
| logged-in-reader | mobile | gacha | 2 | 32.5 | 2867 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2842 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2745 | 0 |
| logged-in-reader | mobile | inventory | 1 | 32.0 | 2855 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2818 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2729 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2752 | 0 |
| logged-in-reader | mobile | profile | 3 | 30.7 | 2872 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2721 | 0 |
| logged-in-reader | mobile | readers | 7 | 29.0 | 2808 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.8 | 2822 | 0 |
| logged-in-reader | mobile | wallet | 4 | 35.5 | 3043 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2731 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3903 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2683 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2747 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3190 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2754 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3903 | 1528 | 1893 | 1324 | 1324 | 0.0000 | 243.0 | 601522 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2710 | 703 | 703 | 504 | 504 | 0.0000 | 0.0 | 512378 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2726 | 710 | 710 | 580 | 580 | 0.0000 | 0.0 | 512944 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2661 | 654 | 654 | 480 | 480 | 0.0000 | 0.0 | 511998 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2751 | 732 | 745 | 572 | 572 | 0.0000 | 0.0 | 512080 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2808 | 803 | 803 | 704 | 704 | 0.0000 | 0.0 | 512208 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2683 | 673 | 673 | 456 | 788 | 0.0000 | 0.0 | 525851 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2669 | 659 | 659 | 552 | 876 | 0.0000 | 0.0 | 526044 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2698 | 688 | 688 | 600 | 600 | 0.0000 | 0.0 | 526119 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2731 | 724 | 724 | 580 | 1112 | 0.0035 | 141.0 | 537716 |
| logged-in-admin | desktop | reading | /vi/reading | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2906 | 855 | 895 | 568 | 928 | 0.0041 | 0.0 | 644145 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 30 | 11 | high | 0 | 0 | 0 | 0 | 5 | 0 | 5 | 5 | 0 | 0 | 2849 | 842 | 842 | 712 | 1108 | 0.0041 | 0.0 | 644622 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 30 | 12 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2953 | 894 | 945 | 744 | 1164 | 0.0041 | 0.0 | 725226 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2885 | 878 | 878 | 644 | 1228 | 0.0041 | 0.0 | 727235 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 31 | 26 | high | 0 | 0 | 1 | 0 | 13 | 3 | 5 | 12 | 1 | 0 | 6677 | 784 | 1294 | 492 | 884 | 0.0042 | 0.0 | 645953 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3027 | 882 | 1021 | 504 | 1164 | 0.0489 | 0.0 | 636487 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 735 | 741 | 528 | 904 | 0.0041 | 0.0 | 631774 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 41 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2844 | 847 | 1089 | 660 | 1032 | 0.0489 | 0.0 | 630853 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2961 | 927 | 953 | 612 | 1028 | 0.0041 | 0.0 | 634025 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2899 | 850 | 869 | 552 | 972 | 0.0041 | 0.0 | 634209 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3250 | 757 | 1245 | 804 | 804 | 0.0179 | 0.0 | 652199 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 36 | 4 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 4095 | 855 | 1344 | 564 | 1884 | 0.0041 | 0.0 | 779200 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | 9 | high | 0 | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 3261 | 883 | 1252 | 540 | 932 | 0.0176 | 0.0 | 643874 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3805 | 1097 | 1098 | 888 | 1424 | 0.0000 | 9.0 | 1135412 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 845 | 850 | 488 | 940 | 0.0041 | 0.0 | 633889 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2691 | 682 | 684 | 500 | 1024 | 0.0041 | 0.0 | 632593 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2806 | 918 | 918 | 512 | 1192 | 0.0041 | 0.0 | 631181 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2891 | 834 | 881 | 552 | 912 | 0.0041 | 0.0 | 634348 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2756 | 743 | 750 | 644 | 1012 | 0.0041 | 0.0 | 632439 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2905 | 884 | 892 | 560 | 1180 | 0.0041 | 0.0 | 633061 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2794 | 786 | 786 | 612 | 904 | 0.0020 | 0.0 | 526124 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 740 | 741 | 548 | 836 | 0.0020 | 0.0 | 526282 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2687 | 678 | 678 | 512 | 832 | 0.0020 | 0.0 | 526429 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3316 | 831 | 1306 | 652 | 1016 | 0.0000 | 0.0 | 647795 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2798 | 764 | 790 | 496 | 944 | 0.0000 | 0.0 | 647715 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2794 | 780 | 781 | 528 | 856 | 0.0000 | 0.0 | 646080 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3327 | 795 | 1320 | 540 | 864 | 0.0022 | 0.0 | 698754 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2795 | 781 | 787 | 520 | 864 | 0.0000 | 0.0 | 644776 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2961 | 922 | 953 | 608 | 924 | 0.0000 | 0.0 | 648197 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2783 | 772 | 773 | 552 | 1016 | 0.0000 | 0.0 | 648860 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2794 | 782 | 786 | 708 | 1128 | 0.0000 | 0.0 | 655878 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2755 | 744 | 747 | 556 | 960 | 0.0000 | 0.0 | 650150 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2827 | 814 | 816 | 620 | 968 | 0.0000 | 0.0 | 646256 |
| logged-in-admin | desktop | reading | /vi/reading/session/4ee0c9fc-07f6-45a1-bee8-9d0914361cc5 | 35 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2974 | 834 | 965 | 548 | 920 | 0.0041 | 0.0 | 726868 |
| logged-in-admin | desktop | reading | /vi/reading/session/c12ee55f-d8e8-4ca0-a3f3-59810449ef7a | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2729 | 713 | 719 | 528 | 908 | 0.0041 | 0.0 | 632199 |
| logged-in-admin | desktop | reading | /vi/reading/session/426acb21-bb75-4833-9355-a487af7db0f4 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2738 | 726 | 727 | 520 | 920 | 0.0041 | 0.0 | 632442 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2870 | 834 | 860 | 532 | 908 | 0.0041 | 0.0 | 633393 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2783 | 773 | 777 | 512 | 900 | 0.0041 | 0.0 | 631272 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2717 | 705 | 707 | 492 | 900 | 0.0041 | 0.0 | 631320 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2710 | 697 | 701 | 532 | 1052 | 0.0041 | 0.0 | 633079 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2726 | 716 | 717 | 480 | 1032 | 0.0041 | 0.0 | 632911 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2877 | 857 | 865 | 552 | 1172 | 0.0041 | 0.0 | 633303 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 745 | 745 | 536 | 1080 | 0.0033 | 181.0 | 537838 |
| logged-in-reader | desktop | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2873 | 826 | 866 | 504 | 920 | 0.0039 | 0.0 | 645275 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 30 | 10 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2839 | 827 | 828 | 500 | 1156 | 0.0039 | 0.0 | 644835 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2903 | 884 | 885 | 528 | 1204 | 0.0039 | 0.0 | 728572 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2888 | 879 | 879 | 532 | 1340 | 0.0039 | 0.0 | 724632 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 26 | high | 0 | 0 | 0 | 0 | 6 | 3 | 0 | 6 | 0 | 0 | 6132 | 679 | 693 | 516 | 516 | 0.0040 | 0.0 | 642498 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3082 | 868 | 1073 | 520 | 1188 | 0.0726 | 0.0 | 636839 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2707 | 685 | 697 | 592 | 976 | 0.0039 | 0.0 | 632153 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2688 | 665 | 680 | 484 | 1284 | 0.0039 | 0.0 | 632659 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2733 | 714 | 725 | 584 | 960 | 0.0039 | 0.0 | 634138 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2748 | 727 | 737 | 564 | 956 | 0.0039 | 0.0 | 632111 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2738 | 705 | 727 | 488 | 868 | 0.0177 | 0.0 | 650135 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 37 | 3 | critical | 0 | 0 | 2 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3605 | 743 | 872 | 484 | 1652 | 0.0039 | 0.0 | 780025 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 33 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3463 | 1453 | 1453 | 960 | 1360 | 0.0430 | 0.0 | 645256 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 27 | 36 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2682 | 1017 | 1017 | 924 | 1492 | 0.0000 | 37.0 | 607240 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 27 | 67 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 677 | 677 | 500 | 500 | 0.0028 | 101.0 | 607337 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2735 | 726 | 726 | 500 | 904 | 0.0039 | 0.0 | 632115 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2890 | 867 | 868 | 532 | 1172 | 0.0095 | 0.0 | 635527 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 827 | 827 | 536 | 920 | 0.0040 | 0.0 | 632417 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2711 | 702 | 703 | 504 | 848 | 0.0039 | 0.0 | 632681 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2823 | 780 | 811 | 520 | 1108 | 0.0039 | 0.0 | 633371 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2657 | 647 | 647 | 492 | 796 | 0.0019 | 0.0 | 526413 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2688 | 679 | 679 | 480 | 808 | 0.0019 | 0.0 | 526422 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2708 | 698 | 698 | 504 | 800 | 0.0019 | 0.0 | 526480 |
| logged-in-reader | desktop | reading | /vi/reading/session/e895cbe6-e5f2-435c-acb0-13ea94ffc8a8 | 36 | 3 | critical | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2919 | 756 | 909 | 504 | 892 | 0.0039 | 0.0 | 728024 |
| logged-in-reader | desktop | reading | /vi/reading/session/759e0b16-0391-43fc-8baa-1e19d276ca4c | 35 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2933 | 754 | 924 | 500 | 872 | 0.0039 | 0.0 | 727410 |
| logged-in-reader | desktop | reading | /vi/reading/session/8e33152e-7625-47c1-9b00-6a3e6bbe84bb | 35 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2984 | 813 | 974 | 560 | 936 | 0.0039 | 0.0 | 727265 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2838 | 813 | 814 | 496 | 892 | 0.0039 | 0.0 | 633674 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2868 | 846 | 855 | 584 | 944 | 0.0039 | 0.0 | 631503 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2678 | 666 | 666 | 476 | 852 | 0.0039 | 0.0 | 631462 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2859 | 836 | 845 | 540 | 1096 | 0.0039 | 0.0 | 633233 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2790 | 776 | 777 | 648 | 1212 | 0.0039 | 0.0 | 633359 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 821 | 829 | 496 | 1116 | 0.0039 | 0.0 | 633482 |
| logged-out | mobile | auth-public | /vi | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3190 | 1180 | 1180 | 900 | 956 | 0.0000 | 0.0 | 602832 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2929 | 920 | 920 | 852 | 852 | 0.0000 | 0.0 | 512494 |
| logged-out | mobile | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2671 | 662 | 662 | 476 | 476 | 0.0000 | 0.0 | 513065 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2733 | 721 | 721 | 504 | 504 | 0.0000 | 0.0 | 512062 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2714 | 701 | 702 | 488 | 488 | 0.0000 | 0.0 | 512220 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2689 | 677 | 677 | 472 | 472 | 0.0000 | 0.0 | 512167 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2784 | 760 | 772 | 688 | 688 | 0.0000 | 0.0 | 525991 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2733 | 716 | 716 | 504 | 832 | 0.0000 | 0.0 | 526043 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2744 | 730 | 730 | 476 | 784 | 0.0000 | 0.0 | 526130 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2756 | 746 | 746 | 520 | 868 | 0.0032 | 0.0 | 537966 |
| logged-in-admin | mobile | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2889 | 867 | 874 | 452 | 852 | 0.0000 | 0.0 | 645068 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 30 | 11 | high | 0 | 0 | 0 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 2780 | 750 | 771 | 608 | 940 | 0.0000 | 0.0 | 644829 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 1 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2851 | 836 | 836 | 448 | 1092 | 0.0000 | 0.0 | 727258 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2892 | 880 | 880 | 488 | 952 | 0.0000 | 0.0 | 726231 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 28 | high | 0 | 0 | 0 | 0 | 13 | 4 | 1 | 12 | 1 | 0 | 5455 | 718 | 735 | 584 | 584 | 0.0000 | 0.0 | 643376 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3023 | 877 | 1015 | 488 | 1132 | 0.0689 | 0.0 | 636825 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2778 | 757 | 769 | 472 | 816 | 0.0000 | 0.0 | 631810 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2724 | 856 | 1088 | 560 | 916 | 0.0760 | 0.0 | 630915 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2856 | 843 | 844 | 448 | 1084 | 0.0000 | 0.0 | 636318 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2802 | 755 | 771 | 484 | 828 | 0.0000 | 0.0 | 631848 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2872 | 791 | 852 | 516 | 852 | 0.0196 | 0.0 | 650009 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 36 | 3 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3629 | 740 | 872 | 456 | 1824 | 0.0051 | 0.0 | 778817 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2935 | 920 | 920 | 772 | 1112 | 0.0000 | 0.0 | 642059 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 35 | 7 | high | 0 | 0 | 2 | 3 | 0 | 0 | 0 | 0 | 0 | 0 | 2900 | 887 | 887 | 468 | 1112 | 0.0000 | 0.0 | 638535 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3649 | 955 | 955 | 772 | 1140 | 0.0000 | 0.0 | 1135680 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2797 | 737 | 785 | 452 | 784 | 0.0000 | 0.0 | 632502 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2753 | 834 | 834 | 588 | 992 | 0.0071 | 0.0 | 631222 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2716 | 686 | 703 | 480 | 820 | 0.0000 | 0.0 | 632267 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2740 | 710 | 727 | 584 | 924 | 0.0000 | 0.0 | 632555 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2670 | 644 | 659 | 444 | 848 | 0.0000 | 0.0 | 632854 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2646 | 633 | 634 | 460 | 768 | 0.0032 | 0.0 | 526495 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2657 | 643 | 643 | 436 | 772 | 0.0032 | 0.0 | 526242 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 755 | 755 | 488 | 804 | 0.0032 | 0.0 | 526474 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2787 | 763 | 775 | 444 | 764 | 0.0000 | 0.0 | 647531 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2820 | 794 | 807 | 632 | 988 | 0.0000 | 0.0 | 647858 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2768 | 737 | 754 | 464 | 784 | 0.0000 | 0.0 | 646216 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2863 | 765 | 850 | 456 | 780 | 0.0000 | 0.0 | 698426 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2710 | 683 | 700 | 452 | 760 | 0.0000 | 0.0 | 644682 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2749 | 717 | 737 | 452 | 764 | 0.0000 | 0.0 | 646579 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2849 | 829 | 837 | 584 | 920 | 0.0000 | 0.0 | 648950 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 28 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2752 | 724 | 740 | 484 | 812 | 0.0000 | 0.0 | 655980 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2885 | 871 | 871 | 648 | 992 | 0.0000 | 0.0 | 650094 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2882 | 775 | 869 | 464 | 772 | 0.0000 | 0.0 | 647865 |
| logged-in-admin | mobile | reading | /vi/reading/session/15f4ee0a-3632-4840-abbc-ba05c023069e | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2751 | 720 | 738 | 548 | 896 | 0.0000 | 0.0 | 632401 |
| logged-in-admin | mobile | reading | /vi/reading/session/d23d44fa-bdf5-4479-8027-62418f95150a | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2945 | 826 | 934 | 500 | 824 | 0.0000 | 0.0 | 695131 |
| logged-in-admin | mobile | reading | /vi/reading/session/3fa31ac5-f45d-4d11-976b-e10334651f82 | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 730 | 748 | 464 | 784 | 0.0000 | 0.0 | 632359 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2811 | 781 | 799 | 564 | 900 | 0.0000 | 0.0 | 631244 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2733 | 705 | 721 | 448 | 784 | 0.0000 | 0.0 | 631353 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2808 | 758 | 799 | 472 | 792 | 0.0000 | 0.0 | 631156 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 712 | 731 | 456 | 852 | 0.0000 | 0.0 | 633016 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2743 | 717 | 731 | 448 | 780 | 0.0000 | 0.0 | 633169 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2720 | 689 | 708 | 460 | 820 | 0.0000 | 0.0 | 633305 |
| logged-in-reader | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2745 | 715 | 735 | 500 | 856 | 0.0032 | 0.0 | 537827 |
| logged-in-reader | mobile | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2884 | 839 | 864 | 460 | 844 | 0.0000 | 0.0 | 645532 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2855 | 840 | 840 | 460 | 1088 | 0.0000 | 0.0 | 646909 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 33 | 9 | high | 0 | 0 | 2 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2885 | 867 | 867 | 452 | 1096 | 0.0000 | 0.0 | 728804 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2849 | 836 | 836 | 468 | 1116 | 0.0000 | 0.0 | 726747 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 31 | 23 | high | 0 | 0 | 1 | 0 | 6 | 1 | 2 | 6 | 0 | 0 | 5573 | 779 | 826 | 464 | 792 | 0.0000 | 0.0 | 644605 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2989 | 843 | 981 | 464 | 1080 | 0.0821 | 0.0 | 637580 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2809 | 725 | 792 | 448 | 772 | 0.0000 | 0.0 | 634118 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 774 | 798 | 448 | 1164 | 0.0000 | 0.0 | 634970 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 827 | 827 | 452 | 1064 | 0.0000 | 0.0 | 637433 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2783 | 724 | 772 | 444 | 780 | 0.0000 | 0.0 | 632106 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2818 | 764 | 806 | 456 | 772 | 0.0196 | 0.0 | 650203 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | 9 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3533 | 709 | 725 | 440 | 1544 | 0.0051 | 0.0 | 643443 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2842 | 832 | 832 | 452 | 836 | 0.0000 | 0.0 | 642285 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2832 | 819 | 819 | 564 | 896 | 0.0000 | 0.0 | 634205 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3669 | 932 | 932 | 744 | 1104 | 0.0000 | 0.0 | 1135778 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2793 | 727 | 782 | 452 | 776 | 0.0000 | 0.0 | 632042 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2877 | 866 | 866 | 448 | 1072 | 0.0330 | 0.0 | 635369 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2752 | 724 | 738 | 564 | 912 | 0.0000 | 0.0 | 632300 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2721 | 692 | 708 | 428 | 748 | 0.0000 | 0.0 | 632540 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2798 | 764 | 786 | 488 | 828 | 0.0000 | 0.0 | 633163 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2686 | 675 | 675 | 476 | 796 | 0.0032 | 0.0 | 526341 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2815 | 792 | 800 | 692 | 692 | 0.0000 | 0.0 | 526413 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2687 | 670 | 670 | 488 | 800 | 0.0032 | 0.0 | 526542 |
| logged-in-reader | mobile | reading | /vi/reading/session/48cff794-03d4-4c3c-920f-9bc41f4c803e | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2719 | 683 | 705 | 448 | 776 | 0.0000 | 0.0 | 632626 |
| logged-in-reader | mobile | reading | /vi/reading/session/418c816e-9061-4fcd-90fa-9cd858dabd1a | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2720 | 685 | 705 | 440 | 760 | 0.0000 | 0.0 | 632615 |
| logged-in-reader | mobile | reading | /vi/reading/session/0379ecbb-d0ca-492d-8849-0e9b4bb5c417 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2990 | 869 | 980 | 544 | 872 | 0.0000 | 0.0 | 695057 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2834 | 816 | 824 | 496 | 824 | 0.0000 | 0.0 | 631509 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2676 | 645 | 666 | 444 | 760 | 0.0000 | 0.0 | 631491 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2922 | 886 | 908 | 460 | 816 | 0.0000 | 0.0 | 633455 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2725 | 700 | 712 | 444 | 788 | 0.0000 | 0.0 | 632967 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2812 | 743 | 802 | 472 | 876 | 0.0000 | 0.0 | 633121 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2847 | 803 | 825 | 452 | 812 | 0.0000 | 0.0 | 635401 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 13 | 3 | 5 | 12 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 6 | 3 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 13 | 4 | 1 | 12 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 6 | 1 | 2 | 6 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 56 | critical | 3 | 49 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/4ee0c9fc-07f6-45a1-bee8-9d0914361cc5 | 35 | high | 3 | 30 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/c12ee55f-d8e8-4ca0-a3f3-59810449ef7a | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/426acb21-bb75-4833-9355-a487af7db0f4 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 37 | critical | 3 | 32 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 33 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 27 | high | 0 | 25 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 27 | high | 0 | 25 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/e895cbe6-e5f2-435c-acb0-13ea94ffc8a8 | 36 | critical | 4 | 30 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/759e0b16-0391-43fc-8baa-1e19d276ca4c | 35 | high | 3 | 30 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/8e33152e-7625-47c1-9b00-6a3e6bbe84bb | 35 | high | 3 | 30 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 35 | high | 6 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 56 | critical | 3 | 49 | 0 |
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
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/15f4ee0a-3632-4840-abbc-ba05c023069e | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/d23d44fa-bdf5-4479-8027-62418f95150a | 34 | high | 3 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/3fa31ac5-f45d-4d11-976b-e10334651f82 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 33 | high | 3 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 31 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | high | 3 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 56 | critical | 3 | 49 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/48cff794-03d4-4c3c-920f-9bc41f4c803e | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/418c816e-9061-4fcd-90fa-9cd858dabd1a | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/0379ecbb-d0ca-492d-8849-0e9b4bb5c417 | 34 | high | 3 | 29 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 30 | high | 2 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1124 | 597 | html | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1032 | 547 | static | https://www.tarotnow.xyz/_next/static/chunks/102-.gonu2j.f.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 1024 | 519 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 987 | 509 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 983 | 334 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-out | mobile | auth-public | /vi | GET | 200 | 960 | 614 | html | https://www.tarotnow.xyz/vi |
| logged-out | mobile | auth-public | /vi/login | GET | 200 | 838 | 554 | html | https://www.tarotnow.xyz/vi/login |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 837 | 440 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 837 | 369 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 832 | 351 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 830 | 313 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 828 | 422 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 827 | 342 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 827 | 349 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 823 | 354 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | reading | /vi/reading/session/0379ecbb-d0ca-492d-8849-0e9b4bb5c417 | GET | 200 | 821 | 313 | html | https://www.tarotnow.xyz/vi/reading/session/0379ecbb-d0ca-492d-8849-0e9b4bb5c417 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 818 | 411 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 814 | 326 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 814 | 335 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 812 | 330 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 806 | 320 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 805 | 562 | static | https://www.tarotnow.xyz/_next/static/chunks/10sftrkhll1m7.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 805 | 315 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 805 | 314 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 804 | 307 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 804 | 347 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-out | desktop | auth-public | /vi | GET | 200 | 801 | 543 | static | https://www.tarotnow.xyz/_next/static/chunks/020ipuq9bw_tk.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 798 | 307 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 797 | 312 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 796 | 324 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 796 | 326 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 794 | 553 | static | https://www.tarotnow.xyz/_next/static/chunks/0ijiw4-281e4s.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 794 | 312 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 794 | 316 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 793 | 326 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 791 | 328 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 786 | 300 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 785 | 377 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 785 | 365 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 783 | 312 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 782 | 320 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | reading | /vi/reading/history | GET | 200 | 782 | 389 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 780 | 315 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 779 | 315 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 777 | 316 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 776 | 317 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 774 | 325 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 772 | 310 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 766 | 364 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 765 | 465 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 764 | 312 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 763 | 328 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 761 | 326 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 756 | 339 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | mobile | reading | /vi/reading/session/d23d44fa-bdf5-4479-8027-62418f95150a | GET | 200 | 756 | 358 | html | https://www.tarotnow.xyz/vi/reading/session/d23d44fa-bdf5-4479-8027-62418f95150a |
| logged-in-admin | desktop | reading | /vi/reading/session/4ee0c9fc-07f6-45a1-bee8-9d0914361cc5 | GET | 200 | 751 | 339 | html | https://www.tarotnow.xyz/vi/reading/session/4ee0c9fc-07f6-45a1-bee8-9d0914361cc5 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 750 | 318 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 748 | 330 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-out | desktop | auth-public | /vi/verify-email | GET | 200 | 747 | 359 | html | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | GET | 200 | 744 | 422 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 741 | 343 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-reader | desktop | reading | /vi/reading/session/8e33152e-7625-47c1-9b00-6a3e6bbe84bb | GET | 200 | 740 | 367 | html | https://www.tarotnow.xyz/vi/reading/session/8e33152e-7625-47c1-9b00-6a3e6bbe84bb |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 733 | 326 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 733 | 353 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 733 | 322 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | desktop | admin | /vi/admin/gamification | GET | 200 | 730 | 340 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 728 | 350 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 727 | 330 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 726 | 319 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 724 | 411 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 723 | 355 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 723 | 310 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | GET | 200 | 717 | 395 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | GET | 200 | 717 | 331 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 716 | 323 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 713 | 399 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 712 | 334 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 709 | 330 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 709 | 334 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 708 | 337 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 708 | 323 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 704 | 314 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | desktop | admin | /vi/admin/readings | GET | 200 | 703 | 356 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 702 | 333 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 701 | 317 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 700 | 323 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 700 | 334 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | desktop | admin | /vi/admin/deposits | GET | 200 | 699 | 326 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 699 | 315 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 699 | 319 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 697 | 364 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 696 | 325 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 695 | 323 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 693 | 315 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | desktop | auth-public | /vi | GET | 200 | 692 | 354 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 690 | 315 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 689 | 341 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 687 | 312 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 686 | 333 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 683 | 327 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | reading | /vi/reading/session/3fa31ac5-f45d-4d11-976b-e10334651f82 | GET | 200 | 683 | 327 | html | https://www.tarotnow.xyz/vi/reading/session/3fa31ac5-f45d-4d11-976b-e10334651f82 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | GET | 200 | 681 | 346 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 681 | 329 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 680 | 349 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | desktop | reading | /vi/reading/session/e895cbe6-e5f2-435c-acb0-13ea94ffc8a8 | GET | 200 | 680 | 320 | html | https://www.tarotnow.xyz/vi/reading/session/e895cbe6-e5f2-435c-acb0-13ea94ffc8a8 |
| logged-in-reader | desktop | reader-chat | /vi/chat | GET | 200 | 678 | 368 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | admin | /vi/admin/users | GET | 200 | 677 | 393 | html | https://www.tarotnow.xyz/vi/admin/users |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-admin | desktop | /vi/gamification | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/api/wallet/balance |
| logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/api/chat/unread-count |
| logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/api/notifications/unread-count |
| logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-reader | desktop | /vi/wallet/deposit | https://www.tarotnow.xyz/vi |

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
| logged-in-admin | desktop | reading.init.spread_3: created 4ee0c9fc-07f6-45a1-bee8-9d0914361cc5. |
| logged-in-admin | desktop | reading.init.spread_5: created c12ee55f-d8e8-4ca0-a3f3-59810449ef7a. |
| logged-in-admin | desktop | reading.init.spread_10: created 426acb21-bb75-4833-9355-a487af7db0f4. |
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
| logged-in-reader | desktop | reading.init.spread_3: created e895cbe6-e5f2-435c-acb0-13ea94ffc8a8. |
| logged-in-reader | desktop | reading.init.spread_5: created 759e0b16-0391-43fc-8baa-1e19d276ca4c. |
| logged-in-reader | desktop | reading.init.spread_10: created 8e33152e-7625-47c1-9b00-6a3e6bbe84bb. |
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
| logged-in-admin | mobile | reading.init.spread_3: created 15f4ee0a-3632-4840-abbc-ba05c023069e. |
| logged-in-admin | mobile | reading.init.spread_5: created d23d44fa-bdf5-4479-8027-62418f95150a. |
| logged-in-admin | mobile | reading.init.spread_10: created 3fa31ac5-f45d-4d11-976b-e10334651f82. |
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
| logged-in-reader | mobile | reading.init.spread_3: created 48cff794-03d4-4c3c-920f-9bc41f4c803e. |
| logged-in-reader | mobile | reading.init.spread_5: created 418c816e-9061-4fcd-90fa-9cd858dabd1a. |
| logged-in-reader | mobile | reading.init.spread_10: created 0379ecbb-d0ca-492d-8849-0e9b4bb5c417. |
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

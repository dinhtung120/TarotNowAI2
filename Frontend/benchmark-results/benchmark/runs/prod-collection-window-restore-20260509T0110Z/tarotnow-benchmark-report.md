# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-09T01:19:17.648Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 0
- High pages (request count): 142
- High slow requests: 35
- Medium slow requests: 262

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2893 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 3027 | 1242 | 0 | 0 | 13 | 0 | yes |
| logged-in-reader | desktop | 33 | 3003 | 948 | 0 | 0 | 8 | 0 | yes |
| logged-out | mobile | 9 | 2782 | 221 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 2949 | 1252 | 0 | 0 | 14 | 0 | yes |
| logged-in-reader | mobile | 33 | 3023 | 960 | 0 | 0 | 10 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2900 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2866 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 7327 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4086 | 0 |
| logged-in-admin | desktop | gacha | 2 | 30.5 | 2930 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 3280 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2776 | 0 |
| logged-in-admin | desktop | inventory | 1 | 32.0 | 2971 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3384 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2724 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2971 | 0 |
| logged-in-admin | desktop | profile | 3 | 28.7 | 2919 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 3059 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.0 | 2822 | 0 |
| logged-in-admin | desktop | reading | 5 | 30.2 | 2863 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.3 | 2894 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.6 | 2903 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2864 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5724 | 0 |
| logged-in-admin | mobile | community | 1 | 35.0 | 3667 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.0 | 2921 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2859 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2777 | 0 |
| logged-in-admin | mobile | inventory | 1 | 31.0 | 2911 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 2822 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2748 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2741 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.7 | 2871 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2857 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.4 | 2856 | 0 |
| logged-in-admin | mobile | reading | 5 | 31.2 | 2922 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.5 | 2812 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2867 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6761 | 0 |
| logged-in-reader | desktop | community | 1 | 35.0 | 3694 | 0 |
| logged-in-reader | desktop | gacha | 2 | 30.0 | 2946 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2882 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2759 | 0 |
| logged-in-reader | desktop | inventory | 1 | 35.0 | 2905 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2871 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2837 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2888 | 0 |
| logged-in-reader | desktop | profile | 3 | 28.7 | 2880 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2781 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.1 | 2858 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.4 | 2860 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.5 | 2833 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2835 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5913 | 0 |
| logged-in-reader | mobile | community | 1 | 35.0 | 3831 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.0 | 2957 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2877 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2759 | 0 |
| logged-in-reader | mobile | inventory | 1 | 35.0 | 2942 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2868 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2761 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2825 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.7 | 3120 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2881 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.1 | 2899 | 0 |
| logged-in-reader | mobile | reading | 5 | 30.2 | 2921 | 0 |
| logged-in-reader | mobile | wallet | 4 | 28.5 | 2894 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2762 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4002 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2742 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2725 | 0 |
| logged-out | mobile | home | 1 | 26.0 | 3214 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2732 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4002 | 1889 | 1907 | 1412 | 1412 | 0.0000 | 761.0 | 600837 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2760 | 731 | 750 | 564 | 564 | 0.0000 | 0.0 | 511598 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2801 | 789 | 789 | 680 | 680 | 0.0000 | 0.0 | 512339 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2681 | 673 | 673 | 468 | 468 | 0.0000 | 0.0 | 511237 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2860 | 849 | 849 | 548 | 548 | 0.0000 | 0.0 | 511382 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2710 | 699 | 699 | 516 | 516 | 0.0000 | 0.0 | 511535 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2776 | 766 | 766 | 500 | 500 | 0.0000 | 0.0 | 525154 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2716 | 706 | 706 | 500 | 500 | 0.0000 | 0.0 | 525348 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2735 | 723 | 723 | 480 | 480 | 0.0000 | 0.0 | 525442 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2776 | 736 | 767 | 648 | 1448 | 0.0035 | 472.0 | 537217 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2964 | 923 | 956 | 548 | 1040 | 0.0041 | 0.0 | 642502 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 2 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2971 | 957 | 957 | 528 | 1324 | 0.0041 | 1.0 | 645875 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 31 | 10 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2952 | 928 | 941 | 584 | 1376 | 0.0041 | 0.0 | 725500 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2908 | 898 | 898 | 528 | 1436 | 0.0041 | 0.0 | 725747 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 27 | high | 0 | 0 | 0 | 0 | 13 | 3 | 4 | 12 | 1 | 0 | 7327 | 784 | 788 | 612 | 612 | 0.0042 | 14.0 | 642917 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3035 | 917 | 1026 | 532 | 1200 | 0.0489 | 0.0 | 634411 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2908 | 860 | 894 | 684 | 1180 | 0.0041 | 0.0 | 631121 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2813 | 850 | 994 | 628 | 1052 | 0.0489 | 0.0 | 630586 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2892 | 878 | 879 | 564 | 1220 | 0.0041 | 0.0 | 633655 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2866 | 820 | 848 | 556 | 1008 | 0.0041 | 0.0 | 631126 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3384 | 834 | 1372 | 652 | 1100 | 0.0041 | 0.0 | 650996 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | 5 | high | 0 | 0 | 1 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 4086 | 824 | 1292 | 596 | 1176 | 0.0041 | 0.0 | 794699 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3280 | 856 | 1268 | 532 | 1012 | 0.0279 | 0.0 | 643105 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2945 | 933 | 933 | 592 | 1388 | 0.0041 | 0.0 | 634363 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2792 | 781 | 785 | 540 | 980 | 0.0041 | 0.0 | 631027 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2886 | 858 | 869 | 544 | 1200 | 0.0041 | 0.0 | 631902 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2953 | 958 | 970 | 596 | 1352 | 0.0041 | 0.0 | 630735 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2971 | 677 | 951 | 624 | 1060 | 0.0041 | 0.0 | 631698 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3059 | 1027 | 1040 | 580 | 1164 | 0.0041 | 0.0 | 632131 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2747 | 728 | 734 | 596 | 1300 | 0.0041 | 0.0 | 632505 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2682 | 670 | 671 | 568 | 932 | 0.0020 | 0.0 | 525616 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2714 | 705 | 705 | 564 | 892 | 0.0020 | 0.0 | 525683 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2775 | 765 | 765 | 548 | 880 | 0.0020 | 0.0 | 525744 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3193 | 730 | 1182 | 568 | 928 | 0.0000 | 0.0 | 647168 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 731 | 736 | 548 | 1088 | 0.0000 | 0.0 | 647097 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2838 | 756 | 825 | 760 | 1116 | 0.0000 | 0.0 | 645430 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3299 | 785 | 1287 | 532 | 908 | 0.0022 | 0.0 | 698078 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2794 | 780 | 784 | 580 | 896 | 0.0000 | 0.0 | 644104 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 819 | 848 | 592 | 932 | 0.0000 | 0.0 | 646005 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2912 | 796 | 900 | 564 | 1076 | 0.0000 | 0.0 | 649833 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2753 | 735 | 740 | 540 | 1052 | 0.0000 | 0.0 | 655284 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2797 | 785 | 787 | 528 | 1224 | 0.0000 | 0.0 | 649469 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2804 | 788 | 792 | 564 | 840 | 0.0000 | 0.0 | 645550 |
| logged-in-admin | desktop | reading | /vi/reading/session/9efb4036-c003-4708-8e05-0835cb7574ee | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2749 | 734 | 736 | 560 | 972 | 0.0041 | 0.0 | 631499 |
| logged-in-admin | desktop | reading | /vi/reading/session/513cf554-2eb5-4458-8c6f-acaf2685fca8 | 33 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2933 | 848 | 916 | 528 | 940 | 0.0041 | 0.0 | 713994 |
| logged-in-admin | desktop | reading | /vi/reading/session/dbbf695c-841c-43a9-87a6-6f163bb469cd | 33 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2921 | 844 | 906 | 584 | 908 | 0.0051 | 0.0 | 714196 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2846 | 808 | 839 | 688 | 1100 | 0.0041 | 0.0 | 630574 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2755 | 742 | 744 | 548 | 932 | 0.0041 | 0.0 | 630758 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2827 | 807 | 817 | 596 | 1040 | 0.0041 | 0.0 | 630713 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 817 | 828 | 560 | 1176 | 0.0041 | 0.0 | 632485 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2841 | 822 | 828 | 572 | 1292 | 0.0041 | 0.0 | 632679 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 731 | 741 | 548 | 1288 | 0.0041 | 0.0 | 632654 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2759 | 747 | 748 | 544 | 1172 | 0.0037 | 385.0 | 537037 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2941 | 901 | 934 | 576 | 1040 | 0.0037 | 0.0 | 642521 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 35 | 4 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2905 | 891 | 891 | 572 | 1072 | 0.0037 | 0.0 | 651879 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 30 | 10 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2926 | 907 | 907 | 552 | 1264 | 0.0037 | 0.0 | 724664 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2965 | 900 | 925 | 556 | 1488 | 0.0037 | 0.0 | 723424 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 24 | high | 0 | 0 | 0 | 0 | 6 | 3 | 0 | 6 | 0 | 0 | 6761 | 734 | 742 | 520 | 908 | 0.0037 | 5.0 | 641813 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3079 | 828 | 1070 | 508 | 1220 | 0.0723 | 0.0 | 634452 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2846 | 760 | 824 | 536 | 960 | 0.0037 | 0.0 | 631164 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2715 | 696 | 700 | 532 | 1332 | 0.0037 | 0.0 | 631892 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2922 | 910 | 910 | 504 | 1268 | 0.0037 | 0.0 | 633905 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 785 | 860 | 616 | 1064 | 0.0037 | 0.0 | 631150 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2871 | 785 | 860 | 540 | 924 | 0.0037 | 0.0 | 650117 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 35 | 3 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3694 | 850 | 955 | 556 | 1100 | 0.0037 | 0.0 | 794465 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2882 | 858 | 858 | 532 | 1032 | 0.0274 | 0.0 | 642176 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2893 | 877 | 877 | 536 | 1344 | 0.0037 | 0.0 | 634255 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2749 | 721 | 732 | 644 | 940 | 0.0037 | 0.0 | 631222 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2759 | 731 | 740 | 548 | 944 | 0.0037 | 0.0 | 631444 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2932 | 917 | 917 | 548 | 1228 | 0.0092 | 0.0 | 633157 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2888 | 873 | 874 | 536 | 996 | 0.0037 | 0.0 | 632579 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2781 | 753 | 769 | 532 | 948 | 0.0037 | 0.0 | 631794 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 737 | 744 | 540 | 1264 | 0.0037 | 0.0 | 632681 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2830 | 809 | 821 | 652 | 1044 | 0.0005 | 0.0 | 525654 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2954 | 939 | 939 | 776 | 1124 | 0.0018 | 0.0 | 525780 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2726 | 714 | 714 | 484 | 840 | 0.0018 | 0.0 | 525757 |
| logged-in-reader | desktop | reading | /vi/reading/session/5c925eeb-da7f-4823-b37a-ed1b218d0992 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2795 | 765 | 778 | 720 | 1120 | 0.0037 | 0.0 | 631928 |
| logged-in-reader | desktop | reading | /vi/reading/session/b54d3013-9175-466a-a0e9-d1656fe1f90c | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2836 | 818 | 823 | 736 | 1104 | 0.0040 | 0.0 | 631964 |
| logged-in-reader | desktop | reading | /vi/reading/session/949a0fbf-4b01-4b87-8037-37c80e86acc7 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2973 | 834 | 960 | 568 | 976 | 0.0037 | 0.0 | 724700 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2762 | 735 | 748 | 580 | 960 | 0.0037 | 0.0 | 630815 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2896 | 852 | 878 | 732 | 1108 | 0.0037 | 0.0 | 630888 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2846 | 809 | 834 | 552 | 988 | 0.0037 | 0.0 | 630857 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2833 | 779 | 817 | 536 | 1368 | 0.0037 | 0.0 | 632466 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2878 | 814 | 863 | 560 | 1224 | 0.0037 | 0.0 | 632642 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2870 | 815 | 855 | 564 | 1104 | 0.0037 | 0.0 | 632465 |
| logged-out | mobile | auth-public | /vi | 26 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3214 | 1202 | 1202 | 996 | 1352 | 0.0000 | 0.0 | 530425 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2704 | 695 | 695 | 476 | 476 | 0.0000 | 0.0 | 511706 |
| logged-out | mobile | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2753 | 741 | 742 | 488 | 488 | 0.0000 | 0.0 | 512266 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2683 | 673 | 673 | 464 | 464 | 0.0000 | 0.0 | 511303 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2753 | 742 | 742 | 476 | 476 | 0.0000 | 0.0 | 511403 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2734 | 717 | 717 | 584 | 584 | 0.0000 | 0.0 | 511559 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2765 | 751 | 751 | 480 | 504 | 0.0000 | 0.0 | 525307 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2713 | 699 | 699 | 468 | 484 | 0.0000 | 0.0 | 525278 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2717 | 706 | 706 | 476 | 804 | 0.0000 | 0.0 | 525289 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2777 | 764 | 764 | 516 | 884 | 0.0032 | 0.0 | 537021 |
| logged-in-admin | mobile | reading | /vi/reading | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2911 | 854 | 892 | 496 | 848 | 0.0000 | 0.0 | 642073 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 31 | 8 | high | 0 | 0 | 1 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 2911 | 900 | 900 | 484 | 1116 | 0.0000 | 0.0 | 644657 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 2 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2985 | 972 | 972 | 500 | 1148 | 0.0000 | 0.0 | 725846 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2856 | 842 | 842 | 452 | 852 | 0.0000 | 0.0 | 725412 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 27 | high | 0 | 0 | 0 | 0 | 13 | 4 | 5 | 12 | 1 | 0 | 5724 | 690 | 706 | 460 | 460 | 0.0000 | 5.0 | 642651 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3062 | 864 | 1051 | 476 | 1104 | 0.0689 | 0.0 | 634381 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2799 | 764 | 786 | 516 | 860 | 0.0000 | 0.0 | 631032 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 37 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2752 | 825 | 1027 | 468 | 1120 | 0.0760 | 0.0 | 630128 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 849 | 849 | 464 | 1096 | 0.0000 | 0.0 | 633832 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 840 | 851 | 492 | 836 | 0.0000 | 0.0 | 630969 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2822 | 752 | 810 | 464 | 812 | 0.0000 | 0.0 | 650071 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 35 | 3 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3667 | 800 | 914 | 484 | 996 | 0.0000 | 0.0 | 794136 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2859 | 835 | 845 | 468 | 848 | 0.0000 | 0.0 | 642107 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2881 | 859 | 859 | 484 | 1132 | 0.0000 | 0.0 | 634316 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2883 | 816 | 863 | 468 | 852 | 0.0000 | 0.0 | 631590 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 710 | 738 | 464 | 824 | 0.0000 | 0.0 | 631812 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 866 | 866 | 472 | 1132 | 0.0071 | 0.0 | 630436 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2741 | 705 | 727 | 448 | 808 | 0.0000 | 0.0 | 631449 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2857 | 833 | 845 | 492 | 836 | 0.0000 | 0.0 | 632022 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2737 | 705 | 723 | 464 | 880 | 0.0000 | 4.0 | 632262 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2657 | 642 | 642 | 444 | 784 | 0.0032 | 0.0 | 525526 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2747 | 733 | 733 | 484 | 796 | 0.0032 | 0.0 | 525671 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 826 | 826 | 484 | 812 | 0.0032 | 0.0 | 525794 |
| logged-in-admin | mobile | admin | /vi/admin | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2907 | 793 | 897 | 480 | 824 | 0.0000 | 0.0 | 648567 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2836 | 803 | 815 | 468 | 888 | 0.0000 | 0.0 | 647092 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 801 | 821 | 496 | 816 | 0.0000 | 0.0 | 645504 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2924 | 812 | 905 | 452 | 800 | 0.0000 | 0.0 | 697871 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2962 | 926 | 943 | 524 | 932 | 0.0000 | 0.0 | 644099 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2850 | 818 | 837 | 500 | 844 | 0.0000 | 0.0 | 645913 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2983 | 936 | 964 | 764 | 1108 | 0.0000 | 0.0 | 648214 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2917 | 803 | 888 | 476 | 828 | 0.0000 | 0.0 | 688767 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2981 | 876 | 964 | 476 | 1184 | 0.0000 | 0.0 | 651031 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2832 | 798 | 814 | 484 | 820 | 0.0000 | 0.0 | 645554 |
| logged-in-admin | mobile | reading | /vi/reading/session/6f8210b4-a511-4ee4-99a7-e0730efe4828 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2980 | 815 | 968 | 468 | 812 | 0.0000 | 0.0 | 692957 |
| logged-in-admin | mobile | reading | /vi/reading/session/4d5d85d4-2afd-42ca-90f5-a4541495a069 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2967 | 813 | 955 | 476 | 808 | 0.0000 | 0.0 | 692792 |
| logged-in-admin | mobile | reading | /vi/reading/session/f481bbdc-f180-4075-b725-dfd8bd1ec4c3 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3013 | 855 | 1000 | 472 | 880 | 0.0000 | 0.0 | 692564 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2961 | 907 | 943 | 564 | 940 | 0.0000 | 0.0 | 631381 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2846 | 816 | 826 | 480 | 820 | 0.0000 | 0.0 | 630779 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2898 | 849 | 881 | 496 | 876 | 0.0000 | 0.0 | 631177 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2831 | 808 | 819 | 464 | 832 | 0.0000 | 0.0 | 632307 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2807 | 786 | 796 | 452 | 800 | 0.0000 | 0.0 | 632346 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2784 | 747 | 765 | 564 | 916 | 0.0000 | 0.0 | 632552 |
| logged-in-reader | mobile | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2759 | 744 | 744 | 488 | 856 | 0.0028 | 0.0 | 537102 |
| logged-in-reader | mobile | reading | /vi/reading | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2908 | 845 | 883 | 464 | 856 | 0.0000 | 0.0 | 642015 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 35 | 4 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2942 | 894 | 897 | 528 | 908 | 0.0000 | 0.0 | 661218 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 35 | 5 | high | 0 | 0 | 0 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2977 | 891 | 965 | 488 | 932 | 0.0069 | 0.0 | 796734 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2936 | 894 | 919 | 512 | 880 | 0.0000 | 0.0 | 724761 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 24 | high | 0 | 0 | 0 | 0 | 6 | 2 | 2 | 6 | 0 | 0 | 5913 | 730 | 798 | 460 | 788 | 0.0000 | 8.0 | 641674 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3773 | 1427 | 1712 | 512 | 1436 | 0.0889 | 0.0 | 638443 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2717 | 684 | 703 | 452 | 796 | 0.0000 | 0.0 | 631225 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2870 | 834 | 854 | 476 | 844 | 0.0000 | 0.0 | 632074 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2918 | 898 | 899 | 536 | 1176 | 0.0000 | 0.0 | 633527 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2835 | 742 | 821 | 472 | 808 | 0.0000 | 0.0 | 631336 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2868 | 795 | 854 | 488 | 836 | 0.0000 | 0.0 | 650191 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 35 | 3 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3831 | 910 | 1077 | 504 | 1208 | 0.0051 | 1.0 | 794463 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2877 | 857 | 867 | 504 | 864 | 0.0000 | 0.0 | 642144 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2954 | 900 | 942 | 484 | 944 | 0.0000 | 0.0 | 633887 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2877 | 827 | 859 | 496 | 880 | 0.0000 | 0.0 | 631820 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2849 | 826 | 836 | 476 | 824 | 0.0000 | 0.0 | 631329 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2897 | 874 | 874 | 496 | 1128 | 0.0330 | 0.0 | 633190 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2825 | 809 | 809 | 464 | 808 | 0.0000 | 0.0 | 631841 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2881 | 842 | 866 | 648 | 984 | 0.0000 | 0.0 | 631992 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2763 | 724 | 747 | 456 | 808 | 0.0000 | 0.0 | 632559 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2768 | 721 | 756 | 520 | 852 | 0.0028 | 0.0 | 525649 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 719 | 740 | 500 | 836 | 0.0028 | 0.0 | 525675 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2761 | 741 | 742 | 484 | 816 | 0.0028 | 0.0 | 525807 |
| logged-in-reader | mobile | reading | /vi/reading/session/1c8022a9-71c4-4d9b-bdc7-7d42d9413c47 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3017 | 821 | 998 | 472 | 856 | 0.0000 | 0.0 | 692980 |
| logged-in-reader | mobile | reading | /vi/reading/session/b61cdc9c-52e7-4019-96b5-d60594bc7795 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3065 | 835 | 1052 | 504 | 880 | 0.0000 | 0.0 | 693116 |
| logged-in-reader | mobile | reading | /vi/reading/session/0127a99b-82f6-44ce-8c33-e3a974758097 | 28 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2850 | 809 | 839 | 544 | 900 | 0.0000 | 0.0 | 631746 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2759 | 716 | 741 | 456 | 784 | 0.0000 | 0.0 | 630864 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2823 | 753 | 807 | 468 | 800 | 0.0000 | 0.0 | 630818 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2792 | 752 | 773 | 484 | 828 | 0.0000 | 0.0 | 630997 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3126 | 1071 | 1107 | 512 | 1080 | 0.0069 | 0.0 | 632967 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3009 | 981 | 996 | 572 | 1004 | 0.0000 | 0.0 | 632432 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2869 | 802 | 854 | 468 | 868 | 0.0000 | 0.0 | 632793 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 13 | 3 | 4 | 12 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 6 | 3 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 13 | 4 | 5 | 12 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 6 | 2 | 2 | 6 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | high | 1 | 31 | 1 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/9efb4036-c003-4708-8e05-0835cb7574ee | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/513cf554-2eb5-4458-8c6f-acaf2685fca8 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/dbbf695c-841c-43a9-87a6-6f163bb469cd | 33 | high | 2 | 29 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 35 | high | 1 | 31 | 1 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/5c925eeb-da7f-4823-b37a-ed1b218d0992 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/b54d3013-9175-466a-a0e9-d1656fe1f90c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/949a0fbf-4b01-4b87-8037-37c80e86acc7 | 34 | high | 2 | 30 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 35 | high | 1 | 31 | 1 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/6f8210b4-a511-4ee4-99a7-e0730efe4828 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/4d5d85d4-2afd-42ca-90f5-a4541495a069 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/f481bbdc-f180-4075-b725-dfd8bd1ec4c3 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 35 | high | 0 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 35 | high | 1 | 31 | 1 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/1c8022a9-71c4-4d9b-bdc7-7d42d9413c47 | 33 | high | 2 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/b61cdc9c-52e7-4019-96b5-d60594bc7795 | 33 | high | 2 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/0127a99b-82f6-44ce-8c33-e3a974758097 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 1377 | 318 | html | https://www.tarotnow.xyz/vi/profile |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1306 | 564 | static | https://www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1304 | 556 | static | https://www.tarotnow.xyz/_next/static/chunks/0jpe042aa74zf.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1173 | 395 | html | https://www.tarotnow.xyz/vi |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1055 | 676 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 1021 | 333 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 933 | 329 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 909 | 333 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | GET | 200 | 887 | 548 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 886 | 356 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 883 | 327 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 879 | 415 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | admin | /vi/admin/promotions | GET | 200 | 871 | 337 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 870 | 368 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 864 | 335 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 858 | 321 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 854 | 321 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 852 | 357 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 850 | 308 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 845 | 319 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 844 | 368 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 832 | 339 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 829 | 315 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 828 | 397 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 823 | 310 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 820 | 324 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 815 | 323 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 814 | 316 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 812 | 305 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 812 | 313 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 811 | 340 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 808 | 317 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 807 | 340 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 806 | 332 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 804 | 325 | html | https://www.tarotnow.xyz/vi/notifications |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 800 | 311 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 800 | 320 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 798 | 332 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 798 | 322 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 798 | 327 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 796 | 321 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 796 | 324 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 796 | 318 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 793 | 322 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-out | desktop | auth-public | /vi/reset-password | GET | 200 | 790 | 325 | html | https://www.tarotnow.xyz/vi/reset-password |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 790 | 316 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 790 | 335 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 789 | 543 | static | https://www.tarotnow.xyz/_next/static/chunks/0w.yuzykfwjex.js |
| logged-in-admin | mobile | reading | /vi/reading/session/f481bbdc-f180-4075-b725-dfd8bd1ec4c3 | GET | 200 | 789 | 336 | html | https://www.tarotnow.xyz/vi/reading/session/f481bbdc-f180-4075-b725-dfd8bd1ec4c3 |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 789 | 310 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 786 | 321 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | GET | 200 | 786 | 345 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 784 | 355 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 780 | 307 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 779 | 537 | static | https://www.tarotnow.xyz/_next/static/chunks/0gyj3cptqnt.8.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 778 | 56 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 777 | 316 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 777 | 315 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 776 | 423 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | mobile | reading | /vi/reading/session/b61cdc9c-52e7-4019-96b5-d60594bc7795 | GET | 200 | 775 | 324 | html | https://www.tarotnow.xyz/vi/reading/session/b61cdc9c-52e7-4019-96b5-d60594bc7795 |
| logged-in-reader | desktop | reading | /vi/reading/session/b54d3013-9175-466a-a0e9-d1656fe1f90c | GET | 200 | 768 | 311 | html | https://www.tarotnow.xyz/vi/reading/session/b54d3013-9175-466a-a0e9-d1656fe1f90c |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 767 | 317 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-reader | mobile | reading | /vi/reading/session/1c8022a9-71c4-4d9b-bdc7-7d42d9413c47 | GET | 200 | 767 | 333 | html | https://www.tarotnow.xyz/vi/reading/session/1c8022a9-71c4-4d9b-bdc7-7d42d9413c47 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 764 | 300 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 760 | 312 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 758 | 332 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 756 | 412 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 756 | 311 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 756 | 328 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 754 | 334 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 753 | 312 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 752 | 327 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 751 | 399 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 750 | 323 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 748 | 311 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 748 | 317 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 748 | 328 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 747 | 345 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 746 | 322 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 745 | 320 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 745 | 325 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 745 | 333 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | GET | 200 | 743 | 399 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-reader | mobile | reading | /vi/reading/session/0127a99b-82f6-44ce-8c33-e3a974758097 | GET | 200 | 741 | 405 | html | https://www.tarotnow.xyz/vi/reading/session/0127a99b-82f6-44ce-8c33-e3a974758097 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 738 | 316 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 738 | 342 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 737 | 346 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 735 | 316 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 734 | 326 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 733 | 334 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 732 | 316 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 730 | 336 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 729 | 438 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | reading | /vi/reading/session/949a0fbf-4b01-4b87-8037-37c80e86acc7 | GET | 200 | 727 | 325 | html | https://www.tarotnow.xyz/vi/reading/session/949a0fbf-4b01-4b87-8037-37c80e86acc7 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 727 | 324 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 727 | 336 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 727 | 339 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 726 | 345 | html | https://www.tarotnow.xyz/vi/collection |
| logged-out | desktop | auth-public | /vi/register | GET | 200 | 724 | 335 | html | https://www.tarotnow.xyz/vi/register |
| logged-in-admin | desktop | reading | /vi/reading/session/dbbf695c-841c-43a9-87a6-6f163bb469cd | GET | 200 | 724 | 320 | html | https://www.tarotnow.xyz/vi/reading/session/dbbf695c-841c-43a9-87a6-6f163bb469cd |
| logged-in-reader | desktop | reader-chat | /vi/chat | GET | 200 | 724 | 392 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | admin | /vi/admin/readings | GET | 200 | 721 | 312 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 719 | 358 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-reader | desktop | reading | /vi/reading/session/5c925eeb-da7f-4823-b37a-ed1b218d0992 | GET | 200 | 717 | 306 | html | https://www.tarotnow.xyz/vi/reading/session/5c925eeb-da7f-4823-b37a-ed1b218d0992 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 716 | 337 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | mobile | reading | /vi/reading/session/4d5d85d4-2afd-42ca-90f5-a4541495a069 | GET | 200 | 716 | 329 | html | https://www.tarotnow.xyz/vi/reading/session/4d5d85d4-2afd-42ca-90f5-a4541495a069 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 715 | 355 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | GET | 200 | 714 | 324 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | desktop | admin | /vi/admin/users | GET | 200 | 713 | 325 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-admin | mobile | reading | /vi/reading/session/6f8210b4-a511-4ee4-99a7-e0730efe4828 | GET | 200 | 712 | 329 | html | https://www.tarotnow.xyz/vi/reading/session/6f8210b4-a511-4ee4-99a7-e0730efe4828 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 709 | 321 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | GET | 200 | 705 | 323 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 705 | 317 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | reading | /vi/reading/session/513cf554-2eb5-4458-8c6f-acaf2685fca8 | GET | 200 | 704 | 331 | html | https://www.tarotnow.xyz/vi/reading/session/513cf554-2eb5-4458-8c6f-acaf2685fca8 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 704 | 308 | html | https://www.tarotnow.xyz/vi/community |

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
| logged-in-admin | desktop | reading.init.spread_3: created 9efb4036-c003-4708-8e05-0835cb7574ee. |
| logged-in-admin | desktop | reading.init.spread_5: created 513cf554-2eb5-4458-8c6f-acaf2685fca8. |
| logged-in-admin | desktop | reading.init.spread_10: created dbbf695c-841c-43a9-87a6-6f163bb469cd. |
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
| logged-in-reader | desktop | reading.init.spread_3: created 5c925eeb-da7f-4823-b37a-ed1b218d0992. |
| logged-in-reader | desktop | reading.init.spread_5: created b54d3013-9175-466a-a0e9-d1656fe1f90c. |
| logged-in-reader | desktop | reading.init.spread_10: created 949a0fbf-4b01-4b87-8037-37c80e86acc7. |
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
| logged-in-admin | mobile | reading.init.spread_3: created 6f8210b4-a511-4ee4-99a7-e0730efe4828. |
| logged-in-admin | mobile | reading.init.spread_5: created 4d5d85d4-2afd-42ca-90f5-a4541495a069. |
| logged-in-admin | mobile | reading.init.spread_10: created f481bbdc-f180-4075-b725-dfd8bd1ec4c3. |
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
| logged-in-reader | mobile | reading.init.spread_3: created 1c8022a9-71c4-4d9b-bdc7-7d42d9413c47. |
| logged-in-reader | mobile | reading.init.spread_5: created b61cdc9c-52e7-4019-96b5-d60594bc7795. |
| logged-in-reader | mobile | reading.init.spread_10: created 0127a99b-82f6-44ce-8c33-e3a974758097. |
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

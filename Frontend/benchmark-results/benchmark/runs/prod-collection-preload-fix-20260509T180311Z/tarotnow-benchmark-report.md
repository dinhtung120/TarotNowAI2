# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T18:12:54.962Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 9
- High pages (request count): 142
- High slow requests: 38
- Medium slow requests: 232

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2872 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 3000 | 1272 | 2 | 0 | 9 | 3 | yes |
| logged-in-reader | desktop | 33 | 3043 | 1021 | 1 | 0 | 13 | 0 | yes |
| logged-out | mobile | 9 | 2762 | 225 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 2960 | 1295 | 1 | 0 | 11 | 0 | yes |
| logged-in-reader | mobile | 33 | 3048 | 1058 | 1 | 0 | 22 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2895 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2742 | 0 |
| logged-in-admin | desktop | collection | 1 | 31.0 | 6703 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 3990 | 0 |
| logged-in-admin | desktop | gacha | 2 | 30.0 | 2871 | 0 |
| logged-in-admin | desktop | gamification | 1 | 36.0 | 3426 | 1 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2790 | 0 |
| logged-in-admin | desktop | inventory | 1 | 35.0 | 3135 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 3269 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2752 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2754 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2853 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2772 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.0 | 2761 | 0 |
| logged-in-admin | desktop | reading | 5 | 28.4 | 2866 | 0 |
| logged-in-admin | desktop | wallet | 4 | 35.0 | 3066 | 1 |
| logged-in-admin | mobile | admin | 10 | 29.3 | 2876 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 3009 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5627 | 0 |
| logged-in-admin | mobile | community | 1 | 30.0 | 3681 | 0 |
| logged-in-admin | mobile | gacha | 2 | 33.5 | 2943 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2840 | 1 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2749 | 0 |
| logged-in-admin | mobile | inventory | 1 | 33.0 | 2841 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 32.0 | 3001 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2757 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2695 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 2881 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2759 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.6 | 2823 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.4 | 2835 | 0 |
| logged-in-admin | mobile | wallet | 4 | 40.8 | 3148 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2828 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6366 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3676 | 0 |
| logged-in-reader | desktop | gacha | 2 | 32.0 | 2907 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2874 | 1 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2775 | 0 |
| logged-in-reader | desktop | inventory | 1 | 30.0 | 2863 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2881 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2723 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2887 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.7 | 2861 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2789 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.6 | 2844 | 0 |
| logged-in-reader | desktop | reading | 5 | 28.2 | 2806 | 0 |
| logged-in-reader | desktop | wallet | 4 | 48.3 | 3497 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2825 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5518 | 0 |
| logged-in-reader | mobile | community | 1 | 30.0 | 3483 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.0 | 2887 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2867 | 1 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2706 | 0 |
| logged-in-reader | mobile | inventory | 1 | 33.0 | 2849 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2829 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2963 | 0 |
| logged-in-reader | mobile | notifications | 1 | 31.0 | 2926 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.7 | 2881 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2767 | 0 |
| logged-in-reader | mobile | readers | 7 | 29.6 | 2848 | 0 |
| logged-in-reader | mobile | reading | 5 | 32.2 | 2934 | 0 |
| logged-in-reader | mobile | wallet | 4 | 49.0 | 3477 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2790 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3695 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2732 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2687 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3207 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2738 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3695 | 1312 | 1679 | 1144 | 1144 | 0.0000 | 340.0 | 601456 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2766 | 757 | 757 | 652 | 652 | 0.0000 | 0.0 | 512427 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2793 | 787 | 787 | 644 | 644 | 0.0000 | 0.0 | 512940 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2846 | 839 | 839 | 808 | 808 | 0.0000 | 0.0 | 512049 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2743 | 736 | 736 | 588 | 588 | 0.0000 | 0.0 | 512088 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2804 | 796 | 796 | 644 | 644 | 0.0000 | 0.0 | 512211 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 721 | 721 | 592 | 592 | 0.0000 | 0.0 | 525963 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2745 | 736 | 736 | 608 | 608 | 0.0000 | 0.0 | 526000 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2722 | 712 | 712 | 624 | 624 | 0.0000 | 0.0 | 526108 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2790 | 782 | 782 | 684 | 1196 | 0.0035 | 124.0 | 537712 |
| logged-in-admin | desktop | reading | /vi/reading | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2860 | 835 | 854 | 664 | 1012 | 0.0041 | 0.0 | 641906 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 35 | 6 | high | 0 | 0 | 0 | 0 | 5 | 1 | 4 | 5 | 0 | 0 | 3135 | 1103 | 1103 | 800 | 1212 | 0.0041 | 0.0 | 652388 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 30 | 12 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2869 | 857 | 861 | 692 | 1148 | 0.0041 | 0.0 | 725159 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2873 | 864 | 864 | 628 | 1264 | 0.0041 | 0.0 | 726331 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 31 | 33 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6703 | 674 | 1164 | 604 | 992 | 0.0042 | 0.0 | 646053 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3028 | 743 | 1022 | 512 | 928 | 0.0489 | 0.0 | 636446 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2807 | 779 | 799 | 628 | 976 | 0.0041 | 0.0 | 631955 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 41 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2725 | 825 | 1023 | 676 | 1060 | 0.0489 | 0.0 | 631138 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2830 | 823 | 823 | 592 | 988 | 0.0041 | 0.0 | 634151 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2742 | 723 | 735 | 516 | 928 | 0.0041 | 0.0 | 631921 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3269 | 727 | 1261 | 616 | 972 | 0.0179 | 0.0 | 652569 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | 5 | high | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3990 | 787 | 1225 | 664 | 1984 | 0.0041 | 0.0 | 777774 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 36 | 11 | critical | 0 | 0 | 2 | 3 | 0 | 0 | 0 | 0 | 0 | 0 | 3426 | 1416 | 1416 | 536 | 1460 | 0.0174 | 0.0 | 647003 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3880 | 1121 | 1121 | 1040 | 1572 | 0.0000 | 11.0 | 1135551 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2797 | 781 | 796 | 636 | 1020 | 0.0041 | 0.0 | 631823 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2823 | 804 | 821 | 656 | 1212 | 0.0041 | 0.0 | 632587 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 41 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2765 | 901 | 901 | 768 | 1252 | 0.0041 | 0.0 | 631223 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 739 | 753 | 604 | 1028 | 0.0041 | 0.0 | 632183 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2772 | 756 | 771 | 596 | 988 | 0.0041 | 0.0 | 632696 |
| logged-in-admin | desktop | reading | /vi/reading/history | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2883 | 855 | 863 | 552 | 1024 | 0.0041 | 0.0 | 635131 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2716 | 711 | 711 | 624 | 956 | 0.0020 | 0.0 | 526276 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2773 | 767 | 767 | 616 | 904 | 0.0020 | 0.0 | 526323 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2768 | 761 | 761 | 640 | 1004 | 0.0020 | 0.0 | 526284 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3218 | 754 | 1210 | 560 | 976 | 0.0000 | 0.0 | 647712 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2857 | 852 | 852 | 824 | 1264 | 0.0000 | 0.0 | 647862 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2858 | 839 | 853 | 624 | 936 | 0.0000 | 0.0 | 646288 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3372 | 767 | 1362 | 612 | 960 | 0.0022 | 0.0 | 699319 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2792 | 784 | 784 | 580 | 948 | 0.0000 | 0.0 | 644694 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2804 | 785 | 797 | 628 | 964 | 0.0000 | 0.0 | 646591 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2786 | 759 | 778 | 628 | 1068 | 0.0000 | 0.0 | 648914 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2777 | 755 | 771 | 664 | 1108 | 0.0000 | 0.0 | 656122 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2739 | 729 | 729 | 628 | 988 | 0.0000 | 0.0 | 650100 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2748 | 739 | 739 | 644 | 960 | 0.0000 | 0.0 | 646322 |
| logged-in-admin | desktop | reading | /vi/reading/session/2fdb7785-2e4b-405a-aaf6-1bf1580e54f0 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2956 | 932 | 948 | 764 | 1136 | 0.0041 | 0.0 | 632386 |
| logged-in-admin | desktop | reading | /vi/reading/session/7324b3b6-4edb-40ce-b187-45cc41ed705e | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2846 | 821 | 838 | 628 | 1000 | 0.0044 | 0.0 | 632558 |
| logged-in-admin | desktop | reading | /vi/reading/session/1cbc149c-7ce8-4c38-81d1-08eb19d7f163 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2785 | 762 | 779 | 644 | 1040 | 0.0041 | 0.0 | 632496 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2764 | 738 | 753 | 580 | 960 | 0.0041 | 0.0 | 631354 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2765 | 743 | 758 | 668 | 1008 | 0.0041 | 0.0 | 631504 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2795 | 739 | 785 | 796 | 796 | 0.0041 | 0.0 | 631452 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 730 | 749 | 624 | 1192 | 0.0041 | 0.0 | 632910 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2708 | 682 | 699 | 596 | 1132 | 0.0041 | 0.0 | 633255 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2710 | 688 | 704 | 536 | 1096 | 0.0041 | 0.0 | 633277 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2775 | 766 | 766 | 704 | 1296 | 0.0033 | 190.0 | 537817 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2925 | 904 | 913 | 592 | 1000 | 0.0039 | 0.0 | 643200 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 30 | 11 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2863 | 855 | 856 | 608 | 1048 | 0.0039 | 0.0 | 644672 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2890 | 881 | 881 | 528 | 1220 | 0.0039 | 0.0 | 728346 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2923 | 909 | 909 | 540 | 1276 | 0.0039 | 0.0 | 725179 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 30 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6366 | 754 | 776 | 520 | 884 | 0.0040 | 0.0 | 642326 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3088 | 835 | 1080 | 620 | 1012 | 0.0726 | 0.0 | 637662 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2790 | 759 | 781 | 528 | 928 | 0.0039 | 0.0 | 631778 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2706 | 670 | 696 | 604 | 1388 | 0.0039 | 0.0 | 632570 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2878 | 871 | 871 | 648 | 1040 | 0.0039 | 0.0 | 634965 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2828 | 798 | 818 | 684 | 1064 | 0.0039 | 0.0 | 632082 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2881 | 847 | 873 | 620 | 964 | 0.0177 | 0.0 | 649827 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3676 | 834 | 852 | 620 | 1732 | 0.0039 | 0.0 | 643387 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2874 | 865 | 865 | 860 | 1236 | 0.0039 | 0.0 | 642252 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3768 | 1061 | 1061 | 932 | 1524 | 0.0000 | 24.0 | 1135451 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 80 | 13 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4531 | 702 | 703 | 596 | 1160 | 0.0033 | 139.0 | 1635273 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2788 | 752 | 779 | 604 | 1008 | 0.0039 | 0.0 | 631954 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2902 | 892 | 892 | 688 | 1084 | 0.0095 | 0.0 | 634051 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2887 | 876 | 876 | 636 | 1052 | 0.0040 | 0.0 | 633559 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2789 | 761 | 779 | 528 | 928 | 0.0039 | 0.0 | 632638 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2798 | 764 | 787 | 628 | 1124 | 0.0039 | 0.0 | 633346 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2702 | 691 | 691 | 604 | 928 | 0.0019 | 0.0 | 526320 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 756 | 756 | 668 | 952 | 0.0019 | 0.0 | 526302 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2699 | 691 | 691 | 508 | 868 | 0.0019 | 0.0 | 526323 |
| logged-in-reader | desktop | reading | /vi/reading/session/998548bd-dcba-4914-b112-3e624fe09489 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2758 | 724 | 748 | 636 | 1016 | 0.0039 | 0.0 | 632277 |
| logged-in-reader | desktop | reading | /vi/reading/session/96c410ed-b4e1-4ee9-b52c-16d361695568 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2721 | 710 | 711 | 488 | 872 | 0.0039 | 0.0 | 632265 |
| logged-in-reader | desktop | reading | /vi/reading/session/099798c5-6eea-4047-a8ae-5041245bdf06 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2827 | 797 | 817 | 612 | 1012 | 0.0039 | 0.0 | 632394 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2767 | 731 | 756 | 608 | 1008 | 0.0039 | 0.0 | 631465 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2825 | 793 | 815 | 628 | 996 | 0.0039 | 0.0 | 631509 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3009 | 807 | 999 | 656 | 1032 | 0.0039 | 0.0 | 634530 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2811 | 784 | 801 | 640 | 1204 | 0.0039 | 0.0 | 633090 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2830 | 795 | 817 | 612 | 1168 | 0.0039 | 0.0 | 633214 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2786 | 753 | 776 | 588 | 1080 | 0.0039 | 0.0 | 633382 |
| logged-out | mobile | auth-public | /vi | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3207 | 1200 | 1200 | 976 | 976 | 0.0000 | 0.0 | 602771 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2725 | 714 | 714 | 596 | 596 | 0.0000 | 0.0 | 512362 |
| logged-out | mobile | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2662 | 642 | 651 | 612 | 612 | 0.0000 | 0.0 | 512883 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2653 | 641 | 641 | 540 | 540 | 0.0000 | 0.0 | 511963 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2714 | 704 | 704 | 644 | 644 | 0.0000 | 0.0 | 512063 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2682 | 672 | 672 | 576 | 576 | 0.0000 | 0.0 | 512242 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2690 | 678 | 678 | 472 | 788 | 0.0000 | 0.0 | 525800 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2737 | 725 | 725 | 588 | 588 | 0.0000 | 0.0 | 525919 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2787 | 777 | 777 | 612 | 612 | 0.0000 | 0.0 | 526116 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2749 | 738 | 738 | 636 | 984 | 0.0032 | 0.0 | 537803 |
| logged-in-admin | mobile | reading | /vi/reading | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2892 | 825 | 885 | 544 | 880 | 0.0000 | 0.0 | 643046 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 33 | 8 | high | 0 | 0 | 2 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 2841 | 832 | 832 | 448 | 1068 | 0.0000 | 0.0 | 647944 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 36 | 6 | critical | 0 | 0 | 1 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 2969 | 851 | 959 | 588 | 944 | 0.0000 | 0.0 | 797983 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2917 | 836 | 903 | 576 | 932 | 0.0000 | 0.0 | 727256 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 32 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5627 | 858 | 860 | 700 | 700 | 0.0000 | 0.0 | 643272 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3111 | 872 | 1104 | 484 | 1124 | 0.0689 | 0.0 | 636324 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2801 | 765 | 790 | 580 | 924 | 0.0000 | 0.0 | 631601 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 41 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 845 | 1030 | 604 | 948 | 0.0760 | 0.0 | 630923 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2863 | 850 | 850 | 476 | 1096 | 0.0000 | 0.0 | 636104 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3009 | 977 | 990 | 736 | 1092 | 0.0000 | 0.0 | 631560 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3001 | 917 | 982 | 540 | 884 | 0.0196 | 0.0 | 652200 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3681 | 774 | 863 | 588 | 2272 | 0.0051 | 0.0 | 643207 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 827 | 827 | 496 | 840 | 0.0000 | 0.0 | 642072 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 27 | 36 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2742 | 988 | 988 | 888 | 1256 | 0.0000 | 0.0 | 607162 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 80 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4231 | 634 | 634 | 512 | 876 | 0.0000 | 0.0 | 1635584 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2801 | 764 | 788 | 556 | 972 | 0.0000 | 0.0 | 632444 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 40 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 871 | 871 | 672 | 1016 | 0.0071 | 0.0 | 631080 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2695 | 658 | 684 | 440 | 784 | 0.0000 | 0.0 | 632157 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2759 | 731 | 747 | 492 | 824 | 0.0000 | 0.0 | 632526 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2825 | 777 | 813 | 464 | 904 | 0.0000 | 0.0 | 632742 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2712 | 699 | 699 | 476 | 788 | 0.0032 | 0.0 | 526130 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2698 | 687 | 687 | 476 | 788 | 0.0032 | 0.0 | 526266 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2862 | 848 | 848 | 740 | 1060 | 0.0032 | 0.0 | 526426 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2821 | 783 | 811 | 576 | 908 | 0.0000 | 0.0 | 647702 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2919 | 887 | 907 | 716 | 1068 | 0.0000 | 0.0 | 647739 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2914 | 875 | 899 | 468 | 872 | 0.0000 | 0.0 | 646033 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2942 | 824 | 924 | 472 | 804 | 0.0000 | 0.0 | 698416 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2898 | 869 | 887 | 596 | 920 | 0.0000 | 0.0 | 644737 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2863 | 847 | 852 | 656 | 976 | 0.0000 | 0.0 | 646635 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2989 | 931 | 980 | 560 | 912 | 0.0000 | 0.0 | 650439 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2812 | 787 | 800 | 576 | 948 | 0.0000 | 0.0 | 655863 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2797 | 779 | 781 | 472 | 1112 | 0.0000 | 0.0 | 650095 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2807 | 778 | 793 | 548 | 872 | 0.0000 | 0.0 | 646203 |
| logged-in-admin | mobile | reading | /vi/reading/session/97c029e6-9b44-446f-8358-b48b6747b348 | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2740 | 702 | 729 | 464 | 800 | 0.0000 | 0.0 | 632235 |
| logged-in-admin | mobile | reading | /vi/reading/session/d107f458-aeb5-413a-a6ee-78c59b7c2edc | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2939 | 815 | 931 | 464 | 800 | 0.0000 | 0.0 | 695052 |
| logged-in-admin | mobile | reading | /vi/reading/session/b3f31f62-ad26-4c13-8dd5-47cbfe88348b | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2781 | 742 | 768 | 492 | 836 | 0.0000 | 0.0 | 632382 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2748 | 718 | 738 | 452 | 792 | 0.0000 | 0.0 | 631056 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2941 | 916 | 926 | 568 | 884 | 0.0000 | 0.0 | 633480 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2818 | 784 | 806 | 620 | 956 | 0.0000 | 0.0 | 631087 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2832 | 771 | 823 | 480 | 828 | 0.0000 | 0.0 | 632928 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 820 | 827 | 492 | 828 | 0.0000 | 0.0 | 632794 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2722 | 687 | 710 | 472 | 828 | 0.0000 | 0.0 | 633090 |
| logged-in-reader | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2706 | 677 | 692 | 480 | 832 | 0.0032 | 0.0 | 537738 |
| logged-in-reader | mobile | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2905 | 850 | 873 | 460 | 848 | 0.0000 | 0.0 | 645034 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 33 | 8 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2849 | 835 | 835 | 452 | 1128 | 0.0000 | 0.0 | 648049 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 36 | 6 | critical | 0 | 0 | 1 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2907 | 889 | 889 | 548 | 904 | 0.0000 | 0.0 | 798185 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2866 | 849 | 849 | 664 | 988 | 0.0000 | 0.0 | 724461 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 30 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5518 | 674 | 697 | 544 | 544 | 0.0000 | 18.0 | 642400 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3057 | 837 | 1047 | 628 | 968 | 0.0000 | 0.0 | 637902 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2812 | 779 | 799 | 524 | 844 | 0.0000 | 0.0 | 631833 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2774 | 742 | 763 | 564 | 1304 | 0.0000 | 0.0 | 632636 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 853 | 853 | 464 | 1112 | 0.0000 | 0.0 | 637359 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2825 | 789 | 812 | 580 | 920 | 0.0000 | 0.0 | 631829 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2829 | 752 | 816 | 476 | 812 | 0.0196 | 0.0 | 649973 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3483 | 733 | 736 | 552 | 1668 | 0.0051 | 0.0 | 643443 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 853 | 853 | 572 | 900 | 0.0000 | 0.0 | 642384 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 56 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3746 | 1026 | 1027 | 780 | 1136 | 0.0000 | 0.0 | 1135590 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 80 | 13 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4501 | 744 | 744 | 456 | 820 | 0.0055 | 0.0 | 1636201 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 735 | 804 | 464 | 784 | 0.0000 | 0.0 | 634391 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2843 | 825 | 826 | 480 | 1120 | 0.0330 | 0.0 | 635614 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2926 | 911 | 911 | 516 | 856 | 0.0000 | 0.0 | 635706 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2767 | 728 | 750 | 460 | 788 | 0.0000 | 0.0 | 632664 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2816 | 754 | 804 | 468 | 800 | 0.0000 | 0.0 | 633140 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2700 | 687 | 687 | 456 | 764 | 0.0032 | 0.0 | 526372 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2845 | 818 | 818 | 548 | 852 | 0.0032 | 0.0 | 526456 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3345 | 1329 | 1329 | 468 | 1320 | 0.0055 | 0.0 | 526313 |
| logged-in-reader | mobile | reading | /vi/reading/session/669dd000-19ea-49a3-8d21-b3530ac0bca9 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3077 | 932 | 1062 | 456 | 928 | 0.0001 | 0.0 | 695175 |
| logged-in-reader | mobile | reading | /vi/reading/session/9e0b088e-2599-4414-a1e0-9b52fea9a1d2 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2935 | 808 | 924 | 448 | 800 | 0.0000 | 0.0 | 695104 |
| logged-in-reader | mobile | reading | /vi/reading/session/2d48d025-8101-427e-90c9-4490ee63428e | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2938 | 796 | 924 | 464 | 804 | 0.0000 | 0.0 | 695163 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2842 | 792 | 820 | 460 | 800 | 0.0000 | 0.0 | 633458 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 766 | 808 | 480 | 816 | 0.0000 | 0.0 | 631460 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2852 | 807 | 836 | 468 | 820 | 0.0000 | 0.0 | 633686 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2899 | 868 | 873 | 468 | 864 | 0.0000 | 0.0 | 635194 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2883 | 838 | 863 | 496 | 840 | 0.0000 | 0.0 | 635223 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2775 | 746 | 760 | 488 | 836 | 0.0000 | 0.0 | 633264 |

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
| logged-in-admin | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 36 | critical | 6 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 56 | critical | 3 | 49 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/2fdb7785-2e4b-405a-aaf6-1bf1580e54f0 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/7324b3b6-4edb-40ce-b187-45cc41ed705e | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/1cbc149c-7ce8-4c38-81d1-08eb19d7f163 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 56 | critical | 3 | 49 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 80 | critical | 3 | 71 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/998548bd-dcba-4914-b112-3e624fe09489 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/96c410ed-b4e1-4ee9-b52c-16d361695568 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/099798c5-6eea-4047-a8ae-5041245bdf06 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 33 | high | 3 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 36 | critical | 1 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 27 | high | 0 | 25 | 0 |
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
| logged-in-admin | mobile | admin | /vi/admin/readings | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/97c029e6-9b44-446f-8358-b48b6747b348 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/d107f458-aeb5-413a-a6ee-78c59b7c2edc | 34 | high | 3 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/b3f31f62-ad26-4c13-8dd5-47cbfe88348b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 33 | high | 3 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 36 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 56 | critical | 3 | 49 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 80 | critical | 3 | 71 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/669dd000-19ea-49a3-8d21-b3530ac0bca9 | 34 | high | 3 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/9e0b088e-2599-4414-a1e0-9b52fea9a1d2 | 34 | high | 3 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/2d48d025-8101-427e-90c9-4490ee63428e | 34 | high | 3 | 29 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 1367 | 334 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 1276 | 313 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1030 | 572 | html | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1019 | 545 | static | https://www.tarotnow.xyz/_next/static/chunks/0.ekkbyazw_22.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 1015 | 562 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-out | desktop | auth-public | /vi | GET | 200 | 992 | 342 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 969 | 503 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 924 | 494 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 918 | 119 | static | https://www.tarotnow.xyz/_next/static/chunks/0.ekkbyazw_22.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 892 | 562 | static | https://www.tarotnow.xyz/_next/static/chunks/0o3-.av4h_47m.js |
| logged-in-admin | desktop | reading | /vi/reading/session/2fdb7785-2e4b-405a-aaf6-1bf1580e54f0 | GET | 200 | 883 | 331 | html | https://www.tarotnow.xyz/vi/reading/session/2fdb7785-2e4b-405a-aaf6-1bf1580e54f0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 879 | 861 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=48&q=75 |
| logged-in-reader | mobile | reading | /vi/reading/session/669dd000-19ea-49a3-8d21-b3530ac0bca9 | GET | 200 | 867 | 310 | html | https://www.tarotnow.xyz/vi/reading/session/669dd000-19ea-49a3-8d21-b3530ac0bca9 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 863 | 358 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 860 | 317 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 839 | 352 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 836 | 318 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 835 | 818 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=48&q=75 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 833 | 815 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Ffree_draw_ticket_50_20260416_165452.avif&w=48&q=75 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 832 | 333 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 831 | 326 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 827 | 339 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 827 | 395 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 825 | 336 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 825 | 314 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 823 | 333 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 814 | 338 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 813 | 350 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 810 | 320 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 810 | 325 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 808 | 359 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 808 | 349 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 808 | 338 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 807 | 565 | static | https://www.tarotnow.xyz/_next/static/chunks/0wz45fkbh96rt.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 806 | 323 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 805 | 327 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 801 | 785 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fdefense_booster_50_20260416_165452.avif&w=48&q=75 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | GET | 200 | 801 | 333 | html | https://www.tarotnow.xyz/vi/admin/deposits |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 800 | 333 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 797 | 318 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 796 | 779 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fexp_booster_50_20260416_165452.avif&w=48&q=75 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 796 | 329 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 796 | 325 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 795 | 424 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 793 | 326 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 793 | 356 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 791 | 312 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-out | desktop | auth-public | /vi/forgot-password | GET | 200 | 789 | 332 | html | https://www.tarotnow.xyz/vi/forgot-password |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 789 | 323 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 788 | 333 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 787 | 565 | static | https://www.tarotnow.xyz/_next/static/chunks/0-8rlz0vpjwdd.js |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 785 | 361 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 782 | 320 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 779 | 321 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 778 | 316 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 777 | 317 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 775 | 320 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 774 | 318 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 773 | 326 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 773 | 351 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | desktop | reading | /vi/reading/session/7324b3b6-4edb-40ce-b187-45cc41ed705e | GET | 200 | 772 | 324 | html | https://www.tarotnow.xyz/vi/reading/session/7324b3b6-4edb-40ce-b187-45cc41ed705e |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 771 | 315 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | GET | 200 | 769 | 420 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 766 | 321 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | desktop | reading | /vi/reading/history | GET | 200 | 763 | 316 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 762 | 321 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 760 | 329 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | reading | /vi/reading/session/9e0b088e-2599-4414-a1e0-9b52fea9a1d2 | GET | 200 | 760 | 323 | html | https://www.tarotnow.xyz/vi/reading/session/9e0b088e-2599-4414-a1e0-9b52fea9a1d2 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 758 | 324 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 757 | 330 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 755 | 336 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | reading | /vi/reading/session/d107f458-aeb5-413a-a6ee-78c59b7c2edc | GET | 200 | 753 | 322 | html | https://www.tarotnow.xyz/vi/reading/session/d107f458-aeb5-413a-a6ee-78c59b7c2edc |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 748 | 321 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | reading | /vi/reading/session/099798c5-6eea-4047-a8ae-5041245bdf06 | GET | 200 | 746 | 341 | html | https://www.tarotnow.xyz/vi/reading/session/099798c5-6eea-4047-a8ae-5041245bdf06 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 745 | 323 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-out | desktop | auth-public | /vi/verify-email | GET | 200 | 743 | 330 | html | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-reader | desktop | reader-chat | /vi/chat | GET | 200 | 743 | 402 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 743 | 316 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 739 | 325 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-reader | mobile | reader-chat | /vi/chat | GET | 200 | 739 | 333 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 738 | 355 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 738 | 311 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 737 | 730 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=256&q=75 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 737 | 333 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 734 | 317 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 732 | 348 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-out | desktop | auth-public | /vi/register | GET | 200 | 730 | 355 | html | https://www.tarotnow.xyz/vi/register |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 730 | 333 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 730 | 318 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 730 | 328 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 729 | 333 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 728 | 329 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 726 | 325 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 726 | 321 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-admin | desktop | admin | /vi/admin/readings | GET | 200 | 726 | 327 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 722 | 322 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 722 | 349 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 720 | 327 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 718 | 346 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | auth-public | /vi | GET | 200 | 717 | 325 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | reading | /vi/reading/session/1cbc149c-7ce8-4c38-81d1-08eb19d7f163 | GET | 200 | 715 | 316 | html | https://www.tarotnow.xyz/vi/reading/session/1cbc149c-7ce8-4c38-81d1-08eb19d7f163 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 713 | 313 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 712 | 326 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | mobile | reading | /vi/reading/session/2d48d025-8101-427e-90c9-4490ee63428e | GET | 200 | 712 | 322 | html | https://www.tarotnow.xyz/vi/reading/session/2d48d025-8101-427e-90c9-4490ee63428e |
| logged-out | desktop | auth-public | /vi/login | GET | 200 | 710 | 337 | html | https://www.tarotnow.xyz/vi/login |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 710 | 326 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 710 | 325 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 709 | 396 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | GET | 200 | 708 | 369 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 708 | 334 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 707 | 329 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 707 | 335 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | GET | 200 | 706 | 327 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 706 | 338 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 705 | 364 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 704 | 322 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | GET | 200 | 704 | 362 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 702 | 340 | html | https://www.tarotnow.xyz/vi/reader/apply |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-admin | desktop | /vi/gamification | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | desktop | /vi/gamification | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | /vi/gamification | https://www.tarotnow.xyz/vi/gamification |
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
| logged-in-admin | desktop | reading.init.spread_3: created 2fdb7785-2e4b-405a-aaf6-1bf1580e54f0. |
| logged-in-admin | desktop | reading.init.spread_5: created 7324b3b6-4edb-40ce-b187-45cc41ed705e. |
| logged-in-admin | desktop | reading.init.spread_10: created 1cbc149c-7ce8-4c38-81d1-08eb19d7f163. |
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
| logged-in-reader | desktop | reading.init.spread_3: created 998548bd-dcba-4914-b112-3e624fe09489. |
| logged-in-reader | desktop | reading.init.spread_5: created 96c410ed-b4e1-4ee9-b52c-16d361695568. |
| logged-in-reader | desktop | reading.init.spread_10: created 099798c5-6eea-4047-a8ae-5041245bdf06. |
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
| logged-in-admin | mobile | reading.init.spread_3: created 97c029e6-9b44-446f-8358-b48b6747b348. |
| logged-in-admin | mobile | reading.init.spread_5: created d107f458-aeb5-413a-a6ee-78c59b7c2edc. |
| logged-in-admin | mobile | reading.init.spread_10: created b3f31f62-ad26-4c13-8dd5-47cbfe88348b. |
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
| logged-in-reader | mobile | reading.init.spread_3: created 669dd000-19ea-49a3-8d21-b3530ac0bca9. |
| logged-in-reader | mobile | reading.init.spread_5: created 9e0b088e-2599-4414-a1e0-9b52fea9a1d2. |
| logged-in-reader | mobile | reading.init.spread_10: created 2d48d025-8101-427e-90c9-4490ee63428e. |
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

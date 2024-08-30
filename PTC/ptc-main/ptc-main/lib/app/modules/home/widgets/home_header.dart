import '../../../constants/constants.dart';

class HomeHeader extends StatelessWidget {
  const HomeHeader({super.key});

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: baseWidth,
      height: baseHeight * 0.3,
      child: Row(
        children: <Widget>[
          Expanded(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: <Widget>[
                Text(
                  "PTC",
                  style: TextStyle(
                    fontSize: baseWidth * 0.1,
                    fontWeight: FontWeight.bold,
                    color: AppColor.theme,
                  ),
                ),
                const Text(
                  "People Traffic Control",
                  style: TextStyle(
                    fontSize: 12,
                    fontWeight: FontWeight.bold,
                    color: AppColor.theme,
                  ),
                ),
              ],
            ),
          ),
          Image.asset("assets/images/splash_image.png")
        ],
      ),
    );
  }
}

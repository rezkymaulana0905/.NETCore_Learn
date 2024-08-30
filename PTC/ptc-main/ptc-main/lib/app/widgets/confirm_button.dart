import '../constants/constants.dart';

class ConfirmButton extends StatelessWidget {
  const ConfirmButton({
    super.key,
    required this.onPressed,
    required this.isLoading,
    this.skipDialog = false,
    this.width,
    this.height,
    this.text,
    this.color,
  });

  final VoidCallback onPressed;
  final RxBool isLoading;
  final bool skipDialog;
  final double? width;
  final double? height;
  final String? text;
  final Color? color;

  @override
  Widget build(BuildContext context) {
    return Obx(() {
      return ElevatedButton(
        onPressed: () {
          if (!isLoading.value) skipDialog ? onPressed() : _showDialog();
        },
        style: ElevatedButton.styleFrom(
          padding: const EdgeInsets.all(10),
          backgroundColor: color ?? Get.arguments['color'],
          fixedSize: Size(
            width ?? baseWidth * 0.8,
            height ?? baseHeight * 0.055,
          ),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(20),
          ),
        ),
        child: Align(
          alignment: Alignment.center,
          child: isLoading.value
              ? _buildLoadingButtonText()
              : _buildButtonText(text ?? "Confirm ${Get.arguments['mode']}"),
        ),
      );
    });
  }

  Widget _buildButtonText(String text) {
    return Text(
      text,
      style: TextStyle(
        color: Colors.white,
        fontSize: baseHeight * 0.025,
        fontWeight: FontWeight.w600,
      ),
    );
  }

  Widget _buildLoadingButtonText() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Widget>[
        const CircularProgressIndicator(color: Colors.white),
        const SizedBox(width: 20),
        _buildButtonText("Loading"),
      ],
    );
  }

  void _showDialog() {
    Get.dialog(
      barrierDismissible: false,
      arguments: Get.arguments,
      Dialog(
        backgroundColor: Colors.white,
        child: SizedBox(
          height: baseHeight * 0.2,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.center,
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: <Widget>[
              Text(
                "Confirm this ${Get.arguments['menu']}?",
                style: TextStyle(fontSize: baseWidth * 0.05),
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: <ElevatedButton>[
                  _buildDialogButton(
                    text: "No",
                    onPressed: () {},
                    backgroundColor: AppColor.leave,
                  ),
                  _buildDialogButton(
                    text: "Yes",
                    onPressed: onPressed,
                    backgroundColor: AppColor.enter,
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }

  ElevatedButton _buildDialogButton({
    required String text,
    required Color backgroundColor,
    required VoidCallback onPressed,
  }) {
    return ElevatedButton(
      onPressed: () {
        Get.back();
        onPressed();
      },
      style: ElevatedButton.styleFrom(
        backgroundColor: backgroundColor,
        fixedSize: Size(baseWidth * 0.3, baseHeight * 0.05),
      ),
      child: Text(
        text,
        textAlign: TextAlign.center,
        style: TextStyle(
          fontSize: baseWidth * 0.04,
          fontWeight: FontWeight.w600,
          color: Colors.white,
        ),
      ),
    );
  }
}

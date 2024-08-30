import '../../../constants/constants.dart';

class MessageBox extends StatelessWidget {
  const MessageBox({super.key, required this.color, required this.message});

  final Color color;
  final String message;

  @override
  Widget build(BuildContext context) {
    return Container(
      height: baseHeight * 0.5,
      width: baseWidth * 0.7,
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(20),
        border: Border.all(color: color, width: 5),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.end,
        children: <Widget>[_buildCloseButton(), _buildMessage()],
      ),
    );
  }

  Widget _buildCloseButton() {
    return GestureDetector(
      onTap: () => Get.back(),
      child: Container(
        margin: EdgeInsets.all(baseWidth * 0.05),
        height: baseWidth * 0.1,
        width: baseWidth * 0.1,
        alignment: Alignment.center,
        decoration: BoxDecoration(
          border: Border.all(width: 5),
          borderRadius: BorderRadius.circular(baseWidth * 0.1),
        ),
        child: Icon(Icons.close, size: baseWidth * 0.05),
      ),
    );
  }

  Widget _buildMessage() {
    return Text(
      message,
      textAlign: TextAlign.center,
      style: TextStyle(fontSize: baseWidth * 0.05, fontWeight: FontWeight.w600),
    );
  }
}

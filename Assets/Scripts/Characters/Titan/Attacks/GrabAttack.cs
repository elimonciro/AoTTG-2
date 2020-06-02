﻿using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class GrabAttack : Attack
    {
        public GrabAttack()
        {
            BodyParts = new[] {BodyPart.HandLeft, BodyPart.HandRight};
        }
        private string AttackAnimation { get; set; }
        private float attackCheckTimeA { get; set; }
        private float attackCheckTimeB { get; set; }
        private GameObject GrabbedTarget { get; set; }

        private BodyPart Hand { get; set; }
        public override bool CanAttack(MindlessTitan titan)
        {
            if (titan.TargetDistance >= titan.AttackDistance * 2) return false;
            if (IsDisabled(titan)) return false;
            Vector3 vector18 = titan.Target.transform.position - titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * 57.29578f;
            var between = -Mathf.DeltaAngle(angle, titan.gameObject.transform.rotation.eulerAngles.y - 90f);

            if (titan.Target.transform.position.y > titan.TitanBody.Neck.position.y - 3f * titan.Size
                && titan.TargetDistance < titan.AttackDistance * 0.5f)
            {
                if (Vector3.Distance(titan.Target.transform.position, titan.TitanBody.CheckOverhead.position) < (3.6f * titan.Size))
                {
                    if (between > 0f)
                    {
                        AttackAnimation = "grab_head_front_r";
                        Hand = BodyPart.HandRight;
                    }
                    else
                    {
                        AttackAnimation = "grab_head_front_l";
                        Hand = BodyPart.HandLeft;
                    }

                    if (IsDisabled(titan, Hand)) return false;
                    attackCheckTimeA = 0.38f;
                    attackCheckTimeB = 0.55f;
                    return true;
                }

                if (between > 0f)
                {
                    if (Vector3.Distance(titan.Target.transform.position, titan.TitanBody.CheckBackRight.position) < (2.8f * titan.Size))
                    {
                        AttackAnimation = "grab_head_back_r";
                        Hand = BodyPart.HandLeft;
                        if (IsDisabled(titan, Hand)) return false;
                        attackCheckTimeA = 0.45f;
                        attackCheckTimeB = 0.5f;
                        return true;
                    }
                }
                if (Vector3.Distance(titan.Target.transform.position, titan.TitanBody.CheckBackLeft.position) < (2.8f * titan.Size))
                {
                    AttackAnimation = "grab_head_back_l";
                    Hand = BodyPart.HandRight;
                    if (IsDisabled(titan, Hand)) return false;
                    attackCheckTimeA = 0.45f;
                    attackCheckTimeB = 0.5f;
                    return true;
                }
            }

            if (Mathf.Abs(between) < 90f && titan.TargetDistance < (titan.AttackDistance * 0.5f))
            {
                AttackAnimation = between > 0f
                    ? "grab_ground_front_r"
                    : "grab_ground_front_l";
                Hand = between > 0f
                    ? BodyPart.HandRight
                    : BodyPart.HandLeft;
                if (IsDisabled(titan, Hand)) return false;
                attackCheckTimeA = 0.37f;
                attackCheckTimeB = 0.6f;
                return true;
            }

            if (titan.TargetDistance < (titan.AttackDistance * 0.5f))
            {
                AttackAnimation = between > 0f
                    ? "grab_ground_back_r"
                    : "grab_ground_back_l";
                Hand = between > 0f
                    ? BodyPart.HandRight
                    : BodyPart.HandLeft;
                if (IsDisabled(titan, Hand)) return false;
                attackCheckTimeA = 0.34f;
                attackCheckTimeB = 0.49f;
                return true;
            }
            return false;
        }

        public bool CanAttackGroundFront(PlayerTitan titan, bool isLeftHand)
        {
            Hand = isLeftHand
                ? BodyPart.HandLeft
                : BodyPart.HandRight;
            if (titan.IsDisabled(Hand)) return false;
            AttackAnimation = isLeftHand
                ? "grab_ground_front_l"
                : "grab_ground_front_r";
            attackCheckTimeA = 0.37f;
            attackCheckTimeB = 0.6f;
            return true;
        }

        public bool CanAttackGroundBack(PlayerTitan titan, bool isLeftHand)
        {
            Hand = isLeftHand
                ? BodyPart.HandLeft
                : BodyPart.HandRight;
            if (titan.IsDisabled(Hand)) return false;
            AttackAnimation = isLeftHand
                ? "grab_ground_back_l"
                : "grab_ground_back_r";
            attackCheckTimeA = 0.34f;
            attackCheckTimeB = 0.49f;
            return true;
        }

        public bool CanAttackNape(PlayerTitan titan, bool isLeftHand)
        {
            Hand = isLeftHand
                ? BodyPart.HandLeft
                : BodyPart.HandRight;
            if (titan.IsDisabled(Hand)) return false;
            AttackAnimation = isLeftHand
                ? "grab_head_back_l"
                : "grab_head_back_r";
            attackCheckTimeA = 0.45f;
            attackCheckTimeB = 0.5f;
            return true;
        }

        public override void Execute(MindlessTitan titan)
        {
            if (IsFinished) return;
            if (!titan.Animation.IsPlaying(AttackAnimation))
            {
                titan.CrossFade(AttackAnimation, 0.1f);
                return;
            }

            if (IsDisabled(titan, Hand))
            {
                IsFinished = true;
                return;
            }

            if (titan.Animation[AttackAnimation].normalizedTime >= this.attackCheckTimeA && titan.Animation[AttackAnimation].normalizedTime <= this.attackCheckTimeB && GrabbedTarget == null)
            {
                var hand = Hand == BodyPart.HandLeft
                    ? titan.TitanBody.HandLeft
                    : titan.TitanBody.HandRight;

                GameObject grabTarget = checkIfHitHand(hand, titan.Size);
                if (grabTarget != null)
                {
                    if (Hand == BodyPart.HandLeft)
                    {
                        eatSetL(titan, grabTarget);
                        GrabbedTarget = grabTarget;
                    }
                    else
                    {
                        eatSet(titan, grabTarget);
                        GrabbedTarget = grabTarget;
                    }
                }
            }
            if (titan.Animation[AttackAnimation].normalizedTime >= 1f)
            {
                if (GrabbedTarget != null)
                {
                    titan.OnTargetGrabbed(GrabbedTarget, Hand == BodyPart.HandLeft);
                }
                IsFinished = true;
            }
        }

        private void eatSet(MindlessTitan titan, GameObject grabTarget)
        {
            if (((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || !titan.photonView.isMine)) || !grabTarget.GetComponent<Hero>().isGrabbed)
            {
                titan.Grab(false);
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && titan.photonView.isMine)
                {
                    titan.photonView.RPC("Grab", PhotonTargets.Others, false);
                    object[] parameters = new object[] { "grabbed" };
                    grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, parameters);
                    object[] objArray2 = new object[] { titan.photonView.viewID, false };
                    grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, objArray2);
                }
                else
                {
                    grabTarget.GetComponent<Hero>().grabbed(titan.gameObject, false);
                    grabTarget.GetComponent<Hero>().GetComponent<Animation>().Play("grabbed");
                }
            }
        }

        private void eatSetL(MindlessTitan titan, GameObject grabTarget)
        {
            if (((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || !titan.photonView.isMine)) || !grabTarget.GetComponent<Hero>().isGrabbed)
            {
                titan.Grab(true);
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && titan.photonView.isMine)
                {
                    titan.photonView.RPC("Grab", PhotonTargets.Others, true);
                    object[] parameters = new object[] { "grabbed" };
                    grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, parameters);
                    object[] objArray2 = new object[] { titan.photonView.viewID, true };
                    grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, objArray2);
                }
                else
                {
                    grabTarget.GetComponent<Hero>().grabbed(titan.gameObject, true);
                    grabTarget.GetComponent<Hero>().GetComponent<Animation>().Play("grabbed");
                }
            }
        }
    }
}
